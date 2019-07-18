using System.Collections.Generic;

public class PrepState : UnitState {
    AbilityComponent abilityComponent;
    public PrepState (Unit Owner) : base (Owner) {
        this.abilityComponent = Owner.AbilityComponent;
    }

    public override UnitState HandleInput (Controller controller) {
        // handle cancel
        if (controller.DetectInputFor (ControlTypes.CANCEL)) {
            BoardVisuals.RemoveTilesFromHighlightsByUnit (Owner);
            return new IdleState (Owner);
        }

        // generate valid moves this frame
        List<PathfindingData> tilesInRange = abilityComponent.GetTilesInRange ();

        HighlightTiles (tilesInRange);

        // user clicks on a walkable tile which is in range....
        if (controller.DetectInputFor (ControlTypes.CONFIRM)) {
            Point mousePosition = BoardUtility.mousePosFromScreenPoint ();
            PathfindingData selectedTile = tilesInRange.Find (
                element => element.tile.Position == mousePosition
            );

            // transition to acting state if it's a valid selection
            // and we successfully prep our ability for use
            if (selectedTile != null && selectedTile.tile.isWalkable &&
                abilityComponent.PrepAbility (tilesInRange, selectedTile))
                return new ActingState (Owner, tilesInRange, selectedTile);
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