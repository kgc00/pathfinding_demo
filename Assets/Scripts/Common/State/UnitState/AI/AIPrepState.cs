using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPrepState : UnitState {
    AbilityComponent abilityComponent;

    public AIPrepState (Unit Owner) : base (Owner) {
        this.abilityComponent = Owner.AbilityComponent;
    }

    public override void Enter () { }

    public override UnitState HandleInput (Controller controller) {
        var plan = controller.Brain.Think ();
        if (plan == null) return null;

        BoardVisuals.AddIndicator (Owner, plan.affectedTiles.ConvertAll (data => data.tile));

        if (!abilityComponent.CurrentAbility) return null;
        HighlightTiles (plan.tilesInRange);

        if (!Owner.EnergyComponent.AdjustEnergy (-abilityComponent.CurrentAbility.EnergyCost)) {
            return null;
        }

        if (!abilityComponent.PrepAbility (plan.tilesInRange, plan.targetLocation)) return null;

        return new AIActingState (Owner, plan.tilesInRange, plan.targetLocation);
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