using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardVisuals : MonoBehaviour
{
    Board board;
    public void Initialize(Board board)
    {
        this.board = board;
    }

    public static void RenderTileHighlights(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            // Animator instance = Instantiate(Resources.Load("Test/DIRT", typeof(Animator))) as Animator;

            // Animator a = tile.gameObject.AddComponent<Animator>();
            // a = instance;
            // Debug.Log(instance);
        }
    }
}
