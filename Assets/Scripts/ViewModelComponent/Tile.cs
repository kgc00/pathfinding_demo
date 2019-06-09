using UnityEngine;

public class Tile : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }

    public Tile (Board board, Point pos) {
        Position = pos;
        Board = board;
    }
}