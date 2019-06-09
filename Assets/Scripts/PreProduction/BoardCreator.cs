using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoardCreator : MonoBehaviour {
    [SerializeField] GameObject tileSelectionIndicatorPrefab;
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
}