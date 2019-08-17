using System.Collections.Generic;
using UnityEngine;
public class WorldSaveComponent : MonoBehaviour {

    public bool HasLoaded { get; private set; } = false;
    private static UnitData playerData;

    internal LevelData createLevelInstance (LevelData initialLevel) {
        LevelData boardData = ScriptableObject.CreateInstance<LevelData> ();
        // cannot directly assign boardData's values to initialLevel
        // or it will modify initialLevel (because it is assigned by ref?)
        List<TileSpawnData> tsd = new List<TileSpawnData> ();
        foreach (TileSpawnData d in initialLevel.tiles) {
            tsd.Add (d);
        }
        List<UnitSpawnData> usd = new List<UnitSpawnData> ();
        foreach (UnitSpawnData u in initialLevel.units) {
            usd.Add (u);
        }
        boardData.tiles = tsd;
        boardData.units = usd;
        return boardData;
    }

    internal void InitializePlayerStats () {
        UnitData instance = ScriptableObject.CreateInstance<UnitData> ();
        UnitData refData = Resources.Load<UnitData> (string.Format ("Beastiary/HeroStats"));
        instance.Assign (refData);
        playerData = instance;
    }

    public void SetAbilitiesLoaded () {
        HasLoaded = true;
    }

    public void ResetAbilitiesLoaded () {
        HasLoaded = false;
    }
    public static UnitData GetPlayerStats () {
        return playerData;
    }

}