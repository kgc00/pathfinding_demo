using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// should refactor some of the if/color based expressions to a more stable form

// static helper class to allow change coloring of itles to display information

public class BoardVisuals : MonoBehaviour {
    private static Dictionary<Unit, List<Renderer>> highlightedTilesByUnit;
    private static List<Renderer> allRangeRenderers;
    private static Dictionary<Unit, List<Renderer>> indicatorRendererByUnit;
    Board board;
    public void Initialize (Board board) {
        this.board = board;
        allRangeRenderers = new List<Renderer> ();
        highlightedTilesByUnit = new Dictionary<Unit, List<Renderer>> ();
        indicatorRendererByUnit = new Dictionary<Unit, List<Renderer>> ();
        Unit.onUnitDeath += RemoveIndicator;
        Unit.onUnitDeath += RemoveTilesFromHighlightsByUnit;
    }

    ~BoardVisuals () {
        Unit.onUnitDeath -= RemoveIndicator;
        Unit.onUnitDeath -= RemoveTilesFromHighlightsByUnit;
    }

    public static void AddIndicator (Unit unit, List<Tile> tiles) {
        if (!unit.HealthComponent.isAlive) return;

        List<Renderer> temp = tiles.ConvertAll (item => item.GetComponent<Renderer> ()).ToList ();

        if (!indicatorRendererByUnit.ContainsKey (unit)) indicatorRendererByUnit.Add (unit, temp);
        else indicatorRendererByUnit[unit] = temp;
    }

    public static void RemoveIndicator (Unit unit) {
        if (!indicatorRendererByUnit.ContainsKey (unit))
            return;

        indicatorRendererByUnit[unit].ForEach (rend => {
            // resets entrance color to normal
            if (rend.GetComponent<Entrance> ())
                // should refactor to some const/variable... 
                rend.material.color = new Color (0.04748131f, 0.9150943f, 0.8268626f);
            else
                rend.material.color = Color.white;
        });
        indicatorRendererByUnit[unit].Clear ();
    }

    public static void AddTileToHighlights (Unit unit, List<Tile> tiles) {
        if (!unit.HealthComponent.isAlive) return;
        List<Renderer> temp = new List<Renderer> ();
        if (highlightedTilesByUnit == null)
            return;

        var query = tiles.Where (tile => tile.TypeReference == TileTypes.DIRT);
        foreach (var tile in query) {
            Renderer rend = tile.GetComponent<Renderer> ();
            if (unit is Monster && rend.material.color != Color.green) {
                rend.material.color = Color.red;
            } else if (unit is Hero && rend.material.color != Color.blue) {
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
        if (!highlightedTilesByUnit.ContainsKey (unit)) return;

        var duplicates = allRangeRenderers
            .GroupBy (x => x)
            .Where (x => x.Count () > 1)
            .Select (x => x.Key);
        foreach (Renderer rend in highlightedTilesByUnit[unit]) {
            if (!duplicates.Contains (rend)) {
                rend.material.color = Color.white;
            }
            allRangeRenderers.Remove (rend);
        }
        highlightedTilesByUnit[unit].Clear ();
    }

    void Update () {
        // update all renderers list
        allRangeRenderers = highlightedTilesByUnit.SelectMany (x => x.Value).ToList ();
        RenderRangeHighlights ();
        RenderIndicatorHighlights ();
    }

    private static void RenderIndicatorHighlights () {
        foreach (KeyValuePair<Unit, List<Renderer>> pair in indicatorRendererByUnit) {
            if (pair.Key is Monster) {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.yellow);
            } else {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.blue);
            }
        }
    }

    private static void RenderRangeHighlights () {
        // keep all tiles associated with user's unit(s) green
        foreach (KeyValuePair<Unit, List<Renderer>> pair in highlightedTilesByUnit) {
            if (pair.Key is Monster) {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.red);
            } else {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.green);
            }
        }
    }
}