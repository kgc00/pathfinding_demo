public struct PathfindingData
{
    public Tile tile;
    public ShadowTile shadow;

    public PathfindingData(Tile t, ShadowTile st)
    {
        this.tile = t;
        this.shadow = st;
    }
}