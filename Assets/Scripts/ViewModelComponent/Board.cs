using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();

    public void CreateTileAt (Point p, Tile t) {
        if (units.ContainsKey (p))
            return;

        Tile tile = Instantiate (t, new Vector3 (p.x, p.y, 0), Quaternion.identity);

        tiles.Add (tile.Position, tile);
    }

    public void CreateUnitAt (Point p, Unit u) {
        if (units.ContainsKey (p))
            return;

        Unit unit = Instantiate (u, new Vector3 (p.x, p.y, 0), Quaternion.identity);

        units.Add (unit.Position, unit);
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
            Destroy (units[p], .25f);
        }
    }

    public void DeleteTileAt (Point p) {
        if (tiles.ContainsKey (p)) {
            Destroy (tiles[p], .25f);
        }
    }

    public void Load () {
        // if (Container.levelData == null)
        //     return;

        // foreach (TileSpawnData data in Container.levelData.tiles) {
        //     PlaceTile (data.location, data.tile);
        // }

        // foreach (UnitSpawnData data in Container.levelData.units) {
        //     Unit unit = BoardHelper.CreateUnit (
        //         Container.transform, this,
        //         data.location, data.unit);

        //     units.Add (unit.Position, unit);
        // }
    }
}