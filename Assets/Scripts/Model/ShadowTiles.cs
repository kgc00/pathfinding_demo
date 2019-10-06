public class ShadowTile {
    public int distance { get; private set; }
    public Point position { get; private set; }
    public ShadowTile previous { get; private set; }
    public Tile tile { get; private set; }
    public ShadowTile (int dist, Point p, ShadowTile prev, Tile t) {
        this.distance = dist;
        this.position = p;
        this.previous = prev;
        this.tile = t;
    }

    ///<summary>
    /// Assigns the previous property.
    /// <param name="previous"> ShadowTile to assign as previous.</param>
    ///</summary>
    public void AssignPrevious (ShadowTile previous) {
        this.previous = previous;
    }
}