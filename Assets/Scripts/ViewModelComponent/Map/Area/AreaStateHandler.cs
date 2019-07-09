using System.Collections.Generic;
using UnityEngine;

public class AreaStateHandler : MonoBehaviour {
    Dictionary<Point, AreaData> world;
    public void Initialize (Dictionary<Point, AreaData> world) {
        this.world = world;
    }
    public void RemoveUnit (Unit u, Point curLoc, Area area) {
        foreach (KeyValuePair<Point, Unit> pair in area.Board.Units) {
            if (pair.Value == u) {
                foreach (UnitSpawnData data in world[curLoc].areaStateData.currentInstance.units) {
                    if (data.location == pair.Key) {
                        Debug.Log (string.Format ("Found item"));
                        world[curLoc].areaStateData.currentInstance.units.Remove (data);
                        area.Board.DeleteUnitAt (pair.Key);
                    }
                }
            }
        }
    }
    public void SetEnterDirection (AreaData ad) {
        // set direction on areastatedata
    }
}