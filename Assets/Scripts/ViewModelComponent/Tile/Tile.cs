using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }
    public Unit OccupiedBy;
    public bool isWalkable;
    [SerializeField] public TileTypes TypeReference;
    private Renderer myRend;
    public Vector3 center { get { return new Vector3 (Position.x, Position.y, -2); } }

    public virtual void Initialize (Board board, Point pos, TileTypes r) {
        Position = pos;
        Board = board;
        TypeReference = r;
        myRend = this.GetComponent<Renderer> ();
    }

    public virtual void SetOccupied (Unit occupier) {
        OccupiedBy = occupier;
    }
    public void SetUnoccupied () {
        OccupiedBy = null;
    }

    protected virtual void OnOccupied (Unit occupier) {

    }

    public bool IsOccupied () {
        return (OccupiedBy != null);
    }
    public bool IsOccupiedBy (Unit u) {
        return (u == OccupiedBy);
    }
}