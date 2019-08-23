using System.Collections.Generic;
using UnityEngine;
public abstract class RangeComponent {
    public int range;
    protected GameObject owner;
    protected Board board;
    protected BoardPathfinding pathfinding;
    public RangeComponent (GameObject owner, Board board, Ability ability) {
        this.owner = owner;
        this.board = board;
        this.pathfinding = board.Pathfinding;
        this.range = ability.Range;
    }
    public abstract List<PathfindingData> GetTilesInRange ();

    // in a more robust implementation we would use a strategy pattern here
    // to mix and match filter with range on a per ability basis
    protected abstract void Filter (List<PathfindingData> tiles);
}