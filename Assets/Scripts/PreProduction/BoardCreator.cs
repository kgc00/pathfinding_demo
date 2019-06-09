using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class BoardCreator : MonoBehaviour {
    [SerializeField] GameObject tileSelectionIndicatorPrefab;
    [SerializeField] public List<Tile> tilePrefabs = new List<Tile> ();
    int currentTileIndex = -1;
    [SerializeField] public List<Unit> unitPrefabs = new List<Unit> ();
    int currentUnitIndex = -1;
    public Point MarkerPosition { get; private set; }
    Transform marker;
    string fileName = "boardcreator";
    [HideInInspector] public EditorInputHandler InputHandler;
    [SerializeField] int width = 10; // world space x
    [SerializeField] int depth = 10; // world space y

    [SerializeField] LevelData levelData;
    private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile> ();
    private Dictionary<Point, Unit> units = new Dictionary<Point, Unit> ();
    [HideInInspector] public Board Current;

    private void Awake () {
        GameObject instance = Instantiate (
            tileSelectionIndicatorPrefab, transform
        ) as GameObject;
        marker = instance.transform;
        MoveAndUpdateMarker (new Point (0, 0));
        Current = gameObject.AddComponent<Board> ();
        InputHandler = gameObject.AddComponent<EditorInputHandler> ();
        InputHandler.Initialize (this);
    }

    public void SetFileName (string s) {
        if (s != null && s.Length > 0) {
            fileName = s;
        }
    }

    public void MoveAndUpdateMarker (Point direction) {
        MarkerPosition += direction;
        marker.position = new Vector3 (MarkerPosition.x, MarkerPosition.y, -2);
    }

    public void FillBoard () {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < depth; j++) {
                PlaceSelectedTile (new Point (i, j));
            }
        }
    }

    public void PlaceSelectedTile (Point p) {
        Tile tile = tilePrefabs[currentTileIndex];
        PlaceTile (p, tile);
    }

    public void PlaceTile (Point p, Tile t) {
        if (tiles.ContainsKey (p)) {
            tiles.Remove (p);
            Current.DeleteTileAt (p);
        }

        Tile tile = Current.CreateTileAt (p, t);
        tile.transform.parent = gameObject.transform;

        // Put tile in the dictionary
        tiles.Add (tile.Position, tile);
    }
    public void DeleteTileAt (Point p) {
        if (tiles.ContainsKey (p))
            Current.DeleteTileAt (p);
    }
    public void UpdateSelectedTileType (int i) {
        if (tilePrefabs.Count > i && tilePrefabs[i] != null &&
            i >= 0
        ) {
            currentTileIndex = i;
        } else {
            Debug.Log ("failed");
        }
    }
    public void PlaceSelectedUnit (Point p) {
        Unit unit = unitPrefabs[currentUnitIndex];
        PlaceUnit (p, unit);
    }
    public void PlaceUnit (Point p, Unit u) {
        if (units.ContainsKey (p)) {
            Current.DeleteUnitAt (p);
            units.Remove (p);
        }
        Unit unit = Current.CreateUnitAt (p, u);
        unit.transform.parent = gameObject.transform;

        // Put unit in the dictionary
        units.Add (unit.Position, unit);
    }
    public void DeleteUnitAt (Point p) {
        if (units.ContainsKey (p)) {
            Current.DeleteUnitAt (p);
            units.Remove (p);
        }
    }
    public void UpdateSelectedUnitType (int i) {
        if (unitPrefabs.Count > i && unitPrefabs[i] != null &&
            i >= 0
        ) {
            currentUnitIndex = i;
        } else {
            Debug.Log ("failed");
        }
    }
    public void Save () {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists (filePath))
            CreateSaveDirectory ();

        LevelData boardData = ScriptableObject.CreateInstance<LevelData> ();

        boardData.tiles = new List<TileSpawnData> ();
        foreach (
            KeyValuePair<Point, Tile> element in tiles) {
            Debug.Log (element.Value.gameObject.name);
            boardData.tiles.Add (new TileSpawnData (element.Key, element.Value));
            Debug.Log ("value: " + element.Value);
        }

        boardData.units = new List<UnitSpawnData> ();
        foreach (KeyValuePair<Point, Unit> element in units)
            boardData.units.Add (new UnitSpawnData (element.Key, element.Value.u));

        string fileURI = string.Format (
            "Assets/Resources/Levels/{1}.asset",
            filePath, fileName);
        AssetDatabase.CreateAsset (boardData, fileURI);
    }
    void CreateSaveDirectory () {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists (filePath))
            AssetDatabase.CreateFolder ("Assets/Resources", "Levels");
        AssetDatabase.Refresh ();
    }
    // public void Load () {
    //     ClearBoard ();
    //     if (levelData == null)
    //         return;

    //     foreach (TileSpawnData data in levelData.tiles) {
    //         PlaceTile (data.location, data.tileType);
    //     }

    //     foreach (UnitSpawnData data in levelData.units) {
    //         PlaceUnit (data.location, data.unitType);
    //     }
    // }
}