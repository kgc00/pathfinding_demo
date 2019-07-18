using System.Collections.Generic;
public class ConstantRange : RangeComponent {
    public ConstantRange (Unit owner, Ability ability) : base (owner, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        return pathfinding.Search (board.TileAt (owner.Position), ExpandSearch);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return (from.distance + 1) <= range;
    }
}