using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();
    public LevelData levelData;
    BoardVisuals vis;
    Point[] dirs = new Point[4] {
        new Point (0, 1),
        new Point (0, -1),
        new Point (1, 0),
        new Point (-1, 0)
    };

    private void Awake () {
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
            Unit instance = Instantiate (Resources.Load ("Prefabs/Hero", typeof (Unit)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Unit;
            unit = instance;
        } else if (type == UnitTypes.MONSTER) {
            Monster instance = Instantiate (Resources.Load ("Prefabs/Monster", typeof (Monster)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
            instance.GetComponent<Monster> ().Initialize (this, p, type);
            unit = instance as Unit;
        } else if (type == UnitTypes.DEBUG) {
            Monster instance = Instantiate (Resources.Load ("Prefabs/Debug", typeof (Monster)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
            instance.GetComponent<Monster> ().Initialize (this, p, type);
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

    public Unit UnitAt (Point p) {
        Unit unit = null;
        units.TryGetValue (p, out unit);
        return unit;
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
        Debug.Log ("start: " + start.Position);
        List<ShadowTile> shadows = new List<ShadowTile> ();
        shadows.Add (new ShadowTile (int.MaxValue, start.Position, null, start));

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

                if (addTile (currentShadow, nextTile)) {
                    ShadowTile checkedShadow = new ShadowTile (currentShadow.distance + 1, nextTile.Position, currentShadow, nextTile);
                    checkNext.Enqueue (checkedShadow);
                    shadows.Add (checkedShadow);
                }
            }

            // black magic
            if (checkNow.Count == 0)
                SwapReference (ref checkNow, ref checkNext);
        }

        List<PathfindingData> retValue = new List<PathfindingData> ();
        shadows.ForEach (shadow => retValue.Add (
            new PathfindingData (shadow.tile, shadow)
        ));
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