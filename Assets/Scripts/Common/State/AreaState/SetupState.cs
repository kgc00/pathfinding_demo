using System;
using System.Collections.Generic;
using System.Linq;

// initial generic state for each room use to spawn and load resources.
// if more design requirements based on room type were necessary
// this should be split into room specific setupstates to handle that logic
public class SetupState : AreaState {
    private Area area;
    private bool shouldAdvanceState = false;
    public static Action onAreaLoaded = delegate { };
    public SetupState (Area area) { this.area = area; }
    UnitTrackerComponent tracker;
    public override void Enter () {
        // filter our tiles down to entrances only
        List<TileSpawnData> entrances = area.Board.levelData.tiles.Where (tiles => tiles.tileRef == TileTypes.ENTRANCE).ToList ();
        // UnityEngine.Debug.Log (string.Format ("length :{0}", entrances.Count));
        // UnityEngine.Debug.Log (string.Format ("location :{0}", entrances[0].location));
        // UnityEngine.Debug.Log (string.Format ("tile ref :{0}", entrances[0].tileRef));
        // find min and max values for this board
        Point min = BoardUtility.SetMinV2Int (area.Board.levelData);
        Point max = BoardUtility.SetMaxV2Int (area.Board.levelData);

        InitializeResources (entrances, min, max);
        SetPlayerPosition (entrances, min, max);
        SetPlayerData ();
        FinishLoading ();
    }

    private void FinishLoading () {
        if (area.AreaData.areaType == AreaTypes.MOB_ROOM || area.AreaData.areaType == AreaTypes.BOSS_ROOM) {
            tracker = new UnitTrackerComponent ();
            tracker.StartTrackingMonstersLeft (area.Board);
            SetupMobRoom.DisableEntrances (area.Board);
        }
        AudioComponent.StopSound (Sounds.RUNNING);
        onAreaLoaded ();
    }

    private void SetPlayerData () {
        // only allow the player to move at board start state
        // load their stats/state
        Hero playerUnit = (Hero) area.Board.Units.FirstOrDefault (unit => unit.Value.TypeReference == UnitTypes.HERO).Value;

        area.Board.UnitFactory.InitializePlayerUnitAt (playerUnit.Position);
    }

    private void SetPlayerPosition (List<TileSpawnData> entrances, Point min, Point max) {
        // UnityEngine.Debug.Log (string.Format ("level from {0}", area.areaData.from));
        Tile entrance = SelectCorrectEntrance (entrances, min, max);
        // UnityEngine.Debug.Log (string.Format ("entrance found: {0} at {1}", entrance.name, entrance.Position));
        Unit hero = area.Board.CreateUnitAt (entrance.Position, UnitTypes.HERO);
    }

    private void InitializeResources (List<TileSpawnData> entrances, Point min, Point max) {
        InitializeEntrances (entrances, min, max);
    }

    private void InitializeEntrances (List<TileSpawnData> entrances, Point min, Point max) {
        entrances.ForEach (tile => {
            Entrance t = area.Board.TileAt (tile.location) as Entrance;

            if (t.Position.x == min.x)
                t.SetTransitionDirection (Directions.East);
            else if (t.Position.y == min.y)
                t.SetTransitionDirection (Directions.South);
            else if (t.Position.x == max.x)
                t.SetTransitionDirection (Directions.West);
            else if (t.Position.y == max.y)
                t.SetTransitionDirection (Directions.North);
            t.SetEnabled ();
        });
        area.UpdateBossDoor ();
    }

    private Tile SelectCorrectEntrance (List<TileSpawnData> entrances, Point min, Point max) {
        // UnityEngine.Debug.Log (string.Format ("selecting entrance for {0}", area.areaData.from.ToPoint ().ToString ()));
        // if there's only 1 return that
        if (entrances.Count () == 1)
            return area.Board.TileAt (entrances[0].location);

        // else use the direction we entered from to determine the entrance we return
        // if we just spawned into the game we use the center of the board
        switch (area.AreaData.from.ToPoint ().ToString ()) {
            case "(1,0)":
                return area.Board.TileAt (entrances.Find (tile => tile.location.x == min.x).location);
            case "(0,1)":
                return area.Board.TileAt (entrances.Find (tile => tile.location.y == max.y).location);
            case "(-1,0)":
                return area.Board.TileAt (entrances.Find (tile => tile.location.x == max.x).location);
            case "(0,-1)":
                return area.Board.TileAt (entrances.Find (tile => tile.location.y == min.y).location);
            default:
                return area.Board.TileAt (new Point ((max.x / 2), (max.y / 2)));
        }
    }

    public void AdvanceAreaState () {
        this.shouldAdvanceState = true;
    }

    public override AreaState HandleUpdate () {
        if (shouldAdvanceState) return new ActiveState (area, tracker);

        return null;
    }

    public override void HandleTransition () {
        if (tracker != null) tracker.StopTrackingMonstersLeft ();
    }
}