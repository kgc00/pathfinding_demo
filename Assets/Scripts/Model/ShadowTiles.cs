public class ShadowTile
{
    public int distance;
    public Point position;
    public ShadowTile previous;
    public Tile tile;
    public ShadowTile(int d, Point p, ShadowTile prev, Tile t)
    {
        this.distance = d;
        this.position = p;
        this.previous = prev;
        this.tile = t;
    }
}