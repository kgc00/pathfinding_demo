using System.Collections.Generic;
using UnityEngine;
public class PrepState : UnitState {
    public Unit owner;
    public WalkingMovement movement;
    public PrepState (Unit owner, WalkingMovement movement) {
        this.owner = owner;
        this.movement = movement;
    }
    public override void Enter () { }
    public override UnitState HandleInput () {
        // get all valid tiles this frame
        List<PathfindingData> tilesInRange = movement.GetTilesInRange (owner.Board);

        // convert pathfinding struct to tiles for AddTileToHighlights func...
        List<Tile> tiles = new List<Tile> ();
        tilesInRange.ForEach (element => {
            tiles.Add (element.tile);
        });
        BoardVisuals.AddTileToHighlights (owner, tiles);
        tiles = null;

        // user clicks on a walkable tile which is in range....
        if (Input.GetMouseButtonDown (1)) {
            Point mousePosition = Camera.main.ScreenToWorldPoint (
                new Vector3 (
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.nearClipPlane)).ToPoint ();

            PathfindingData selectedTile = tilesInRange.Find (element => element.tile.Position == mousePosition);

            if (selectedTile != null && selectedTile.tile.isWalkable) {
                // transition to acting state...
                return new ActingState (owner, movement, tilesInRange, selectedTile);
            }
        }

        tilesInRange = null;
        return null;
    }

}