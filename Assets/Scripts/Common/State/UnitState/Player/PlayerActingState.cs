using System.Collections.Generic;

public class PlayerActingState : UnitState {
    List<PathfindingData> tilesInRange;
    PathfindingData targetTile;
    UnitState state;
    AbilityComponent abilityComponent;
    public PlayerActingState (Unit Owner,
        List<PathfindingData> tilesInRange, PathfindingData targetTile) : base (Owner) {
        this.tilesInRange = tilesInRange;
        this.targetTile = targetTile;
        this.abilityComponent = Owner.AbilityComponent;
    }

    public override void Enter () {
        // call the ability's start method
        abilityComponent.ActivateWithCallback ((cooldownDuration) => this.UpdateState (cooldownDuration));
        if (Owner is Hero) {
            EventQueue.AddEvent (new AreaStateChangeEventArgs (Owner, null, AreaStateTypes.Active));
        }
    }

    // return ActingState until updatestate is called;
    public override UnitState HandleInput (Controller controller) { return state; }

    // transition to cooldown state
    public void UpdateState (float cooldownDuration) { state = new PlayerCooldownState (Owner, cooldownDuration); }
}