using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class RangeComponent {
    protected int range;
    public GameObject Owner { get; protected set; }
    protected Board board;
    protected BoardPathfinding pathfinding;
    public RangeComponent (GameObject Owner, Board board, Ability ability) {
        this.Owner = Owner;
        this.board = board;
        this.pathfinding = board ? board.Pathfinding : null;
        this.range = ability ? ability.Range : 0;
    }
    public virtual RangeComponent SetRange (int range) {
        this.range = range;
        return this;
    }

    public RangeComponent SetStartPosFromMouse (Point mousePosition) {
        Owner.transform.position = new Vector3 (mousePosition.x, mousePosition.y, 0);
        return this;
    }

    public RangeComponent SetOwnerPos (Point ownerPos) {
        Owner.transform.position = new Vector3 (ownerPos.x, ownerPos.y, 0);
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