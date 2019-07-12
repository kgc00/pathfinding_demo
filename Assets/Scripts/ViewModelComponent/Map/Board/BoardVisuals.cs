using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardVisuals : MonoBehaviour {
    private static Dictionary<Unit, List<Renderer>> highlightedTilesByUnit;
    private static List<Renderer> allRenderers;
    private static List<Renderer> debugRends;
    Board board;
    public void Initialize (Board board) {
        this.board = board;
        allRenderers = new List<Renderer> ();
        highlightedTilesByUnit = new Dictionary<Unit, List<Renderer>> ();
    }

    public static void AddTileToHighlights (Unit unit, List<Tile> tiles) {
        List<Renderer> temp = new List<Renderer> ();
        if (highlightedTilesByUnit == null || highlightedTilesByUnit.ContainsKey (unit))
            return;

        var query = tiles.Where (tile => tile.TypeReference == TileTypes.DIRT);
        foreach (var tile in query) {
            Renderer rend = tile.GetComponent<Renderer> ();
            rend.material.color = Color.green;
            temp.Add (rend);
            allRenderers.Add (rend);
        }
        highlightedTilesByUnit.Add (unit, temp);
    }

    public static void RemoveTileFromHighlights (Unit unit) {
        if (highlightedTilesByUnit.ContainsKey (unit)) {
            var duplicates = allRenderers
                .GroupBy (x => x)
                .Where (x => x.Count () > 1)
                .Select (x => x.Key);
            foreach (Renderer rend in highlightedTilesByUnit[unit]) {
                if (!duplicates.Contains (rend)) {
                    rend.material.color = Color.white;
                }
                allRenderers.Remove (rend);
            }
            highlightedTilesByUnit.Remove (unit);
        }
    }

    public static void DebugHighlights (Renderer r) {
        r.material.color = Color.cyan;
    }

    public static void RemoveDebugHighlights (Renderer r) {
        r.material.color = Color.white;
    }
}