using UnityEngine;

public class Unit : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }

    public void Initialize (Board board, Point pos) {
        Position = pos;
        Board = board;
    }
}