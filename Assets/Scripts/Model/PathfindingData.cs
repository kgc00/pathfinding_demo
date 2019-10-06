public class PathfindingData {
    public Tile tile { get; private set; }
    public ShadowTile shadow { get; private set; }
    public PathfindingData (Tile t, ShadowTile st) {
        this.tile = t;
        this.shadow = st;
    }
}