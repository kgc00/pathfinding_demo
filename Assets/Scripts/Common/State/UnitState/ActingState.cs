using System.Collections.Generic;

public class ActingState : UnitState {
    List<PathfindingData> tilesInRange;
    PathfindingData targetTile;
    UnitState state;
    AbilityComponent abilityComponent;
    public ActingState (Unit Owner,
        List<PathfindingData> tilesInRange, PathfindingData targetTile) : base (Owner) {
        this.tilesInRange = tilesInRange;
        this.targetTile = targetTile;
        this.abilityComponent = Owner.AbilityComponent;
    }

    public override void Enter () {
        // call the ability's start method
        abilityComponent.ActivateWithCallback ((cooldownDuration) => this.UpdateState (cooldownDuration));
    }

    // return ActingState until updatestate is called;
    public override UnitState HandleInput (Controller controller) { return state; }

    // transition to cooldown state
    public void UpdateState (float cooldownDuration) { state = new CooldownState (Owner, cooldownDuration); }
}