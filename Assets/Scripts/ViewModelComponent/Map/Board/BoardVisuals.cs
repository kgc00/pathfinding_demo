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
        if (highlightedTilesByUnit == null)
            return;

        var query = tiles.Where (tile => tile.TypeReference == TileTypes.DIRT);
        foreach (var tile in query) {
            Renderer rend = tile.GetComponent<Renderer> ();
            if (unit is Monster && rend.material.color != Color.green) {
                rend.material.color = Color.red;
            } else if (unit is Hero) {
                rend.material.color = Color.green;
            }
            temp.Add (rend);
        }
        if (!highlightedTilesByUnit.ContainsKey (unit))
            highlightedTilesByUnit.Add (unit, temp);
        else
            highlightedTilesByUnit[unit] = temp;

    }

    public static void RemoveTilesFromHighlightsByUnit (Unit unit) {
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
        highlightedTilesByUnit[unit].Clear ();
    }

    void Update () {
        // update all renderers list
        allRenderers = highlightedTilesByUnit.SelectMany (x => x.Value).ToList ();

        // keep all tiles associated with user's unit(s) green
        var heroHighlights = highlightedTilesByUnit.FirstOrDefault (item => item.Key.TypeReference == UnitTypes.HERO).Value;
        if (heroHighlights != null) {
            foreach (var rend in allRenderers.Except (heroHighlights)) {
                rend.material.color = Color.red;
            }
        }
    }
}