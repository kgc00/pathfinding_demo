using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }
    public Unit OccupiedBy { get; protected set; }

    public bool isWalkable { get; protected set; }

    [SerializeField] public TileTypes TypeReference { get; protected set; }
    public Vector3 Center { get { return new Vector3 (Position.x, Position.y, -2); } }

    public virtual void Initialize (Board board, Point pos, TileTypes r) {
        Position = pos;
        Board = board;
        TypeReference = r;
        if (TypeReference != TileTypes.WALL) {
            isWalkable = true;
        } else {
            isWalkable = false;
        }
    }

    public virtual void SetOccupied (Unit occupier) {
        OccupiedBy = occupier;
    }
    public void SetUnoccupied () {
        OccupiedBy = null;
    }

    public bool IsOccupied () {
        // Debug.Log (string.Format ("tiles occupied is {0}", OccupiedBy.ToString ()));
        return (OccupiedBy != null);
    }
    public bool IsOccupiedBy (Unit u) {
        return (u == OccupiedBy);
    }
}