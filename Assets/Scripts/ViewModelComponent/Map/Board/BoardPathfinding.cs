using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardPathfinding : MonoBehaviour {
    private Board board;
    public void Initialize (Board board) {
        this.board = board;
    }

    /// <summary> 
    /// General pathfinding algorithm used by all abilities.
    /// <param name="start">Tile we calculate from</param>
    /// <param name="addTile">Function used to determine whether a new tile should be considered a valid option.</param>
    /// </summary> 

    public List<PathfindingData> Search (Tile start, Func<ShadowTile, Tile, bool> addTile) {
        List<ShadowTile> shadows = new List<ShadowTile> ();
        var startShadow = new ShadowTile (0, start.Position, null, start);
        shadows.Add (startShadow);

        Queue<ShadowTile> checkNext = new Queue<ShadowTile> ();
        Queue<ShadowTile> checkNow = new Queue<ShadowTile> ();

        checkNow.Enqueue (shadows[0]);
        while (checkNow.Count > 0) {
            ShadowTile currentShadow = checkNow.Dequeue ();

            // add the next 'ring' of tiles to search, expanding out from
            // whatever tile we are currently checking
            for (int i = 0; i < 4; ++i) {
                Tile nextTile = GetTile (currentShadow.position + board.Dirs[i]);
                if (nextTile == null) {
                    continue;
                }

                // skip the addTile logic if this tile has been checked already
                ShadowTile oldShadow = shadows.Find (shadow => shadow.tile == nextTile);
                if (oldShadow != null) continue;

                // use strategy pattern to define which adjacent tiles are valid targets
                if (addTile (currentShadow, nextTile)) {
                    var checkedShadow = new ShadowTile (currentShadow.distance + 1, nextTile.Position, currentShadow, nextTile);
                    checkNext.Enqueue (checkedShadow);
                    shadows.Add (checkedShadow);
                }
            }

            // swap the ref between empty and full queue's to avoid re-allocating
            if (checkNow.Count == 0) SwapReference (ref checkNow, ref checkNext);
        }

        // convert our algorithm's results to a pathfindingdata structure so
        // it's usable outside this algorithm
        List<PathfindingData> retValue = new List<PathfindingData> ();
        shadows.ForEach (shadow => {
            retValue.Add (new PathfindingData (shadow.tile, shadow));
        });

        return retValue;
    }

    /// <summary>
    /// Returns the unobstroctued distance between two tiles.
    /// <param name="from">Tile we calculate from</param>
    /// <param name="to">Tile we calculate to</param>
    /// </summary>
    public int GetUnobstructedDistance (Tile from, Tile to) {
        int distanceX = Mathf.Abs (from.Position.x - to.Position.x);
        int distanceY = Mathf.Abs (from.Position.y - to.Position.y);

        return distanceX + distanceY;
    }

    public Tile GetTile (Point p) {
        return board.Tiles.ContainsKey (p) ? board.Tiles[p] : null;
    }

    void SwapReference (ref Queue<ShadowTile> a, ref Queue<ShadowTile> b) {
        Queue<ShadowTile> temp = a;
        a = b;
        b = temp;
    }
}