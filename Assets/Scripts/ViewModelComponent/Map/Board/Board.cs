using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();
    public Dictionary<Point, Tile> Tiles => tiles;
    public Dictionary<Point, Unit> Units => units;
    public LevelData levelData;
    BoardVisuals vis;
    BoardPathfinding bpf;
    RangeUtil rangeUtil;
    public UnitFactory UnitFactory;
    public BoardPathfinding Pathfinding => bpf;
    private Area area;
    public Point[] Dirs => new Point[4] {
        new Point (0, 1),
        new Point (0, -1),
        new Point (1, 0),
        new Point (-1, 0)
    };
    private Transform tileWrapper;

    public void Initialize (LevelData data, Area a) {
        this.area = a;
        levelData = data;

        var unitWrapper = new GameObject ();
        unitWrapper.transform.SetParent (gameObject.transform);
        unitWrapper.name = "Units";

        var tileWrapper = new GameObject ();
        tileWrapper.transform.SetParent (gameObject.transform);
        tileWrapper.name = "Tiles";
        this.tileWrapper = tileWrapper.transform;

        vis = gameObject.AddComponent<BoardVisuals> ();
        bpf = gameObject.AddComponent<BoardPathfinding> ();
        rangeUtil = gameObject.AddComponent<RangeUtil> ();
        UnitFactory = gameObject.AddComponent<UnitFactory> ();
        vis.Initialize (this);
        bpf.Initialize (this);
        rangeUtil.Initialize (this);
        UnitFactory.Initialize (this, unitWrapper.transform);
        Unit.onUnitDeath += DeleteUnit;
        Load ();
    }

    public void CreateUnitFactory () {
        var unitWrapper = new GameObject ();
        unitWrapper.transform.SetParent (gameObject.transform);
        unitWrapper.name = "Units";

        var tileWrapper = new GameObject ();
        tileWrapper.transform.SetParent (gameObject.transform);
        tileWrapper.name = "Tiles";
        this.tileWrapper = tileWrapper.transform;

        this.UnitFactory = gameObject.AddComponent<UnitFactory> ();
        this.UnitFactory.Initialize (this, unitWrapper.transform);
    }

    ~Board () {
        Unit.onUnitDeath -= DeleteUnit;
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
        } else if (type == TileTypes.ENTRANCE) {
            Tile instance = Instantiate (Resources.Load ("Prefabs/Entrance", typeof (Tile)),
                new Vector3 (p.x, p.y, 0), Quaternion.identity) as Tile;
            tile = instance;
        } else if (type == TileTypes.WALL) {
            Tile instance = Instantiate (Resources.Load ("Prefabs/Wall", typeof (Tile)),
                new Vector3 (p.x, p.y, 0), Quaternion.identity) as Tile;
            tile = instance;
        } else if (type == TileTypes.BOSS_ENTRANCE) {
            Tile instance = Instantiate (Resources.Load ("Prefabs/Boss Entrance", typeof (Tile)),
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
        tile.transform.SetParent (tileWrapper);
        return tile;

    }

    public Unit CreateUnitAt (Point p, UnitTypes type) {
        if (units.ContainsKey (p)) {
            DeleteUnitAt (p);
        }

        Unit unit = UnitFactory.CreateUnitFromType (p, type);

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
    public Unit UnitFromStartLocation (Point p) {
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

    // called by onUnitDeath action
    // useful when we don't know the unit's original dictionary key
    private void DeleteUnit (Unit u) {
        Point p = KeyFromUnit (u);
        DeleteUnitAt (p);
    }

    public void DeleteUnitAt (Point p) {
        if (units.ContainsKey (p)) {
            Destroy (units[p].gameObject, .25f);
            units.Remove (p);
        }
    }

    public Point KeyFromUnit (Unit unit) {
        return units.FirstOrDefault (u => u.Value == unit).Key;
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