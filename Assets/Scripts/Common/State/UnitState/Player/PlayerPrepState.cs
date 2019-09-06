using System;
using System.Collections.Generic;

public class PlayerPrepState : UnitState {
    public static Action<Unit, int> onAbilitySet = delegate { };
    public static Action<Unit, int> onAbilityCommited = delegate { };
    public static Action onAbilityCanceled = delegate { };
    AbilityComponent abilityComponent;
    List<Tile> indicatorList;
    Tile AoECenter;
    public PlayerPrepState (Unit Owner) : base (Owner) {
        this.abilityComponent = Owner.AbilityComponent;
        indicatorList = new List<Tile> ();
    }

    UnitState SwapActiveAbility (Controller controller) {
        bool[] pressed = new bool[] {
            controller.DetectInputFor (ControlTypes.ABILITY_ONE),
            controller.DetectInputFor (ControlTypes.ABILITY_TWO),
            controller.DetectInputFor (ControlTypes.ABILITY_THREE),
        };

        // loop through them and see if any have been pressed...
        for (int i = 0; i < pressed.Length; i++) {
            if (!pressed[i]) continue;

            // transition to the next state with that data
            if (abilityComponent.SetCurrentAbility (i)) {
                onAbilitySet (Owner, i);
                return new PlayerPrepState (Owner);
            }
        }

        if (controller.DetectInputFor (ControlTypes.CANCEL)) {
            onAbilityCanceled ();
            return new PlayerIdleState (Owner);
        }
        return null;
    }

    public override UnitState HandleInput (Controller controller) {
        var didSwap = SwapActiveAbility (controller);
        if (didSwap != null) {
            BoardVisuals.RemoveTilesFromHighlightsByUnit (Owner);
            CleanIndicator ();
            return didSwap;
        }

        List<PathfindingData> tilesInRange = GetTilesInRange ();
        Point mousePosition = BoardUtility.mousePosFromScreenPoint ();
        HighlightTiles (tilesInRange, mousePosition);

        // user clicks on a walkable tile which is in range....
        if (controller.DetectInputFor (ControlTypes.CONFIRM)) {
            PathfindingData selectedTarget = tilesInRange.Find (
                element => element.tile.Position == mousePosition
            );

            // transition to acting state if it's a valid selection
            // and we successfully prep our ability for use
            bool targetIsValid = selectedTarget != null && selectedTarget.tile.isWalkable;
            if (targetIsValid && Owner.EnergyComponent.AdjustEnergy (-abilityComponent.CurrentAbility.EnergyCost) &&
                abilityComponent.PrepAbility (tilesInRange, selectedTarget)) {
                onAbilityCommited (Owner, abilityComponent.IndexOfCurrentAbility ());
                return new PlayerActingState (Owner, tilesInRange, selectedTarget);
            }
        }
        return null;
    }

    private List<PathfindingData> GetTilesInRange () {
        // return normal range if the area is a cleared mob room
        // or we are not using a movement ability
        bool useMovementAbility = abilityComponent.CurrentAbility is MovementAbility;
        if (!AreaStateHandler.AreaIsCleared (Owner.Board) || !useMovementAbility) {
            return abilityComponent.GetTilesInRange ();
        }

        // return all tiles if we are in a cleared room
        return RangeUtil.SurveyBoard (Owner.Position, Owner.Board);
    }

    private void HighlightTiles (List<PathfindingData> tilesInRange, Point mousePosition) {
        HandleRange (tilesInRange);
        HandleIndicator (tilesInRange, mousePosition);
    }

    private void HandleRange (List<PathfindingData> tilesInRange) {
        // convert pathfinding struct to tiles for AddTileToHighlights func...
        List<Tile> tiles = new List<Tile> ();
        tilesInRange.ForEach (element => {
            tiles.Add (element.tile);
        });
        BoardVisuals.AddTileToHighlights (Owner, tiles);
    }

    private void HandleIndicator (List<PathfindingData> tilesInRange, Point mousePosition) {
        var selectedTile = Owner.Board.TileAt (mousePosition);
        var isValid = tilesInRange.Exists (data => data.tile == selectedTile);
        // tile is not valid or already is highlighted, return 
        if (!isValid) return;

        if (abilityComponent.CurrentAbility.AreaOfEffect > 0) {
            var aoeRange = RangeUtil.GetAoERangeFromMousePosition (mousePosition, abilityComponent.CurrentAbility);
            var curFrameCenter = aoeRange.Find (data => data.shadow.distance == 0).tile;
            if (curFrameCenter == AoECenter) return;
            else {
                CleanIndicator ();
                aoeRange.ForEach (data => {
                    if (data.shadow.distance == 0) AoECenter = data.tile;
                    indicatorList.Add (data.tile);
                });
            }
        } else {
            if (indicatorList.Contains (selectedTile)) return;
            CleanIndicator ();
            indicatorList.Add (selectedTile);
        }
        BoardVisuals.AddIndicator (Owner, indicatorList);
    }

    private void CleanIndicator () {
        BoardVisuals.RemoveIndicator (Owner);
        indicatorList.Clear ();
    }
}