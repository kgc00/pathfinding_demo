using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WorldProgressionComponent {
    public static int AreasCleared { get; private set; }
    static Dictionary<Point, int> thresholds = new Dictionary<Point, int> ();
    World world;
    static Dictionary<Point, AreaData> worldMap;
    public WorldProgressionComponent (World world) {
        this.world = world;
        worldMap = world.world;
    }

    private void SetThresholds (Dictionary<Point, AreaData> world) {
        // var dynamicallySet = world.Select (area => area.Value).Where (data => data.areaStateData.areaType == AreaTypes.MOB_ROOM).ToList ().Count;
        var staticSet = 3;
        thresholds.Add (new Point (0, 1), staticSet);
    }

    public void AreaCleared (Area area) {
        Debug.Log (string.Format ("cleared"));
        AreasCleared++;
        area.UpdateBossDoor ();
        SetupMobRoom.EnableEntrances (area.Board);
    }

    public static bool CheckDoorUnlockRequirements (BossRoomEntrance door, Area area) {
        var instance = worldMap.Values.FirstOrDefault (val => val.areaStateData.initialLevel == area.areaData.initialLevel);
        var entry = thresholds.Keys.FirstOrDefault (key => key == instance.Location);

        if (AreasCleared >= thresholds[entry]) {
            Debug.Log (string.Format ("above"));
            return true;
        } else {
            Debug.Log (string.Format ("below"));
            return false;
        }
    }
    internal void Initialize (Dictionary<Point, AreaData> world) {
        SetThresholds (world);
    }
}