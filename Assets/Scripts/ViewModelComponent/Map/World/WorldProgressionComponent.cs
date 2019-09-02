using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WorldProgressionComponent {
    public int AreasCleared { get; private set; }
    int threshold;
    World world;
    static Dictionary<Point, AreaData> worldMap;
    public WorldProgressionComponent (World world) {
        this.world = world;
        worldMap = world.world;
    }
    public void AreaCleared () {
        Debug.Log (string.Format ("cleared"));
        AreasCleared++;
        if (AreasCleared >= threshold) {

        }
    }

    public static bool CheckDoorUnlockRequirements (BossRoomEntrance door, Area area) {
        var location = worldMap.Select (worldMapArea => worldMapArea.Value).Where (mapArea => mapArea.areaStateData.initialLevel == area.areaData.initialLevel).Select (thing => thing.Location);
        Debug.Log (string.Format ("location {0}", location.ToString ()));
        // if (area.areaData.initialLevel)
        return false;
    }
    internal void Initialize (Dictionary<Point, AreaData> world) {
        threshold = world.Select (area => area.Value).Where (data => data.areaStateData.areaType == AreaTypes.MOB_ROOM).ToList ().Count;
    }
}