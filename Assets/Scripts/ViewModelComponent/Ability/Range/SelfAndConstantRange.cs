using System.Collections.Generic;
using UnityEngine;
public class SelfAndConstantRange : RangeComponent {
    public SelfAndConstantRange (GameObject Owner, Board board, Ability ability) : base (Owner, board, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (Owner.transform.position.ToPoint ()), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    // in a more robust implementation we would use a strategy pattern here
    // to mix and match filter with range on a per ability basis
    protected override void Filter (List<PathfindingData> tiles) {
        return;
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return (from.distance + 1) <= range && to.isWalkable;
    }
}