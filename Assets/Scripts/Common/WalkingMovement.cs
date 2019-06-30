using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMovement : Movement {
    public bool isDebug;
    public override void Initialize (Board board, Unit owner, int range) {
        base.Initialize (board, owner, range);
    }

    public override List<PathfindingData> GetTilesInRange (Board board) {
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

        while (target != null) {
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

            // some dynamic obstacle like a unit is now
            // occupying the tile, we end the traversal early
            if (!to.IsOccupiedBy (owner) && to.OccupiedBy) {
                break;
            }

            Directions dir = from.GetDirection (to);
            if (owner.dir != dir)
                yield return StartCoroutine (Turn (dir));
            yield return StartCoroutine (Walk (from, to));
        }
        onComplete ();
        yield return null;
    }

    IEnumerator Walk (Tile from, Tile target) {
        Tweener tweener = transform.MoveTo (target.center, 0.5f, EasingEquations.Linear);
        bool wasInterrupted = false;
        while (tweener != null) {
            if (!target.IsOccupiedBy (owner) && target.OccupiedBy) {
                wasInterrupted = true;
                break;
            }
            yield return null;
        }

        if (wasInterrupted) {
            Tweener tweenerFrom = transform.MoveTo (from.center, 0.5f, EasingEquations.Linear);
            while (tweenerFrom != null) {
                yield return null;
            }
        }
    }
}