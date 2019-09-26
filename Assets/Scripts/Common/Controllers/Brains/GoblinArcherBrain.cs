using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoblinArcherBrain : Brain {
    int minDistance = 3;
    int lowestCost = -1;
    public GoblinArcherBrain (Unit owner) : base (owner) { }
    public override PlanOfAction Think () {
        // determine where the player is
        var player = board.Units.FirstOrDefault (unit => unit.Value is Hero).Value;

        if (ShouldWait (player)) return null;

        SetLowestAbilityCost ();

        var tilesOnBoard = RangeUtil.SurveyBoard (owner.Position, owner.Board);
        var targetData = FindTarget (tilesOnBoard, player);
        var tilesFromPlayerPerspective = RangeUtil.SurveyBoard (targetData.Tile.Position, owner.Board);
        // 1: if too close, move away from player

        if (WeAreTooClose (tilesOnBoard, targetData)) return Move (tilesOnBoard, targetData, tilesFromPlayerPerspective, true);
        // 2: attack player
        if (WeOccupyTheSameAxis (tilesOnBoard, targetData)) {
            if (ThereAreNoObstacles (tilesOnBoard, targetData))
                return Attack (tilesOnBoard, targetData, tilesFromPlayerPerspective);
            else
                return Move (tilesOnBoard, targetData, tilesFromPlayerPerspective, false, false);
        }
        // 3: move towards player
        return Move (tilesOnBoard, targetData, tilesFromPlayerPerspective, false);
    }

    private bool ThereAreNoObstacles (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        return owner.Board.Pathfinding.GetUnobstructedDistance (owner.Board.TileAt (owner.Position), targetData.Tile) == targetData.Shadow.Distance;
    }

    private PlanOfAction Attack (List<PathfindingData> tilesOnBoard, PathfindingData targetData, List<PathfindingData> tilesFromPlayerPerspective) {
        var attackAbility = abilityComponent.EquippedAbilities
            .Find (ability => ability is AttackAbility);
        if (!abilityComponent.SetCurrentAbility (attackAbility)) return null;
        var tilesInRange = abilityComponent.GetTilesInRange ();

        for (int i = 0; i < tilesInRange.Count; i++) {
            if (tilesInRange[i].Tile == targetData.Tile) {
                return new PlanOfAction (attackAbility, tilesInRange[i], Targets.Enemy, tilesInRange);
            }
        }

        return null;
    }

    private bool WeOccupyTheSameAxis (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        return owner.Position.x == targetData.Tile.Position.x || owner.Position.y == targetData.Tile.Position.y;
    }

    private void SetLowestAbilityCost () {
        if (lowestCost == -1) lowestCost = abilityComponent.LowestEnergySkill;
    }

    private PlanOfAction Move (List<PathfindingData> tilesOnBoard, PathfindingData targetData,
        List<PathfindingData> tilesFromPlayerPerspective, bool moveAway, bool checkSameAxis = true) {
        // find my move range
        var movementAbility = abilityComponent.EquippedAbilities
            .Find (ability => ability is MovementAbility);
        if (!abilityComponent.SetCurrentAbility (movementAbility)) return null;
        var tilesInRange = abilityComponent.GetTilesInRange ();

        // Find all tiles a certain distance from the player
        // cannot use Linq or it would lose the linkedlist of prev tile
        var orderedPossibilities = moveAway ?
            tilesFromPlayerPerspective.OrderBy (data => data.Shadow.Distance).ToList () :
            tilesFromPlayerPerspective.OrderByDescending (data => data.Shadow.Distance).ToList ();

        // find the first available move target
        PathfindingData moveTarget = FindFirstAvailable (tilesInRange, targetData, orderedPossibilities, checkSameAxis);

        if (moveTarget == null) {
            Debug.LogError ("never found a valid target");
            return null;
        }

        return new PlanOfAction (movementAbility, moveTarget, Targets.Tile, tilesInRange);
    }

    private PathfindingData FindFirstAvailable (List<PathfindingData> tilesInRange,
        PathfindingData targetData, List<PathfindingData> ordered, bool checkSameAxis = false) {
        PathfindingData moveTarget = null;
        var prioritized = ordered.FindAll (data => data.Tile.Position.x == targetData.Tile.Position.x || data.Tile.Position.y == targetData.Tile.Position.y);
        if (checkSameAxis) moveTarget = CheckXYAxis (tilesInRange, moveTarget, prioritized, targetData.Tile);
        if (moveTarget == null) { moveTarget = CheckRestOfPool (tilesInRange, ordered, moveTarget, targetData.Tile); }

        return moveTarget;
    }

    private PathfindingData CheckRestOfPool (List<PathfindingData> tilesInRange, List<PathfindingData> ordered, PathfindingData moveTarget, Tile playerTile) {
        foreach (var item in ordered) {
            for (int i = 0; i < tilesInRange.Count; i++) {
                if (tilesInRange[i].Tile == item.Tile && item.Shadow.Distance >= minDistance) {
                    moveTarget = tilesInRange[i];
                    break;
                }
            }
        }

        return moveTarget;
    }

    private PathfindingData CheckXYAxis (List<PathfindingData> tilesInRange, PathfindingData moveTarget, List<PathfindingData> prioritized, Tile playerTile) {
        foreach (var item in prioritized) {
            for (int i = 0; i < tilesInRange.Count; i++) {
                if (tilesInRange[i].Tile == item.Tile && item.Shadow.Distance >= minDistance) {
                    if (owner.Board.Pathfinding.GetUnobstructedDistance (playerTile, item.Tile) == item.Shadow.Distance) {
                        moveTarget = tilesInRange[i];
                        break;
                    }
                }
            }
        }

        return moveTarget;
    }

    bool WeAreTooClose (List<PathfindingData> tilesOnBoard, PathfindingData targetData) {
        return targetData.Shadow.Distance < minDistance;
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
            if (item.Tile == targetTile) {
                return item;
            }
        }
        return null;
    }
}