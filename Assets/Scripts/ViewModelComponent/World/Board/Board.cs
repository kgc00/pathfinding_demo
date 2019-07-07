using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();
    public Dictionary<Point, Tile> Tiles => tiles;
    public Dictionary<Point, Unit> Units => units;
    public LevelData levelData;
    BoardVisuals vis;
    BoardPathfinding bpf;
    public BoardPathfinding Pathfinding => bpf;
    public Point[] Dirs => new Point[4] {
        new Point (0, 1),
        new Point (0, -1),
        new Point (1, 0),
        new Point (-1, 0)
    };

    private void Awake () {
        vis = gameObject.AddComponent<BoardVisuals> ();
        bpf = gameObject.AddComponent<BoardPathfinding> ();
        vis.Initialize (this);
        bpf.Initialize (this);
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

    public Unit LevelEditorCreateUnitAt (Point p, UnitTypes type) {
        if (units.ContainsKey (p)) {
            DeleteUnitAt (p);
        }

        Unit unit = null;
        if (type == UnitTypes.HERO) {
            Hero instance = Instantiate (Resources.Load ("Prefabs/Hero", typeof (Hero)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Hero;
            unit = instance as Unit;
        } else if (type == UnitTypes.MONSTER) {
            Monster instance = Instantiate (Resources.Load ("Prefabs/Monster", typeof (Monster)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
            unit = instance as Unit;
        } else if (type == UnitTypes.DEBUG) {
            Monster instance = Instantiate (Resources.Load ("Prefabs/Debug", typeof (Monster)),
                new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
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
}