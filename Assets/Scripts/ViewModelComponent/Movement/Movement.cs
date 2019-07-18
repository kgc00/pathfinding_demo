using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {
    protected Board board;
    protected Unit owner;
    [SerializeField] protected int range;

    public abstract IEnumerator Traverse (List<PathfindingData> path, PathfindingData target, System.Action onComplete);
    public virtual void Initialize (Board board, Unit owner, int range) {
        this.board = board;
        this.owner = owner;
        this.range = range;
    }
    public virtual List<PathfindingData> GetTilesInRange (Board board) {
        List<PathfindingData> retValue = board.Pathfinding.Search (board.TileAt (owner.Position), ExpandSearch);
        Filter (retValue);
        return retValue;
    }
    protected virtual bool ExpandSearch (ShadowTile from, Tile to) {
        return (from.distance + 1) <= range;
    }

    protected virtual void Filter (List<PathfindingData> tiles) {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (!tiles[i].tile.isWalkable)
                tiles.RemoveAt (i);
    }
    protected virtual IEnumerator Turn (Directions dir) {
        TransformLocalEulerTweener t =
            (TransformLocalEulerTweener) transform
            .RotateToLocal (
                dir.ToEuler (), 0.25f, EasingEquations.EaseInOutQuad
            );

        // When rotating between North and West, we must make an exception so it looks like the unit
        // rotates the most efficient way (since 0 and 360 are treated the same)
        if (Mathf.Approximately (t.startValue.y, 0f) &&
            Mathf.Approximately (t.endValue.y, 270f))
            t.startValue = new Vector3 (t.startValue.x, 360f, t.startValue.z);
        else if (Mathf.Approximately (t.startValue.y, 270) &&
            Mathf.Approximately (t.endValue.y, 0))
            t.endValue = new Vector3 (t.startValue.x, 360f, t.startValue.z);
        owner.dir = dir;

        while (t != null)
            yield return null;
    }
}