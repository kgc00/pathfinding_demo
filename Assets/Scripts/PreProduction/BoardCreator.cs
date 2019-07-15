using System.Collections.Generic;
using System.IO;
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

    public void ClearBoard () {
        List<Point> tilePositions = new List<Point> (tiles.Keys);
        foreach (Point pos in tilePositions) {
            DeleteTileAt (pos);
        }

        List<Point> unitPositions = new List<Point> (units.Keys);
        foreach (Point pos in unitPositions) {
            DeleteUnitAt (pos);
        }
    }

    public void RefreshUnitTypes () {
        unitPrefabs.Clear ();
        UnityEngine.Object[] tmp = Resources.LoadAll ("Prefabs", typeof (Unit));
        for (int i = 0; i < tmp.Length; ++i) {
            unitPrefabs.Add ((Unit) tmp[i]);
        }
    }

    public void RefreshTileTypes () {
        tilePrefabs.Clear ();
        UnityEngine.Object[] tmp = Resources.LoadAll ("Prefabs", typeof (Tile));
        for (int i = 0; i < tmp.Length; ++i) {
            tilePrefabs.Add ((Tile) tmp[i]);
        }
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
        PlaceTile (p, tilePrefabs[currentTileIndex].TypeReference);
    }

    public void PlaceTile (Point p, TileTypes type) {
        if (tiles.ContainsKey (p)) {
            tiles.Remove (p);
            Current.DeleteTileAt (p);
        }

        Tile tile = Current.CreateTileAt (p, type);
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
        PlaceUnit (p, unitPrefabs[currentUnitIndex].TypeReference);
    }

    public void PlaceUnit (Point p, UnitTypes type) {
        if (units.ContainsKey (p)) {
            Current.DeleteUnitAtViaRef (p);
            units.Remove (p);
        }

        Unit unit = Current.LevelEditorCreateUnitAt (p, type);
        unit.transform.parent = gameObject.transform;

        // Put unit in the dictionary
        units.Add (unit.Position, unit);
    }
    public void DeleteUnitAt (Point p) {
        if (units.ContainsKey (p)) {
            Current.DeleteUnitAtViaRef (p);
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
            boardData.tiles.Add (new TileSpawnData (element.Key, element.Value.TypeReference));
        }

        boardData.units = new List<UnitSpawnData> ();
        foreach (KeyValuePair<Point, Unit> element in units)
            boardData.units.Add (new UnitSpawnData (element.Key, element.Value.TypeReference));

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
    public void Load () {
        ClearBoard ();
        if (levelData == null)
            return;

        foreach (TileSpawnData data in levelData.tiles) {
            PlaceTile (data.location, data.tileRef);
        }

        foreach (UnitSpawnData data in levelData.units) {
            PlaceUnit (data.location, data.unitRef);
        }
    }
}