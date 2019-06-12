using UnityEngine;

[System.Serializable]
public class Unit : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }

    [SerializeField] public UnitTypes TypeReference;

    public void Initialize (Board board, Point pos, UnitTypes r) {
        Position = pos;
        Board = board;
        TypeReference = r;
    }
}