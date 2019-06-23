public struct PathfindingData : System.IEquatable<PathfindingData> {
    public Tile tile;
    public ShadowTile shadow;

    public PathfindingData (Tile t, ShadowTile st) {
        this.tile = t;
        this.shadow = st;
    }

    public static bool operator == (PathfindingData a, PathfindingData b) {
        return a.tile == b.tile && a.shadow == b.shadow;
    }
    public static bool operator != (PathfindingData a, PathfindingData b) {
        return !(a == b);
    }
    public override bool Equals (object obj) {
        if (obj is PathfindingData) {
            PathfindingData d = (PathfindingData) obj;
            return tile == d.tile && shadow == d.shadow;
        }
        return false;
    }
    public bool Equals (PathfindingData d) {
        return tile == d.tile && shadow == d.shadow;
    }
}