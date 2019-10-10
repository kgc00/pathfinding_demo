using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaStateHandler : MonoBehaviour {
    static Dictionary<Point, AreaData> worldMap;
    public void Initialize (Dictionary<Point, AreaData> world) {
        worldMap = world;
    }

    public void RemoveUnit (Unit u, Point curLoc, Area area) {
        UnitSpawnData sd = new UnitSpawnData (new Point (-99, -99), UnitTypes.SLIME);
        foreach (KeyValuePair<Point, Unit> pair in area.Board.Units) {
            if (pair.Value == u) {
                foreach (UnitSpawnData data in worldMap[curLoc].areaStateData.currentInstance.units) {
                    if (data.location == pair.Key) {
                        sd = data;
                    }
                }
            }
        }

        if (sd.location.x != -99) {
            Debug.Log (string.Format ("deleting item"));
            worldMap[curLoc].areaStateData.currentInstance.units.Remove (sd);
            area.Board.DeleteUnitAt (sd.location);
        }
    }

    public void KillEnemy (Area area) {
        var units = area.Board.Units.Where (entry => entry.Value.TypeReference != UnitTypes.HERO).ToList ();
        if (units.Count > 0) {
            if (!units[0].Value.HealthComponent) return;
            units[0].Value.HealthComponent.AdjustHealth (-99);
        }
    }

    public void SetEnterDirection (out Directions from, Point curLoc, Point newLoc) {
        var t = newLoc - curLoc;
        switch (t.ToString ()) {
            case "(1,0)":
                from = Directions.West;
                break;
            case "(0,1)":
                from = Directions.South;
                break;
            case "(-1,0)":
                from = Directions.East;
                break;
            case "(0,-1)":
                from = Directions.North;
                break;
            default:
                from = Directions.None;
                break;
        }
    }
    public static bool AreaIsCleared (Board board) {
        if (board.Area.AreaData.areaType != AreaTypes.MOB_ROOM &&
            board.Area.AreaData.areaType != AreaTypes.BOSS_ROOM) {
            return true;
        }

        var nonHeroUnitCount = board.Units.Where (unit => unit.Value.TypeReference != UnitTypes.HERO).ToList ().Count;
        if (nonHeroUnitCount > 0) return false;
        else return true;
    }
}