using System;
using System.Collections.Generic;
using System.Linq;

public class SetupState : AreaState {
    private Area area;
    public SetupState (Area area) { this.area = area; }

    public override void Enter () {
        SetPlayerPosition ();
    }

    private void SetPlayerPosition () {
        UnityEngine.Debug.Log (string.Format ("level from {0}", area.areaData.from));
        Tile entrance = SelectCorrectEntrance ();
        UnityEngine.Debug.Log (string.Format ("entrance found: {0} at {1}", entrance.name, entrance.Position));
        Unit hero = area.Board.CreateUnitAt (entrance.Position, UnitTypes.HERO);
    }

    private Tile SelectCorrectEntrance () {
        // filter our tiles down to entrances only
        List<TileSpawnData> tsd = area.Board.levelData.tiles.Where (tiles => tiles.tileRef == TileTypes.ENTRANCE).ToList ();

        // if there's only 1 return that
        if (tsd.Count () == 1)
            return area.Board.TileAt (tsd[0].location);

        // find min and max values for this board
        Point min = BoardUtility.SetMinV2Int (area.Board.levelData);
        Point max = BoardUtility.SetMaxV2Int (area.Board.levelData);

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
    public override AreaState HandleUpdate () {
        var info = area.eventQueue.HandleEvent ();
        if (info != null)
            UnityEngine.Debug.Log (string.Format ("stopped event"));
        return new ActiveState (area);
    }
}