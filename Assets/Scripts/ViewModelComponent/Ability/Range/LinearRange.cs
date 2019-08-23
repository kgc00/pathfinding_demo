using System.Collections.Generic;
using UnityEngine;
public class LinearRange : RangeComponent {
    public LinearRange (GameObject owner, Board board, Ability ability) : base (owner, board, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (owner.transform.position.ToPoint ()), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    protected override void Filter (List<PathfindingData> tiles) {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (!tiles[i].tile.isWalkable || tiles[i].tile.OccupiedBy == owner)
                tiles.RemoveAt (i);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        var ownerPos = owner.transform.position.ToPoint ();

        return (ownerPos.y == to.Position.y ||
                ownerPos.x == to.Position.x) &&
            (from.distance + 1) <= range;
    }
}