using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class WorldSaveComponent : MonoBehaviour {
    void OnApplicationQuit () {
        // works outside of destructor
        FileUtil.DeleteFileOrDirectory ("Assets/Resources/Levels/Boards/Current");
        FileUtil.DeleteFileOrDirectory ("Assets/Resources/Levels/Boards/Current.meta");
        UnityEditor.AssetDatabase.Refresh ();
    }

    internal LevelData createLevelInstance (LevelData initialLevel) {
        string filePath = Application.dataPath + "/Resources/Levels/Boards/Current";
        if (!Directory.Exists (filePath))
            CreateSaveDirectory ();

        LevelData boardData = AssignLevelParameters (initialLevel);
        string name = initialLevel.name + "-current";

        string fileURI = string.Format (
            "Assets/Resources/Levels/Boards/Current/{0}.asset",
            name);
        AssetDatabase.CreateAsset (boardData, fileURI);
        return Resources.Load<LevelData> (string.Format ("Levels/Boards/Current/{0}", name));
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

    void CreateSaveDirectory () {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources", "Levels");
        filePath += "/Boards";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources/Levels", "Boards");
        filePath += "/Current";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources/Levels/Boards", "Current");
        AssetDatabase.Refresh ();
    }
}