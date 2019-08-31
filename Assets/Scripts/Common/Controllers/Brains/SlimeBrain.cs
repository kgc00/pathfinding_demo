using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SlimeBrain : Brain {
    public SlimeBrain (Unit owner) : base (owner) { }
    public override PlanOfAction Think () {
        // destructure to shorten lines
        var equiped = abilityComponent.EquippedAbilities;

        // determine where the player is
        var player = board.Units.FirstOrDefault (unit => unit.Value is Hero).Value;

        // in case where player is dead, end early with some dummy values
        // otherwise we run into null exceptions when searching for player position
        bool playerIsDead = player == null || !player.HealthComponent.isAlive;
        if (playerIsDead) {
            return CreateMovePlan (equiped, null);
        }

        Tile targetTile = board.TileAt (player.Position);

        // first check attack abilities, if we are in range use the first one
        // ...since this is just a demo we don't assign priority/weight
        var attacks = equiped.Where (abil => abil is AttackAbility).ToList ();

        if (CreateAttackPlan (targetTile, attacks) != null) {
            return CreateAttackPlan (targetTile, attacks);
        } else {
            return CreateMovePlan (equiped, targetTile);
        }
    }

    private PlanOfAction CreateMovePlan (List<Ability> equiped, Tile targetTile) {
        PathfindingData target;
        // select a movement ability to get in range
        var movAbil = equiped.Find (ability => ability is MovementAbility);
        if (!abilityComponent.SetCurrentAbility (movAbil)) {
            return null;
        }
        var tilesInRange = abilityComponent.GetTilesInRange ();

        if (FindMovementTarget (targetTile, tilesInRange) != null)
            target = FindMovementTarget (targetTile, tilesInRange);
        else
            target = tilesInRange[UnityEngine.Random.Range (0, tilesInRange.Count)];

        return new PlanOfAction (movAbil, target, Targets.Tile, tilesInRange);
    }

    private PathfindingData FindMovementTarget (Tile targetTile, List<PathfindingData> tilesInRange) {
        foreach (var item in tilesInRange) {
            if (item.tile == targetTile) {
                return item;
            }
        }
        return null;
    }

    private PlanOfAction CreateAttackPlan (Tile targetTile, List<Ability> firstChoice) {
        // select an attack which will reach
        foreach (var ability in firstChoice) {
            // if we don't have enough energy to act
            if (!abilityComponent.SetCurrentAbility (ability)) return null;

            // need the pathfinding data to see if we can reach the tile
            var tilesInRange = abilityComponent.GetTilesInRange ();

            // if find and set works we've got a valid move and we break
            if (FindAndSetTarget (targetTile, ability, tilesInRange) != null) {
                var target = FindAndSetTarget (targetTile, ability, tilesInRange);
                return new PlanOfAction (ability, target, Targets.Enemy, tilesInRange);
            }
        }
        return null;
    }

    // sets ability component's current ability and returns whether 
    // that skill will reach the target... none of this has astar /
    // obstacle pathfinding support
    private PathfindingData FindAndSetTarget (Tile targetTile, Ability ability, List<PathfindingData> tilesInRange) {
        var tile = tilesInRange.Find (data => data.tile == targetTile);
        if (tile != null) {
            return tile;
        }
        return null;
    }
}