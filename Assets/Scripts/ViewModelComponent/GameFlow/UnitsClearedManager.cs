using System;
using System.Collections.Generic;
using System.Linq;
public class UnitsClearedManager {
    public static Dictionary<PlayableUnits, bool> unitsCleared = new Dictionary<PlayableUnits, bool> { { PlayableUnits.SHARPSHOOTER, false }, { PlayableUnits.BRAWLER, false } };
    public static PlayableUnits currentUnit { get; private set; } = PlayableUnits.NONE;
    public static bool hasCompletionStatus => unitsCleared.All (entry => entry.Value == true);

    public static void AddUnitCleared (PlayableUnits unitType) {
        if (unitsCleared.ContainsKey (unitType)) {
            unitsCleared[unitType] = true;
        } else {
            unitsCleared.Add (unitType, true);
        }
    }

    public static void AddUnitCleared () {
        if (unitsCleared.ContainsKey (currentUnit)) ClearCurrentUnit ();
        else throw new Exception ("Key does not exist bro, geez");
    }

    private static void ClearCurrentUnit () {
        if (unitsCleared.ContainsKey (currentUnit)) {
            unitsCleared[currentUnit] = true;
        } else {
            unitsCleared.Add (currentUnit, true);
        }
    }

    public static void SetPlayerUnit (PlayableUnits unitType) {
        currentUnit = unitType;
    }

    internal static void ResetClearedUnits () {
        unitsCleared = unitsCleared.ToDictionary (p => p.Key, p => false);
    }
}