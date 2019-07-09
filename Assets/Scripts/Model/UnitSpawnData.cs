using System;

[Serializable]
public struct UnitSpawnData {
    public Point location;
    public UnitTypes unitRef;

    public UnitSpawnData (Point _location, UnitTypes _unitRef) {
        this.location = _location;
        this.unitRef = _unitRef;
    }
}