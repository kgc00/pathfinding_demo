using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementComponent : MonoBehaviour {
    protected Board board;
    protected Unit owner;

    public abstract IEnumerator Traverse (List<PathfindingData> path, PathfindingData target, System.Action onComplete);
    public virtual void Initialize (Board board, Unit owner) {
        this.board = board;
        this.owner = owner;
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
        if (Mathf.Approximately (t.startValue.z, 0f) &&
            Mathf.Approximately (t.endValue.z, 270f))
            t.startValue = new Vector3 (t.startValue.x, t.startValue.y, 360f);
        else if (Mathf.Approximately (t.startValue.z, 270) &&
            Mathf.Approximately (t.endValue.z, 0))
            t.endValue = new Vector3 (t.startValue.x, t.startValue.y, 360f);
        owner.dir = dir;

        while (t != null)
            yield return null;
    }
}