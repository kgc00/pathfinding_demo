using System.Collections.Generic;
public class LinearRange : RangeComponent {
    public LinearRange (Unit owner, Ability ability) : base (owner, ability) { }

    public override List<PathfindingData> GetTilesInRange () {
        return pathfinding.Search (board.TileAt (owner.Position), ExpandSearch);
    }

    bool ExpandSearch (ShadowTile from, Tile to) {
        return from.position.y == to.Position.y ||
            from.position.x == to.Position.x &&
            (from.distance + 1) <= range;
    }
}