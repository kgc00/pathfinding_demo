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
        CoroutineHelper.Instance.CoroutineFromEnumerator (
            movement.Traverse (tilesInRange, tileToMoveTo, () => this.UpdateState ()));
    }

    public void UpdateState () { state = new CooldownState (owner, movement); }

    public override UnitState HandleInput () { return state; }
}