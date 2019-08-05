using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class WorldSaveComponent : MonoBehaviour {
    public bool HasLoaded { get; private set; } = false;
    void OnApplicationQuit () {
        // works outside of destructor
        FileUtil.DeleteFileOrDirectory ("Assets/Resources/Current");
        FileUtil.DeleteFileOrDirectory ("Assets/Resources/Current.meta");
        UnityEditor.AssetDatabase.Refresh ();
    }

    internal LevelData createLevelInstance (LevelData initialLevel) {
        string filePath = Application.dataPath + "/Current/Levels/Boards";
        if (!Directory.Exists (filePath))
            CreateLevelSaveDirectory ();

        LevelData boardData = AssignLevelParameters (initialLevel);
        string name = initialLevel.name + "-current";

        string fileURI = string.Format (
            "Assets/Resources/Current/Levels/Boards/{0}.asset",
            name);
        AssetDatabase.CreateAsset (boardData, fileURI);
        return Resources.Load<LevelData> (string.Format ("Current/Levels/Boards/{0}", name));
    }

    private LevelData AssignLevelParameters (LevelData initialLevel) {
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

    void CreateLevelSaveDirectory () {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets", "Resources");
        filePath += "/Current";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources", "Current");
        filePath += "/Levels";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources/Current", "Levels");
        filePath += "/Boards";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources/Current/Levels", "Boards");
        AssetDatabase.Refresh ();
    }

    void CreateUnitSaveDirectory () {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets", "Resources");
        filePath += "/Current";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources", "Current");
        filePath += "/Units";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources/Current", "Units");
        AssetDatabase.Refresh ();
    }

    internal void InitializePlayerStats () {
        string filePath = Application.dataPath + "/Current/Units";
        if (!Directory.Exists (filePath)) {
            CreateUnitSaveDirectory ();

            UnitData playerData = ScriptableObject.CreateInstance<UnitData> ();
            UnitData refData = Resources.Load<UnitData> (string.Format ("Beastiary/HeroStats"));
            playerData.Assign (refData);
            string name = "HeroStats-current";
            string fileURI = string.Format (
                "Assets/Resources/Current/Units/{0}.asset",
                name);
            AssetDatabase.CreateAsset (playerData, fileURI);
            AssetDatabase.Refresh ();
        }
    }

    public void SetAbilitiesLoaded () {
        HasLoaded = true;
    }

    public void ResetAbilitiesLoaded () {
        HasLoaded = false;
    }
    public static UnitData GetPlayerStats () {
        return Resources.Load<UnitData> ("Current/Units/HeroStats-current");
    }

}