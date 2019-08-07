using System;
using System.Collections.Generic;
using System.Linq;

public class SetupState : AreaState {
    private Area area;
    private bool shouldAdvanceState = false;
    public static Action onAreaLoaded = delegate { };
    public SetupState (Area area) { this.area = area; }

    public override void Enter () {
        // filter our tiles down to entrances only
        List<TileSpawnData> tsd = area.Board.levelData.tiles.Where (tiles => tiles.tileRef == TileTypes.ENTRANCE).ToList ();

        // find min and max values for this board
        Point min = BoardUtility.SetMinV2Int (area.Board.levelData);
        Point max = BoardUtility.SetMaxV2Int (area.Board.levelData);

        InitializeResources (tsd, min, max);
        SetPlayerPosition (tsd, min, max);
        SetPlayerData ();
        onAreaLoaded ();
    }

    private void SetPlayerData () {
        // only allow the player to move at board start state
        Hero playerUnit = (Hero) area.Board.Units.First (unit => unit.Value.TypeReference == UnitTypes.HERO).Value;

        // load their stats/state
        area.Board.InitializeUnitAt (playerUnit.Position);
        playerUnit.LoadUnitState (WorldSaveComponent.GetPlayerStats ());
    }

    private void SetPlayerPosition (List<TileSpawnData> tsd, Point min, Point max) {
        // UnityEngine.Debug.Log (string.Format ("level from {0}", area.areaData.from));
        Tile entrance = SelectCorrectEntrance (tsd, min, max);
        // UnityEngine.Debug.Log (string.Format ("entrance found: {0} at {1}", entrance.name, entrance.Position));
        Unit hero = area.Board.CreateUnitAt (entrance.Position, UnitTypes.HERO);
    }

    private void InitializeResources (List<TileSpawnData> tsd, Point min, Point max) {
        InitializeEntrances (tsd, min, max);
    }

    private void InitializeEntrances (List<TileSpawnData> tsd, Point min, Point max) {
        tsd.ForEach (tile => {
            Entrance t = area.Board.TileAt (tile.location) as Entrance;
            if (t.Position.x == min.x)
                t.SetTransitionDirection (Directions.West);
            else if (t.Position.y == min.y)
                t.SetTransitionDirection (Directions.North);
            else if (t.Position.x == max.x)
                t.SetTransitionDirection (Directions.East);
            else if (t.Position.y == max.y)
                t.SetTransitionDirection (Directions.North);
        });
    }

    private Tile SelectCorrectEntrance (List<TileSpawnData> tsd, Point min, Point max) {
        // if there's only 1 return that
        if (tsd.Count () == 1)
            return area.Board.TileAt (tsd[0].location);

        // else use the direction we entered from to determine the entrance we return
        // if we just spawned into the game we use the center of the board
        switch (area.areaData.from.ToPoint ().ToString ()) {
            case "(1,0)":
                return area.Board.TileAt (tsd.Find (tile => tile.location.x == min.x).location);
            case "(0,1)":
                return area.Board.TileAt (tsd.Find (tile => tile.location.y == min.y).location);
            case "(-1,0)":
                return area.Board.TileAt (tsd.Find (tile => tile.location.x == max.x).location);
            case "(0,-1)":
                return area.Board.TileAt (tsd.Find (tile => tile.location.y == max.y).location);
            default:
                return area.Board.TileAt (new Point ((max.x / 2), (max.y / 2)));
        }
    }

    public void AdvanceAreaState () {
        this.shouldAdvanceState = true;
    }

    public override AreaState HandleUpdate () {
        if (shouldAdvanceState)
            return new ActiveState (area);

        return null;
    }
}