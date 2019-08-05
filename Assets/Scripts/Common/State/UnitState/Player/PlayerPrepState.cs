using System.Collections.Generic;

public class PlayerPrepState : UnitState {
    public static System.Action<Unit, int> onAbilityCommited = delegate { };
    AbilityComponent abilityComponent;
    List<Tile> indicatorList;
    public PlayerPrepState (Unit Owner) : base (Owner) {
        this.abilityComponent = Owner.AbilityComponent;
        indicatorList = new List<Tile> ();
    }

    public override UnitState HandleInput (Controller controller) {
        // handle cancel
        if (controller.DetectInputFor (ControlTypes.CANCEL)) {
            BoardVisuals.RemoveTilesFromHighlightsByUnit (Owner);
            CleanIndicator ();
            return new PlayerIdleState (Owner);
        }

        // generate valid moves this frame
        List<PathfindingData> tilesInRange = abilityComponent.GetTilesInRange ();
        Point mousePosition = BoardUtility.mousePosFromScreenPoint ();
        HighlightTiles (tilesInRange, mousePosition);

        // user clicks on a walkable tile which is in range....
        if (controller.DetectInputFor (ControlTypes.CONFIRM)) {
            PathfindingData selectedTile = tilesInRange.Find (
                element => element.tile.Position == mousePosition
            );

            // transition to acting state if it's a valid selection
            // and we successfully prep our ability for use
            if (selectedTile != null && selectedTile.tile.isWalkable &&
                abilityComponent.PrepAbility (tilesInRange, selectedTile)) {
                onAbilityCommited (Owner, abilityComponent.IndexOfCurrentAbility ());
                return new PlayerActingState (Owner, tilesInRange, selectedTile);
            }
        }
        return null;
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
        if (indicatorList.Contains (selectedTile) || !isValid) return;

        // highlight the players pointer location
        CleanIndicator ();
        indicatorList.Add (selectedTile);
        BoardVisuals.AddIndicator (Owner, indicatorList);
    }

    private void CleanIndicator () {
        BoardVisuals.RemoveIndicator (Owner);
        indicatorList.Clear ();
    }
}