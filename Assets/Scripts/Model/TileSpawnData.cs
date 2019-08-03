using System;

[Serializable]
public struct TileSpawnData {
    public Point location;
    public TileTypes tileRef;

    public TileSpawnData (Point _location, TileTypes _tileRef) {
        this.location = _location;
        this.tileRef = _tileRef;
    }
}