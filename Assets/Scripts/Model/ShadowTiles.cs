public class ShadowTile {
    public int distance;
    public Point position;
    public ShadowTile previous;
    public Tile tile;
    public ShadowTile nextAvailableInPool { get; private set; }
    public ShadowTile (int d, Point p, ShadowTile prev, Tile t) {
        this.distance = d;
        this.position = p;
        this.previous = prev;
        this.tile = t;
    }

    // ~ShadowTile () {
    //     UnityEngine.Debug.Log ("Should not see this");
    //     UnityEngine.Debug.Log ("next: " + nextAvailableInPool);
    // }

    public void SetNext (ShadowTile st) {
        this.nextAvailableInPool = st;
    }

    public void ClearLocalData () {
        this.distance = int.MaxValue;
        this.position = new Point (-99, -99);
        this.previous = null;
        this.tile = null;
    }

    public void AssignValues (int d, Point p, ShadowTile prev, Tile t) {
        this.distance = d;
        this.position = p;
        this.previous = prev;
        this.tile = t;
    }
}