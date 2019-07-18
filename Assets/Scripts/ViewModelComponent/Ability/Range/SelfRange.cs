using System.Collections.Generic;
public class SelfRange : RangeComponent {
    public SelfRange (Unit owner, Ability ability) : base (owner, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        return pathfinding.Search (board.TileAt (owner.Position), ExpandSearch);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return false;
    }
}