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
        List<PathfindingData> tilesInRange = movement.GetTilesInRange (owner.Board);

        // convert pathfinding struct to tiles for AddTileToHighlights func...
        List<Tile> tiles = new List<Tile> ();
        tilesInRange.ForEach (element => {
            tiles.Add (element.tile);
        });
        BoardVisuals.AddTileToHighlights (owner, tiles);

        // user clicks on a walkable tile in range....
        if (Input.GetMouseButtonDown (1)) {
            Debug.Log ("Clicked");
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, 50, 1 << 10)) {
                Tile selectedTile = hit.transform.GetComponent<Tile> ();
                if (!selectedTile.isWalkable)
                    return null;

                PathfindingData tileToMoveTo = tilesInRange.Find (element => element.tile == selectedTile);
                if (tileToMoveTo.tile != null) {

                    // transition to acting state.
                    Debug.Log ("selected tile");
                    return new ActingState (owner, movement, tilesInRange, tileToMoveTo);
                }
            }
        }
        return null;
    }

}