using System.Collections.Generic;
using UnityEngine;
public class ConstantRange : RangeComponent {
    public ConstantRange (GameObject owner, Board board, Ability ability) : base (owner, board, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (owner.transform.position.ToPoint ()), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    // in a more robust implementation we would use a strategy pattern here
    // to mix and match filter with range on a per ability basis
    protected override void Filter (List<PathfindingData> tiles) {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].tile.OccupiedBy == owner)
                tiles.RemoveAt (i);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return (from.distance + 1) <= range && to.isWalkable;
    }
}