using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoblinWarriorBrain : Brain {
    int attackRange = -1;
    int lowestCost = -1;
    public GoblinWarriorBrain (Unit owner) : base (owner) { }
    public override PlanOfAction Think () {
        // determine where the player is
        var player = board.Units.FirstOrDefault (unit => unit.Value is Hero).Value;

        SetLowestAbilityCost ();
        SetAttackRange ();
        if (ShouldWait (player)) return null;

        var tilesOnBoard = RangeUtil.SurveyBoard (owner.Position, owner.Board);
        var targetData = FindTarget (tilesOnBoard, player);
        var tilesFromPlayerPerspective = RangeUtil.SurveyBoard (targetData.tile.Position, owner.Board);

        // 1: if we are in range attack the player
        if (CanAttack (tilesOnBoard, targetData)) return Attack (tilesOnBoard, targetData);

        // 2: move towards player
        return Move (tilesOnBoard, targetData, tilesFromPlayerPerspective);
    }

    private void SetAttackRange () {
        if (attackRange == -1) attackRange = abilityComponent.EquippedAbilities.Find (ability => ability is AttackAbility).Range;
    }

    private PlanOfAction Attack (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        var attackAbility = abilityComponent.EquippedAbilities
            .Find (ability => ability is AttackAbility);
        if (!abilityComponent.SetCurrentAbility (attackAbility)) return null;
        var tilesInRange = abilityComponent.GetTilesInRange ();

        for (int i = 0; i < tilesInRange.Count; i++) {
            if (tilesInRange[i].tile == targetData.tile) {
                return new PlanOfAction (attackAbility, tilesInRange[i], Targets.Enemy, tilesInRange);
            }
        }

        return null;
    }

    private bool CanAttack (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        return targetData.shadow.distance == 1;
    }

    private void SetLowestAbilityCost () {
        if (lowestCost == -1) lowestCost = abilityComponent.LowestEnergySkill;
    }

    private PlanOfAction Move (List<PathfindingData> tilesOnBoard, PathfindingData targetData,
        List<PathfindingData> tilesFromPlayerPerspective) {
        // find my move range
        var movementAbility = abilityComponent.EquippedAbilities
            .Find (ability => ability is MovementAbility);
        if (!abilityComponent.SetCurrentAbility (movementAbility)) return null;
        var tilesInRange = abilityComponent.GetTilesInRange ();

        // Find all tiles a certain distance from the player
        // cannot use Linq or it would lose the linkedlist of prev tile
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