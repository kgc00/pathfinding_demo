using System;

public class ShadowTile {
    public int Distance {get; private set;}
    public Point Position {get; private set;}
    public ShadowTile Previous {get; private set;}
    public Tile Tile{get; private set;}
    public ShadowTile (int dist, Point p, ShadowTile prev, Tile t) {
        this.Distance = dist;
        this.Position = p;
        this.Previous = prev;
        this.Tile = t;
    }

    internal void SetPrevious(ShadowTile shadow)
    {
        Previous = shadow;
    }
}