using System;
using System.Collections.Generic;
public class ConstantRange : RangeComponent {
    public ConstantRange (Unit owner, Ability ability) : base (owner, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (owner.Position), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    // in a more robust implementation we would use a strategy pattern here
    // to mix and match filter with range on a per ability basis
    protected override void Filter (List<PathfindingData> tiles) {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (!tiles[i].tile.isWalkable)
                tiles.RemoveAt (i);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return (from.distance + 1) <= range;
    }
}