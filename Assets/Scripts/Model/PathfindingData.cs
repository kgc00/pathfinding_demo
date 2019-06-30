public class PathfindingData {
    public Tile tile;
    public ShadowTile shadow;
    public PathfindingData nextAvailableInPool { get; private set; }
    public PathfindingData (Tile t, ShadowTile st) {
        this.tile = t;
        this.shadow = st;
    }

    public void SetNext (PathfindingData next) {
        this.nextAvailableInPool = next;
    }
}