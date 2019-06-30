using System.Collections.Generic;
using UnityEngine;

public class ActingState : UnitState {
    public Unit owner;
    public WalkingMovement movement;
    private List<PathfindingData> tilesInRange;
    private PathfindingData tileToMoveTo;
    private UnitState state;
    public ActingState (Unit owner, WalkingMovement movement, List<PathfindingData> tilesInRange, PathfindingData tileToMoveTo) {
        this.owner = owner;
        this.movement = movement;
        this.tilesInRange = tilesInRange;
        this.tileToMoveTo = tileToMoveTo;
    }

    public override void Enter () {
        // start a timer with a callback to transition to the next state
        CoroutineHelper.Instance.CoroutineFromEnumerator (
            movement.Traverse (tilesInRange, tileToMoveTo, () => {
                this.UpdateState ();
                tilesInRange.ForEach (pfd => {
                    Board.stPool.ReturnItem (pfd.shadow);
                    Board.pfdPool.ReturnItem (pfd);
                });
            }));
    }

    public void UpdateState () { state = new CooldownState (owner, movement); }

    // return null until UpdateState is called;
    public override UnitState HandleInput () { return state; }
}