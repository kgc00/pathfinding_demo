using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller {
    public override void Initialize (Board board, Unit owner, WalkingMovement movement) {
        base.Initialize (board, owner, movement);
        StartAILogic ();
    }

    public void StartAILogic () {
        IdleState ();
    }

    public override void IdleState () {
        owner.SetState (UnitStates.PREPARING);
    }

    public override IEnumerator PrepState () {
        List<PathfindingData> data = movement.GetTilesInRange (board);
        List<Tile> tiles = new List<Tile> ();
        data.ForEach (element => {
            tiles.Add (element.tile);
        });
        BoardVisuals.RenderTileHighlights (tiles);
        PathfindingData selected = data[Random.Range (0, data.Count)];
        // selected.tile.GetComponent<MeshRenderer> ().enabled = false;
        StartCoroutine (movement.Traverse (data, selected, () => owner.SetState (UnitStates.COOLDOWN)));
        owner.SetState (UnitStates.ACTING);
        yield return null;
    }

    public override void ActingState () { }

    public override void CooldownState () {
        StartCoroutine (countdown (Random.Range (0, 4), () => owner.SetState (UnitStates.IDLE)));
    }

    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        int safety = 0;
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
            if (safety > 999999) {
                Debug.Log ("Break");
                break;
            }
        }
        onComplete ();
    }
}