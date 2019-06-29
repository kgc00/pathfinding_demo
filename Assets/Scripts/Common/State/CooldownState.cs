using UnityEngine;

public class CooldownState : UnitState {
    public Unit owner;
    public WalkingMovement movement;
    private UnitState state;
    public CooldownState (Unit owner, WalkingMovement movement) {
        this.owner = owner;
        this.movement = movement;
    }
    public override void Enter () {
        BoardVisuals.RemoveTileFromHighlights (owner);

        // set a timer to transition to the next state
        CoroutineHelper.Instance.StartCountdown (Random.Range (0, 4),
            () => this.UpdateState ());
    }

    public void UpdateState () { state = new IdleState (owner, movement); }

    public override UnitState HandleInput () { return state; }

}