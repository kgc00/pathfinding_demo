using UnityEngine;

[System.Serializable]
public class Unit : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }
    public Unit u;

    public void Initialize (Board board, Point pos) {
        Position = pos;
        Board = board;
        u = this;
    }
}