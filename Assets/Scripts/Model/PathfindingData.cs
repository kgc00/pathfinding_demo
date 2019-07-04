public class PathfindingData {
    public Tile tile;
    public ShadowTile shadow;
    public PathfindingData nextAvailableInPool { get; private set; }
    public PathfindingData (Tile t, ShadowTile st) {
        this.tile = t;
        this.shadow = st;
    }
    // ~PathfindingData () {
    //     UnityEngine.Debug.Log ("Should not see this");
    //     UnityEngine.Debug.Log ("next: " + nextAvailableInPool);
    // }

    public void SetNext (PathfindingData next) {
        this.nextAvailableInPool = next;
    }

    public void ClearLocalData () {
        this.shadow = null;
        this.tile = null;
    }
}