using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit>();
    public LevelData levelData;

    private void Awake()
    {
        if (levelData != null)
        {
            Load();
        }
    }

    public Tile CreateTileAt(Point p, TileTypes type)
    {
        if (tiles.ContainsKey(p))
        {
            DeleteTileAt(p);
        }

        Tile tile = null;
        if (type == TileTypes.DIRT)
        {
            Tile instance = Instantiate(Resources.Load("Prefabs/Dirt", typeof(Tile)),
                new Vector3(p.x, p.y, 0), Quaternion.identity) as Tile;
            tile = instance;
        }
        else if (type == TileTypes.WALL)
        {
            Tile instance = Instantiate(Resources.Load("Prefabs/Wall", typeof(Tile)),
                new Vector3(p.x, p.y, 0), Quaternion.identity) as Tile;
            tile = instance;
        }

        if (tile == null)
        {
            Debug.LogError("tile should not be null and is.");
            return null;
        }

        tile.Initialize(this, p, type);
        tile.gameObject.name = type.ToString();
        tiles.Add(tile.Position, tile);
        tile.transform.parent = gameObject.transform;
        return tile;

    }

    public Unit CreateUnitAt(Point p, UnitTypes type)
    {
        if (units.ContainsKey(p))
        {
            DeleteUnitAt(p);
        }

        Unit unit = null;
        if (type == UnitTypes.HERO)
        {
            Unit instance = Instantiate(Resources.Load("Prefabs/Hero", typeof(Unit)),
                new Vector3(p.x, p.y, -2), Quaternion.identity) as Unit;
            unit = instance;
        }
        else if (type == UnitTypes.MONSTER)
        {
            Unit instance = Instantiate(Resources.Load("Prefabs/Monster", typeof(Unit)),
                new Vector3(p.x, p.y, -2), Quaternion.identity) as Unit;
            unit = instance;
        }

        if (unit == null)
        {
            Debug.LogError("unit should not be null and is.");
            return null;
        }

        unit.GetComponent<Unit>().Initialize(this, p, type);
        unit.gameObject.name = type.ToString();
        unit.transform.parent = gameObject.transform;
        units.Add(unit.Position, unit);
        return unit;
    }

    public Tile TileAt(Point p)
    {
        Tile tile = null;
        tiles.TryGetValue(p, out tile);
        return tile;
    }

    public Unit UnitAt(Point p)
    {
        Unit unit = null;
        units.TryGetValue(p, out unit);
        return unit;
    }

    public void DeleteUnitAt(Point p)
    {
        if (units.ContainsKey(p))
        {
            Destroy(units[p].gameObject, .25f);
            units.Remove(p);
        }
    }

    public void DeleteTileAt(Point p)
    {
        if (tiles.ContainsKey(p))
        {
            Tile tile = tiles[p];
            tiles.Remove(p);
            Destroy(tile.gameObject);

        }
    }

    public void Load()
    {
        if (levelData == null)
            return;

        foreach (TileSpawnData data in levelData.tiles)
        {
            CreateTileAt(data.location, data.tileRef);
        }

        foreach (UnitSpawnData data in levelData.units)
        {
            CreateUnitAt(data.location, data.unitRef);
        }
    }
}