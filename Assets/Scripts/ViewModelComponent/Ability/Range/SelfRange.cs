using System.Collections.Generic;
public class SelfRange : RangeComponent {
    public SelfRange (Unit owner, Ability ability) : base (owner, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        var retValue = pathfinding.Search (board.TileAt (owner.Position), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    protected override void Filter (List<PathfindingData> tiles) {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (!tiles[i].tile.isWalkable)
                tiles.RemoveAt (i);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return false;
    }
}