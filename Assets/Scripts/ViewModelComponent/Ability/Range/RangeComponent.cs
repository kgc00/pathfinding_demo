using System.Collections.Generic;
public abstract class RangeComponent {
    public int range;
    protected Unit owner;
    protected Board board;
    protected BoardPathfinding pathfinding;
    public RangeComponent (Unit owner, Ability ability) {
        this.owner = owner;
        this.board = owner.Board;
        this.pathfinding = board.Pathfinding;
        this.range = ability.Range;
    }
    public abstract List<PathfindingData> GetTilesInRange ();
}