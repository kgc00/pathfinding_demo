using System;
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
        this.pathfinding = board ? board.Pathfinding : null;
        this.range = ability ? ability.Range : 0;
    }
    public virtual RangeComponent SetRange (int range) {
        this.range = range;
        return this;
    }

    public RangeComponent SetStartPosFromMouse (Point mousePosition) {
        owner.transform.position = new Vector3 (mousePosition.x, mousePosition.y, 0);
        return this;
    }

    public RangeComponent SetOwnerPos (Point ownerPos) {
        owner.transform.position = new Vector3 (ownerPos.x, ownerPos.y, 0);
        return this;
    }

    public RangeComponent SetBoard (Board board) {
        this.board = board;
        this.pathfinding = board.Pathfinding;
        return this;
    }

    public abstract List<PathfindingData> GetTilesInRange ();

    // in a more robust implementation we would use a strategy pattern here
    // to mix and match filter with range on a per ability basis
    protected abstract void Filter (List<PathfindingData> tiles);
}