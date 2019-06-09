using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : ScriptableObject {

    public List<TileSpawnData> tiles;

    public List<UnitSpawnData> units;
}

[System.Serializable]
public struct TileSpawnData {
    public Point location;
    public Tile tile;

    public TileSpawnData (Point _location, Tile _tile) {
        this.location = _location;
        this.tile = _tile;
    }
}

[System.Serializable]
public struct UnitSpawnData {
    public Point location;
    public Unit unit;

    public UnitSpawnData (Point _location, Unit _unit) {
        this.location = _location;
        this.unit = _unit;
    }
}