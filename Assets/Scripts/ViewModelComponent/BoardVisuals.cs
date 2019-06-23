using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardVisuals : MonoBehaviour {
    Board board;
    List<Renderer> tilesToHighlight;
    public void Initialize (Board board) {
        this.board = board;
        tilesToHighlight = new List<Renderer> ();
    }

    // public static void AddTileToHighlights (List<Tile> tiles) {
    //     foreach (Tile tile in tiles) {
    //         if (!tilesToHighlight.Contains (tile.GetComponent<Renderer> ()))
    //             tilesToHighlight.Add (tile.GetComponent<Renderer> ());
    //     }
    // }

    // public static void RemoveTileFromHighlights (List<Tile> tiles) {
    //     foreach (Tile tile in tiles) {
    //         if (!tilesToHighlight.Contains (tile.GetComponent<Renderer> ()))
    //             tilesToHighlight.Remove (tile.GetComponent<Renderer> ());
    //     }
    // }
}