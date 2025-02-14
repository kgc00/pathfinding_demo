using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIDriver : SimpleDriver {
    public UnitStates State;
    public override void Initialize (Board board, Unit owner, MovementComponent movement) {
        base.Initialize (board, (Monster) owner, (WalkingMovement) movement);
        SetState (UnitStates.IDLE);
    }

    public void SetState (UnitStates state) {
        this.State = state;
        switch (state) {
            case UnitStates.IDLE:
                EnterIdle ();
                break;
            case UnitStates.PREPARING:
                EnterPrep ();
                break;
            case UnitStates.ACTING:
                EnterActing ();
                break;
            case UnitStates.COOLDOWN:
                EnterCooldown ();
                break;
            default:
                break;
        }
    }

    private void Update () {
        switch (State) {
            case UnitStates.IDLE:
                IdleState ();
                break;
            case UnitStates.PREPARING:
                PrepState ();
                break;
            case UnitStates.ACTING:
                ActingState ();
                break;
            case UnitStates.COOLDOWN:
                CooldownState ();
                break;
        }
    }

    public override void EnterIdle () { SetState (UnitStates.PREPARING); }

    public override void IdleState () { }

    public override void EnterPrep () {
        // List<PathfindingData> data = movement.GetTilesInRange (board);
        // List<Tile> tiles = new List<Tile> ();
        // data.ForEach (element => {
        //     tiles.Add (element.tile);
        // });
        // BoardVisuals.AddTileToHighlights (owner, tiles);
        // tiles = null;
        // PathfindingData selected = data[Random.Range (0, data.Count)];
        // StartCoroutine (movement.Traverse (data, selected, () => {
        //     SetState (UnitStates.COOLDOWN);
        // }));
        // SetState (UnitStates.ACTING);
    }
    public override void PrepState () { }

    public override void EnterActing () { }
    public override void ActingState () { }

    public override void EnterCooldown () {
        BoardVisuals.RemoveTilesFromHighlightsByUnit (owner);
        StartCoroutine (countdown (Random.Range (0, 4), () => SetState (UnitStates.IDLE)));
    }
    public override void CooldownState () { }

    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
        }
        onComplete ();
    }
}