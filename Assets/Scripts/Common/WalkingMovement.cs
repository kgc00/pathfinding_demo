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

    // using the strategy pattern to re-use how we search
    protected override bool ExpandSearch (ShadowTile from, Tile to) {
        if (!to.isWalkable) {
            return false;
        }
        return (from.distance + 1) <= range;
    }

    public override IEnumerator Traverse (List<PathfindingData> path, PathfindingData target, System.Action onComplete) {
        List<Tile> targets = new List<Tile> ();

        while (target != null) {
            targets.Insert (0, target.tile);
            target = path.Find (data => data.shadow == target.shadow.previous);
        }

        // Move to each waypoint in succession
        bool shouldBreak = false;
        for (int i = 1; i < targets.Count; ++i) {
            Tile from = targets[i - 1];
            Tile to = targets[i];

            if (shouldBreak)
                break;

            // some dynamic obstacle like a unit is now
            // occupying the tile, we end the traversal early
            if (to.IsOccupied ()) {
                break;
            }

            // pass in a callback to update our state if our path is blocked during traversal
            yield return StartCoroutine (Walk (from, to, () => shouldBreak = true));
        }
        onComplete ();
        yield return null;
    }

    // using the strategy pattern to customize how we travel
    IEnumerator Walk (Tile from, Tile target, System.Action onInterrupted) {
        float journeyLength = Vector3.Distance (owner.transform.position, target.center);
        float startTime = Time.time;
        float speed = 1;
        while (owner.transform.position != target.center) {
            bool targetOccupied = target.IsOccupied () && !target.IsOccupiedBy (owner);
            if (targetOccupied) {
                target = from;
                onInterrupted ();
                yield return null;
            }

            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            owner.transform.position = Vector3.Lerp (owner.transform.position, target.center, fracJourney);
            yield return false;
        }
    }
}