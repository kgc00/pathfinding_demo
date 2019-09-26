public class PathfindingData {
    public Tile Tile {get; private set;}
    public ShadowTile Shadow {get; private set;}
    public PathfindingData (Tile t, ShadowTile st) {
        this.Tile = t;
        this.Shadow = st;
    }
}