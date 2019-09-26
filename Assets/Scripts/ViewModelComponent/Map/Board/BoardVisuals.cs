using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// <para>Static helper class to allow change coloring of tiles to display UI information on the board.  Should only ever be
/// one instance of this class at once. </para>
/// 
/// <para>Runs assignment and sorting on update so it should eventually refactor to an event based system that only 
/// updates when needed.  As performance isn't an issue I've skipped this for now. </para>
/// </summary>
public class BoardVisuals : MonoBehaviour {
    private static Dictionary<Unit, List<Renderer>> highlightedTilesByUnit;
    private static List<Renderer> allRangeRenderers;
    private static Dictionary<Unit, List<Renderer>> indicatorRendererByUnit;
    Board board;

    /// <summary>
    /// Method used to inject relevant references into this class instance, a replacement for vanilla constructors in monobehavior objects. 
    /// Should be called only once at object creation.
    /// <param name="board">The board which this class interfaces with.</param>
    /// </summary>
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

    /// <summary>
    /// Adds tiles to the dictionary used to render potential valid targets for an action.  Should be called when a unit has started it's prep phase.
    /// <param name="unit">Unit which acts as a key.</param>
    /// <param name="tiles">List of tiles which acts as a value.</param>
    /// </summary>
    public static void AddIndicator (Unit unit, List<Tile> tiles) {
        if (unit.HealthComponent.isDead) return;

        List<Renderer> temp = tiles.ConvertAll (item => item.GetComponent<Renderer> ()).ToList ();

        if (!indicatorRendererByUnit.ContainsKey (unit)) indicatorRendererByUnit.Add (unit, temp);
        else indicatorRendererByUnit[unit] = temp;
    }

    /// <summary>
    /// Removes tiles from dictionary used to render target indicators.  Should be called after a unit has finished it's cooldown phase.  In the case of players
    /// we also call it every frame during prep phase because their target changes while the mouse over different tiles.
    /// <param name="unit">Unit which acts as a key.</param>
    /// </summary>
    public static void RemoveIndicator (Unit unit) {
        if (!indicatorRendererByUnit.ContainsKey (unit))
            return;

        indicatorRendererByUnit[unit].ForEach (rend => {
            // not an entrance
            if (!rend.GetComponent<Entrance> ()) {
                rend.material.color = Color.white;
                return;
            }

            // resets entrance color to normal
            if (rend.GetComponent<Entrance> ().isEnabled) {
                // should refactor to some const/variable... 
                rend.material.color = new Color (0.04748131f, 0.9150943f, 0.8268626f);
            } else {
                // should refactor to some const/variable... 
                rend.material.color = new Color (0.2969028f, 0.4528302f, 0.437308f);
            }
        });
        indicatorRendererByUnit[unit].Clear ();
    }

    /// <summary>
    /// Adds tiles to the dictionary used to render highlights.  Should be called when a unit has started it's prep phase.
    /// <param name="unit">Unit which acts as a key.</param>
    /// <param name="tiles">List of tiles which acts as a value.</param>
    /// </summary>
    public static void AddTileToHighlights (Unit unit, List<Tile> tiles) {
        if (unit.HealthComponent.isDead) return;
        List<Renderer> temp = new List<Renderer> ();
        if (highlightedTilesByUnit == null)
            return;

        var query = tiles.Where (tile => tile.TypeReference == TileTypes.DIRT);
        foreach (var tile in query) {
            Renderer rend = tile.GetComponent<Renderer> ();
            if (unit is Monster && rend.material.color != Color.cyan) {
                rend.material.color = Color.red;
            } else if (unit is Hero && rend.material.color != Color.blue) {
                rend.material.color = Color.cyan;
            }
            temp.Add (rend);
        }

        if (!highlightedTilesByUnit.ContainsKey (unit))
            highlightedTilesByUnit.Add (unit, temp);
        else
            highlightedTilesByUnit[unit] = temp;
    }

    /// <summary>
    /// Removes tiles from dictionary used to render highlights.  Should be called after a unit has finished it's prep phase.
    /// <param name="unit">Unit which acts as a key.</param>
    /// </summary>
    public static void RemoveTilesFromHighlightsByUnit (Unit unit) {
        if (!highlightedTilesByUnit.ContainsKey (unit)) return;

        // only add a value once
        var duplicates = allRangeRenderers
            .GroupBy (x => x)
            .Where (x => x.Count () > 1)
            .Select (x => x.Key);
        foreach (Renderer rend in highlightedTilesByUnit[unit]) {
            // reset colors of the tile when it leaves this dictionary
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

    /// <summary>
    /// Highlights tiles which are confirmed targets for a unit's actions on the board in a specific color.
    /// Called every frame by Update
    /// </summary>
    private static void RenderIndicatorHighlights () {
        foreach (KeyValuePair<Unit, List<Renderer>> pair in indicatorRendererByUnit) {
            // if a unit dies with highlights up
            if (pair.Key == null) continue;

            // red for monsters, blue for players
            if (pair.Key is Monster) {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.red);
            } else {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.blue);
            }
        }
    }

    /// <summary>
    /// Highlights tiles which are valid targets for a user's actions on the board in a specific color.
    /// Called every frame by Update
    /// </summary>
    private static void RenderRangeHighlights () {
        foreach (KeyValuePair<Unit, List<Renderer>> pair in highlightedTilesByUnit) {
            // if a unit dies with highlights up
            if (pair.Key == null) continue;

            // keep all tiles associated with user's unit(s) cyan
            if (pair.Key is Hero) {
                pair.Value.ForEach (rend =>
                    rend.material.color = Color.cyan);
            }
        }
    }
}