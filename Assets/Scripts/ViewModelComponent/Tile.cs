using UnityEngine;

public class Tile : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }
    public bool isWalkable;

    public void Initialize (Board board, Point pos) {
        Position = pos;
        Board = board;
    }
}