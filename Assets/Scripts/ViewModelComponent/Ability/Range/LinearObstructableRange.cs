using System.Collections.Generic;
using UnityEngine;
public class LinearObstructableRange : RangeComponent {
    public LinearObstructableRange (GameObject Owner, Board board, Ability ability) : base (Owner, board, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (Owner.transform.position.ToPoint ()), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    protected override void Filter (List<PathfindingData> tiles) {
        if (Owner.GetComponent<Unit> ()) {
            for (int i = tiles.Count - 1; i >= 0; --i) {
                if (tiles[i].tile.OccupiedBy == Owner.GetComponent<Unit> ())
                    tiles.RemoveAt (i);
            }
        }
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        var ownerPos = Owner.transform.position.ToPoint ();

        return (ownerPos.y == to.Position.y || ownerPos.x == to.Position.x) &&
            (from.distance + 1) <= range &&
            to.isWalkable;
    }
}