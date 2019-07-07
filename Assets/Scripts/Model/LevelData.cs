using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData : ScriptableObject {

    public List<TileSpawnData> tiles;

    public List<UnitSpawnData> units;
}

[Serializable]
public struct TileSpawnData {
    public Point location;
    public TileTypes tileRef;

    public TileSpawnData (Point _location, TileTypes _tileRef) {
        this.location = _location;
        this.tileRef = _tileRef;
    }
}

[Serializable]
public struct UnitSpawnData {
    public Point location;
    public UnitTypes unitRef;

    public UnitSpawnData (Point _location, UnitTypes _unitRef) {
        this.location = _location;
        this.unitRef = _unitRef;
    }
}