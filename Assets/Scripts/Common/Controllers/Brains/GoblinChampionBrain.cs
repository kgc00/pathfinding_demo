using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoblinChampionBrain : Brain {
    int shockwaveRange = -1;
    int lowestCost = -1;
    public GoblinChampionBrain (Unit owner) : base (owner) { }
    public override PlanOfAction Think () {
        // determine where the player is
        var player = board.Units.FirstOrDefault (unit => unit.Value is Hero).Value;

        SetLowestAbilityCost ();
        SetAttackRange ();
        if (ShouldWait (player)) return null;

        var tilesOnBoard = RangeUtil.SurveyBoard (owner.Position, owner.Board);
        var targetData = FindTarget (tilesOnBoard, player);
        var tilesFromPlayerPerspective = RangeUtil.SurveyBoard (targetData.tile.Position, owner.Board);

        if (WithinShockwaveRange (tilesOnBoard, targetData))
            return CloseRangePlan (tilesOnBoard, targetData, tilesFromPlayerPerspective);
        else
            return LongRangePlan (tilesOnBoard, targetData, tilesFromPlayerPerspective);
    }

    private void SetAttackRange () {
        if (shockwaveRange == -1) shockwaveRange = abilityComponent.EquippedAbilities.Find (ability => ability.DisplayName == "Shockwave").Range;
    }

    private PlanOfAction CloseRangePlan (List<PathfindingData> tilesOnBoard, PathfindingData targetData, List<PathfindingData> tilesFromPlayerPerspective) {
        // 1: 3/10 times move toward player, 7/10 use shockwave
        var seed = UnityEngine.Random.Range (0, 11);
        if (seed < 3)
            return MoveTowardsPlayer (tilesOnBoard, targetData, tilesFromPlayerPerspective);
        else
            return UseShockwave (tilesOnBoard, targetData);

    }

    private PlanOfAction MoveTowardsPlayer (List<PathfindingData> tilesOnBoard, PathfindingData targetData,
        List<PathfindingData> tilesFromPlayerPerspective) {
        // find my move range
        var movementAbility = abilityComponent.EquippedAbilities
            .Find (ability => ability is MovementAbility);
        if (!abilityComponent.SetCurrentAbility (movementAbility)) return null;
        var tilesInRange = abilityComponent.GetTilesInRange ();

        // Find all tiles a certain distance from the player
        var orderedPossibilities = tilesFromPlayerPerspective.OrderByDescending (data => data.shadow.distance).ToList ();

        // find the first available move target
        PathfindingData moveTarget = FindFirstAvailable (tilesInRange, targetData, orderedPossibilities);

        if (moveTarget == null) {
            Debug.LogError ("never found a valid target");
            return null;
        }

        return new PlanOfAction (movementAbility, moveTarget, Targets.Tile, tilesInRange);
    }

    private PathfindingData FindFirstAvailable (List<PathfindingData> tilesInRange,
        PathfindingData targetData, List<PathfindingData> ordered) {
        PathfindingData moveTarget = null;

        foreach (var item in ordered) {
            for (int i = 0; i < tilesInRange.Count; i++) {
                if (tilesInRange[i].tile == item.tile) {
                    moveTarget = tilesInRange[i];
                    break;
                }
            }
        }

        return moveTarget;
    }

    private PlanOfAction UseShockwave (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        var shockwave = abilityComponent.EquippedAbilities
            .Find (ability => ability.DisplayName == "Shockwave");

        if (!abilityComponent.SetCurrentAbility (shockwave)) return null;

        var tilesInRange = abilityComponent.GetTilesInRange ();
        return new PlanOfAction (shockwave, tilesInRange[0], Targets.Enemy, tilesInRange);
    }

    private bool WithinShockwaveRange (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        return targetData.shadow.distance <= shockwaveRange;
    }

    private void SetLowestAbilityCost () {
        if (lowestCost == -1) lowestCost = abilityComponent.LowestEnergySkill;
    }

    private PlanOfAction LongRangePlan (List<PathfindingData> tilesOnBoard, PathfindingData targetData,
        List<PathfindingData> tilesFromPlayerPerspective) {
        // 2: 1/10 move towards player, 9/10 times use earth spike
        var seed = UnityEngine.Random.Range (0, 11);
        if (seed < 1)
            return MoveTowardsPlayer (tilesOnBoard, targetData, tilesFromPlayerPerspective);
        else
            return UseEarthSpike (tilesOnBoard, targetData);

    }

    private PlanOfAction UseEarthSpike (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        var earthSpike = abilityComponent.EquippedAbilities
            .Find (ability => ability.DisplayName == "Earth Spike");

        if (!abilityComponent.SetCurrentAbility (earthSpike)) return null;

        var tilesInRange = abilityComponent.GetTilesInRange ();

        PathfindingData target = null;
        foreach (var data in tilesInRange) {
            if (data.tile == targetData.tile) {
                target = data;
                break;
            }
        }

        return new PlanOfAction (earthSpike, target, Targets.Enemy, tilesInRange);
    }

    bool ShouldWait (Unit player) {
        // in case where player is dead, end early 
        // otherwise we run into null exceptions when searching for player position
        bool playerIsNull = player == null || player.HealthComponent.isDead;
        var energyCost = lowestCost != -1 ? lowestCost : 2;
        bool notEnoughEnergy = owner.EnergyComponent.CurrentEnergy < energyCost; // some placeholder
        return playerIsNull || notEnoughEnergy;
    }

    PathfindingData FindTarget (List<PathfindingData> tilesOnBoard, Unit player) {
        var targetTile = board.TileAt (player.Position);
        foreach (var item in tilesOnBoard) {
            if (item.tile == targetTile) {
                return item;
            }
        }
        return null;
    }
}