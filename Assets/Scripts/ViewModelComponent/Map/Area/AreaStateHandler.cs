using System.Collections.Generic;
using UnityEngine;

public class AreaStateHandler : MonoBehaviour {
    Dictionary<Point, AreaData> world;
    public void Initialize (Dictionary<Point, AreaData> world) {
        this.world = world;
    }
    public void RemoveUnit (Unit u, Point curLoc, Area area) {
        UnitSpawnData sd = new UnitSpawnData (new Point (-99, -99), UnitTypes.MONSTER);
        foreach (KeyValuePair<Point, Unit> pair in area.Board.Units) {
            if (pair.Value == u) {
                foreach (UnitSpawnData data in world[curLoc].areaStateData.currentInstance.units) {
                    if (data.location == pair.Key) {
                        sd = data;
                    }
                }
            }
        }

        if (sd.location.x != -99) {
            Debug.Log (string.Format ("deleting item"));
            world[curLoc].areaStateData.currentInstance.units.Remove (sd);
            area.Board.DeleteUnitAt (sd.location);
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
}