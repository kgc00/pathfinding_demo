using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPathfinding : MonoBehaviour {
    private Board board;
    public void Initialize (Board board) {
        this.board = board;
    }
    public List<PathfindingData> Search (Tile start, Func<ShadowTile, Tile, bool> addTile) {
        List<ShadowTile> shadows = new List<ShadowTile> ();
        var startShadow = new ShadowTile (int.MaxValue, start.Position, null, start);
        shadows.Add (startShadow);

        Queue<ShadowTile> checkNext = new Queue<ShadowTile> ();
        Queue<ShadowTile> checkNow = new Queue<ShadowTile> ();
        shadows[0].distance = 0;

        checkNow.Enqueue (shadows[0]);
        while (checkNow.Count > 0) {
            ShadowTile currentShadow = checkNow.Dequeue ();
            for (int i = 0; i < 4; ++i) {
                Tile nextTile = GetTile (currentShadow.position + board.Dirs[i]);
                if (nextTile == null) {
                    continue;
                }
                ShadowTile oldShadow = shadows.Find (shadow => shadow.tile == nextTile);
                if (oldShadow != null) {
                    if (oldShadow.distance <= currentShadow.distance + 1) {
                        continue;
                    }
                    continue;
                }

                // use strategy pattern to define unique filtering logic for each request
                if (addTile (currentShadow, nextTile)) {
                    var checkedShadow = new ShadowTile (currentShadow.distance + 1, nextTile.Position, currentShadow, nextTile);
                    checkNext.Enqueue (checkedShadow);
                    shadows.Add (checkedShadow);
                }
            }

            // swap the ref between empty and full queue's to avoid re-allocating
            if (checkNow.Count == 0)
                SwapReference (ref checkNow, ref checkNext);
        }

        List<PathfindingData> retValue = new List<PathfindingData> ();

        // use a pool of pathfinding data
        shadows.ForEach (shadow => {
            retValue.Add (new PathfindingData (shadow.tile, shadow));
        });

        return retValue;
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