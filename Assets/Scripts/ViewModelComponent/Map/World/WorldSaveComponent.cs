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

        LevelData boardData = ScriptableObject.CreateInstance<LevelData> ();
        boardData.tiles = initialLevel.tiles;
        boardData.units = initialLevel.units;
        string name = initialLevel.name + "-current";

        string fileURI = string.Format (
            "Assets/Resources/Levels/Boards/Current/{0}.asset",
            name);
        AssetDatabase.CreateAsset (boardData, fileURI);
        return Resources.Load<LevelData> (string.Format ("Levels/Boards/Current/{0}", name));
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