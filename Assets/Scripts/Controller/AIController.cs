using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public override void Initialize(Board board, Unit owner, WalkingMovement movement)
    {
        base.Initialize(board, owner, movement);
        StartAILogic();
    }

    public void StartAILogic()
    {
        IdleState();
    }

    public override void IdleState()
    {

        owner.SetState(UnitStates.PREPARING);
    }

    public override IEnumerator PrepState()
    {
        Debug.Log("called");
        List<PathfindingData> data = movement.GetTilesInRange(board);
        List<Tile> tiles = new List<Tile>();
        data.ForEach(element =>
        {
            tiles.Add(element.tile);
        });
        BoardVisuals.RenderTileHighlights(tiles);
        Tile selected = tiles[Random.Range(0, tiles.Count)];
        selected.GetComponent<MeshRenderer>().enabled = false;
        // movement.Traverse(selected);
        yield return null;
    }

    public override void ActingState()
    {
        Debug.Log("Acting");
    }

    public override void CooldownState()
    {
        Debug.Log("Cooldown");
    }
}

// foreach (Tile tile in tiles)
// {
//     tile.GetComponent<MeshRenderer>().enabled = false;
// }