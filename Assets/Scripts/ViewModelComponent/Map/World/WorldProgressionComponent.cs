using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WorldProgressionComponent {
    public static int AreasCleared { get; private set; }
    static Dictionary<Point, int> thresholds;
    World world;
    static Dictionary<Point, AreaData> worldMap;
    public WorldProgressionComponent (World world) {
        this.world = world;
        worldMap = world.world;
    }

    private void SetThresholds (Dictionary<Point, AreaData> world) {
        // var dynamicallySet = world.Select (area => area.Value).Where (data => data.areaStateData.areaType == AreaTypes.MOB_ROOM).ToList ().Count;
        var staticSet = 3;
        thresholds = new Dictionary<Point, int> () { { new Point (0, 1), staticSet } };
    }

    public void AreaCleared (Area area) {
        AreasCleared++;
        area.UpdateBossDoor ();
        SetupMobRoom.EnableEntrances (area.Board);
        if (area.AreaData.areaType == AreaTypes.BOSS_ROOM) {
            EventQueue.AddEvent (new GameClearedEvent (this, () => ClearedBossRoom ()));
        }
    }

    private static void ClearedBossRoom () {
        UnitsClearedManager.AddUnitCleared ();
        if (UnitsClearedManager.hasCompletionStatus) {
            UnitsClearedManager.ResetClearedUnits ();
            SceneUtility.LoadScene ("Win Screen");
        } else SceneUtility.LoadScene ("Character Select");
    }

    public static bool CheckDoorUnlockRequirements (BossRoomEntrance door, Area area) {
        var instance = worldMap.Values.FirstOrDefault (val => val.areaStateData.initialLevel == area.AreaData.initialLevel);
        var entry = thresholds.Keys.FirstOrDefault (key => key == instance.Location);

        if (AreasCleared >= thresholds[entry]) {
            return true;
        } else {
            return false;
        }
    }
    internal void Initialize (Dictionary<Point, AreaData> world) {
        SetThresholds (world);
    }
}