using System.Collections.Generic;

public class AIPrepState : UnitState {
    AbilityComponent abilityComponent;

    public AIPrepState (Unit Owner) : base (Owner) {
        this.abilityComponent = Owner.AbilityComponent;
    }

    public override void Enter () { }

    public override UnitState HandleInput (Controller controller) {
        var plan = controller.Brain.Think ();

        if (NoValidActionsAreAvailable (plan)) return null;

        if (abilityComponent.CurrentAbility is AttackAbility) {
            BoardVisuals.AddIndicator (Owner, plan.affectedTiles.ConvertAll (data => data.tile));
        }

        return new AIActingState (Owner, plan.tilesInRange, plan.targetLocation);
    }

    private bool NoValidActionsAreAvailable (PlanOfAction plan) {
        return plan == null ||
            !abilityComponent.CurrentAbility ||
            !Owner.EnergyComponent.AdjustEnergy (-abilityComponent.CurrentAbility.EnergyCost) ||
            !abilityComponent.PrepAbility (plan.tilesInRange, plan.targetLocation);
    }

    private void HighlightTiles (List<PathfindingData> tilesInRange) {
        // convert pathfinding struct to tiles for AddTileToHighlights func...
        List<Tile> tiles = new List<Tile> ();
        tilesInRange.ForEach (element => {
            tiles.Add (element.tile);
        });
        BoardVisuals.AddTileToHighlights (Owner, tiles);
    }
}