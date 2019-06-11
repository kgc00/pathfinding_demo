using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();

    public Tile CreateTileAt (Point p, Tile t) {

        if (tiles.ContainsKey (p)) {
            DeleteTileAt (p);
        }
        if (!tiles.ContainsKey (p)) {
            Tile tile = Instantiate (t, new Vector3 (p.x, p.y, 0), Quaternion.identity);
            tile.Initialize (this, p, t.TypeReference);
            tile.gameObject.name = t.ToString ();
            tiles.Add (tile.Position, tile);
            return tile;
        } else {
            Debug.Log ("null");
            return null;
        }
    }

    public Unit CreateUnitAt (Point p, Unit u) {
        if (units.ContainsKey (p)) {
            DeleteUnitAt (p);
        }
        Unit unit = Instantiate (u, new Vector3 (p.x, p.y, -2), Quaternion.identity);
        unit.Initialize (this, p);
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