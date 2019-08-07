using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPrepState : UnitState {
    AbilityComponent abilityComponent;
    List<PathfindingData> tilesInRange;
    PathfindingData target;
    float randomDelay;
    float timeLeft;

    public AIPrepState (Unit Owner) : base (Owner) {
        this.abilityComponent = Owner.AbilityComponent;
        this.randomDelay = Random.Range (1, 3);
    }

    public override void Enter () {
        DetermineAbilityToUse ();
        HighlightTiles (tilesInRange);
        timeLeft = randomDelay;
        timeLeft = randomDelay;
    }

    // loop through options in a simple manner and select one
    private void DetermineAbilityToUse () {
        // destructure to shorten lines
        var equiped = abilityComponent.EquippedAbilities;
        var board = Owner.Board;

        // determine where the player is
        var targetUnit = board.Units.FirstOrDefault (unit => unit.Value is Hero).Value;

        // in case where player is dead, end early with some dummy values
        // otherwise we run into null exceptions when searching for player position
        if (targetUnit == null) {
            HandleMovementLogic (equiped, null);
            return;
        }

        Tile targetTile = board.TileAt (targetUnit.Position);

        // first check attack abilities, if we are in range use the first one
        // ...since this is just a demo we don't assign priority/weight
        var firstChoice = equiped.Where (abil => abil is AttackAbility).ToList ();

        if (!HandleAttackLogic (targetTile, firstChoice))
            HandleMovementLogic (equiped, targetTile);
    }

    private void HandleMovementLogic (List<Ability> equiped, Tile targetTile) {
        // select a movement ability to get in range
        var movAbil = equiped.Find (ability => ability is MovementAbility);
        abilityComponent.SetCurrentAbility (movAbil);
        tilesInRange = abilityComponent.GetTilesInRange ();

        if (!FindMovementTarget (targetTile))
            target = tilesInRange[Random.Range (0, tilesInRange.Count)];

        // does not work... Linq loses linked list data?!
        // target = tilesInRange.Find (data => data.tile = targetTile);

        // works
        // FindMovementTarget(targetTile)

        BoardVisuals.AddIndicator (Owner, new List<Tile> { target.tile });
    }

    private bool FindMovementTarget (Tile targetTile) {
        var targetFound = false;
        foreach (var item in tilesInRange) {
            if (item.tile == targetTile) {
                targetFound = true;
                target = item;
            }
        }
        return targetFound;
    }

    private bool HandleAttackLogic (Tile targetTile, List<Ability> firstChoice) {
        bool selectedAbil = false;
        // select an attack which will reach
        foreach (var ability in firstChoice) {
            // if find and set works we've got a valid move and we break
            if (FindAndSetTarget (targetTile, ability)) {
                selectedAbil = true;
                break;
            }
        }
        return selectedAbil;
    }

    // sets ability component's current ability and returns whether 
    // that skill will reach the target... none of this has astar /
    // obstacle pathfinding support
    private bool FindAndSetTarget (Tile targetTile, Ability ability) {
        abilityComponent.SetCurrentAbility (ability);
        tilesInRange = abilityComponent.GetTilesInRange ();
        var tile = tilesInRange.Find (data => data.tile == targetTile);
        if (tile != null) {
            target = tile;
            BoardVisuals.AddIndicator (Owner, new List<Tile> { target.tile });
            return true;
        }
        return false;
    }

    public override UnitState HandleInput (Controller controller) {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0 && abilityComponent.PrepAbility (tilesInRange, target)) {
            return new AIActingState (Owner, tilesInRange, target);
        }
        return null;
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