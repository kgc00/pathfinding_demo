using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMovement : Movement {
    public override void Initialize (Board board, Unit owner, int range) {
        base.Initialize (board, owner, range);
    }

    public virtual List<PathfindingData> GetTilesInRange (Board board) {
        List<PathfindingData> retValue = board.Search (board.TileAt (owner.Position), ExpandSearch);
        Filter (retValue);
        return retValue;
    }

    protected override bool ExpandSearch (ShadowTile from, Tile to) {
        if (!to.isWalkable) {
            return false;
        }
        return (from.distance + 1) <= range;
    }
    public override IEnumerator Traverse (List<PathfindingData> path, PathfindingData target, System.Action onComplete) {
        List<Tile> targets = new List<Tile> ();
        int safetyCount = 0;

        while (target.shadow != null) {
            if (safetyCount > 1000) {
                break;
            }
            targets.Insert (0, target.tile);
            target = path.Find (data => data.shadow == target.shadow.previous);
            safetyCount++;
        }

        // Move to each waypoint in succession
        for (int i = 1; i < targets.Count; ++i) {
            Tile from = targets[i - 1];
            Tile to = targets[i];
            Directions dir = from.GetDirection (to);
            if (owner.dir != dir)
                yield return StartCoroutine (Turn (dir));
            yield return StartCoroutine (Walk (to));
        }
        onComplete ();
        yield return null;
    }

    IEnumerator Walk (Tile target) {
        Tweener tweener = transform.MoveTo (target.center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }
}