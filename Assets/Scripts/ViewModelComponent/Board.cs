using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();
    public LevelData levelData;
    BoardVisuals vis;
    public static PathFindingDataPool pfdPool;
    public static ShadowTilePool stPool;
    Point[] dirs = new Point[4] {
        new Point (0, 1),
        new Point (0, -1),
        new Point (1, 0),
        new Point (-1, 0)
    };

    private void Awake () {
        // must initialize the pool before we load units
        pfdPool = new PathFindingDataPool ();
        stPool = new ShadowTilePool ();
        vis = gameObject.AddComponent<BoardVisuals> ();
        vis.Initialize (this);
        if (levelData != null) {
            Load ();
        }
    }

    public Tile CreateTileAt (Point p, TileTypes type) {
        if (tiles.ContainsKey (p)) {
            DeleteTileAt (p);
        }

        Tile tile = null;
        if (type == TileTypes.DIRT) {
            Tile instance = Instantiate (Resources.Load ("Prefabs/Dirt", typeof (Tile)),
                new Vector3 (p.x, p.y, 0), Quaternion.identity) as Tile;
            tile = instance;
        } else if (type == TileTypes.WALL) {
            Tile instance = Instantiate (Resources.Load ("Prefabs/Wall", typeof (Tile)),
                new Vector3 (p.x, p.y, 0), Quaternion.identity) as Tile;
            tile = instance;
        }

        if (tile == null) {
            Debug.LogError ("tile should not be null and is.");
            return null;
        }

        tile.Initialize (this, p, type);
        tile.gameObject.name = type.ToString ();
        tiles.Add (tile.Position, tile);
        tile.transform.parent = gameObject.transform;
        return tile;

    }

    public Unit CreateUnitAt (Point p, UnitTypes type) {
        if (units.ContainsKey (p)) {
            DeleteUnitAt (p);
        }

        Unit unit = null;
        if (type == UnitTypes.HERO) {
            Hero instance = Instantiate (Resources.Load ("Prefabs/Hero", typeof (Hero)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Hero;
            instance.GetComponent<Hero> ().Initialize (this, type);
            unit = instance as Unit;
        } else if (type == UnitTypes.MONSTER) {
            Monster instance = Instantiate (Resources.Load ("Prefabs/Monster", typeof (Monster)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
            instance.GetComponent<Monster> ().Initialize (this, type);
            unit = instance as Unit;
        } else if (type == UnitTypes.DEBUG) {
            Monster instance = Instantiate (Resources.Load ("Prefabs/Debug", typeof (Monster)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
            instance.GetComponent<Monster> ().Initialize (this, type);
            unit = instance as Unit;
        }

        if (unit == null) {
            Debug.LogError ("unit should not be null and is.");
            return null;
        }

        unit.gameObject.name = type.ToString ();
        unit.transform.parent = gameObject.transform;
        units.Add (unit.Position, unit);
        return unit;
    }

    public Tile TileAt (Point p) {
        Tile tile = null;
        tiles.TryGetValue (p, out tile);
        return tile;
    }

    // this method uses the point key in the units dictionary to find a unit
    // therefore it is only for level creation, not runtime usage
    public Unit UnitAtForBoardCreation (Point p) {
        Unit unit = null;
        units.TryGetValue (p, out unit);
        return unit;
    }

    public Unit UnitAt (Point p) {
        foreach (KeyValuePair<Point, Unit> pair in units) {
            if (pair.Value.Position == p) {
                return pair.Value;
            }
        }
        return null;
    }

    public void DeleteUnitAt (Point p) {
        if (units.ContainsKey (p)) {
            Destroy (units[p].gameObject, .25f);
            units.Remove (p);
        }
    }

    public void DeleteTileAt (Point p) {
        if (tiles.ContainsKey (p)) {
            Tile tile = tiles[p];
            tiles.Remove (p);
            Destroy (tile.gameObject);

        }
    }

    public void Load () {
        if (levelData == null)
            return;

        foreach (TileSpawnData data in levelData.tiles) {
            CreateTileAt (data.location, data.tileRef);
        }

        foreach (UnitSpawnData data in levelData.units) {
            CreateUnitAt (data.location, data.unitRef);
        }
    }

    public List<PathfindingData> Search (Tile start, Func<ShadowTile, Tile, bool> addTile) {
        List<ShadowTile> shadows = new List<ShadowTile> ();
        var startShadow = stPool.RetrieveItem ();
        startShadow.AssignValues (int.MaxValue, start.Position, null, start);
        shadows.Add (startShadow);

        Queue<ShadowTile> checkNext = new Queue<ShadowTile> ();
        Queue<ShadowTile> checkNow = new Queue<ShadowTile> ();
        shadows[0].distance = 0;

        checkNow.Enqueue (shadows[0]);
        while (checkNow.Count > 0) {
            ShadowTile currentShadow = checkNow.Dequeue ();
            for (int i = 0; i < 4; ++i) {
                Tile nextTile = GetTile (currentShadow.position + dirs[i]);
                if (nextTile == null) {
                    continue;
                }

                ShadowTile oldShadow = shadows.Find (shadow => shadow.tile == nextTile);
                if (oldShadow != null) {
                    if (oldShadow.distance <= currentShadow.distance + 1) {
                        continue;
                    }
                    continue;
                }

                // use strategy pattern to define unique filtering logic for each request
                if (addTile (currentShadow, nextTile)) {
                    ShadowTile checkedShadow = stPool.RetrieveItem ();
                    checkedShadow.AssignValues (currentShadow.distance + 1, nextTile.Position, currentShadow, nextTile);
                    checkNext.Enqueue (checkedShadow);
                    shadows.Add (checkedShadow);
                }
            }

            // swap the ref between empty and full queue's to avoid re-allocating
            if (checkNow.Count == 0)
                SwapReference (ref checkNow, ref checkNext);
        }

        List<PathfindingData> retValue = new List<PathfindingData> ();

        // use a pool of pathfinding data
        shadows.ForEach (shadow => {
            var newpfd = pfdPool.RetrieveItem ();
            newpfd.shadow = shadow;
            newpfd.tile = shadow.tile;
            retValue.Add (newpfd);
        });

        return retValue;
    }

    public Tile GetTile (Point p) {
        return tiles.ContainsKey (p) ? tiles[p] : null;
    }

    void SwapReference (ref Queue<ShadowTile> a, ref Queue<ShadowTile> b) {
        Queue<ShadowTile> temp = a;
        a = b;
        b = temp;
    }
}