using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkingMovement : MovementComponent {
    public override void Initialize (Board board, Unit owner, UnitData data) {
        base.Initialize (board, owner, data);
    }

    public override IEnumerator Traverse (List<PathfindingData> tilesInRange, PathfindingData target, System.Action onComplete) {
        List<Tile> path = new List<Tile> ();
        PathfindingData startdata = AddStartingData (tilesInRange);

        foreach (var data in tilesInRange) {
            if (data.shadow.distance == 1) {
                data.shadow.AssignPrevious (startdata.shadow);
            }
        }

        while (target != null) {
            path.Insert (0, target.tile);
            target = tilesInRange.Find (data => data.shadow == target.shadow.previous);
        }

        // Move to each waypoint in succession
        bool shouldBreak = false;
        for (int i = 1; i < path.Count; ++i) {
            Tile from = path[i - 1];
            Tile to = path[i];

            if (shouldBreak || !path.Contains (startdata.tile))
                break;

            // some dynamic obstacle like a unit is now
            // occupying the tile, we end the traversal early
            if (to.IsOccupied () || !to.isWalkable) {
                break;
            }

            Directions dir = from.GetDirection (to);
            if (owner.dir != dir)
                yield return StartCoroutine (Turn (dir));

            // pass in a callback to update our state if our path is blocked during traversal
            yield return StartCoroutine (Walk (from, to, () => shouldBreak = true));
        }
        onComplete ();
        yield return null;
    }

    // in general we don't want players to click on the start tile, so we remove it in the ability filter method
    // and add it back into the list here, so the unit still traverses it.
    private PathfindingData AddStartingData (List<PathfindingData> tilesInRange) {
        var startdata = new PathfindingData (owner.Board.TileAt (owner.Position), new ShadowTile (0, owner.Position, null, owner.Board.TileAt (owner.Position)));
        // Debug.Log (string.Format ("tiles in range: {0}, startData: {1}", tilesInRange, startdata));
        tilesInRange.Add (startdata);
        return startdata;
    }

    // using the strategy pattern to customize how we travel
    IEnumerator Walk (Tile from, Tile target, System.Action onInterrupted) {
        float journeyLength = Vector3.Distance (owner.transform.position, target.center);
        float startTime = Time.time;
        float speed = 1 * data.MovementSpeed;
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