using System.Collections.Generic;
using UnityEngine;
public class ConstantRange : RangeComponent {
    public ConstantRange (GameObject Owner, Board board, Ability ability) : base (Owner, board, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (Owner.transform.position.ToPoint ()), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    // in a more robust implementation we would use a strategy pattern here
    // to mix and match filter with range on a per ability basis
    protected override void Filter (List<PathfindingData> tiles) {
        if (Owner.GetComponent<Unit> ()) {
            for (int i = tiles.Count - 1; i >= 0; --i)
                if (tiles[i].Tile.OccupiedBy == Owner.GetComponent<Unit> ())
                    tiles.RemoveAt (i);
        }

    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return (from.Distance + 1 <= range) && to.isWalkable;
    }
}