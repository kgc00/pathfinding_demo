using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }
    public bool isWalkable;
    [SerializeField] public TileTypes TypeReference;

    public void Initialize (Board board, Point pos, TileTypes r) {
        Position = pos;
        Board = board;
        TypeReference = r;
    }
}