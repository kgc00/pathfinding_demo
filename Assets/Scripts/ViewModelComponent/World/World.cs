using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour {
    [HideInInspector] public World Instance;
    // Load all area data and store it in a list
    [SerializeField] private List<AreaData> data;
    private Dictionary<Point, LevelData> world = new Dictionary<Point, LevelData> ();
    [SerializeField] private Point curLoc = new Point (0, 0);
    private GameObject curArea;
    private void Awake () {
        if (Instance != this && Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        Initialize ();
    }

    private void LoadCurrentArea () {
        GameObject instance = new GameObject ("Area: " + curLoc.ToString ());
        Area area = instance.AddComponent<Area> ();
        area.Initialize (world[curLoc]);
        curArea = instance;
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.A)) {
            Point p = new Point (curLoc.x - 1, curLoc.y);
            if (world.ContainsKey (p)) {
                curLoc = p;
                TransitionToNewArea ();
            }
        } else if (Input.GetKeyDown (KeyCode.D)) {
            Point p = new Point (curLoc.x + 1, curLoc.y);
            if (world.ContainsKey (p)) {
                curLoc = p;
                TransitionToNewArea ();
            }
        } else if (Input.GetKeyDown (KeyCode.W)) {
            Point p = new Point (curLoc.x, curLoc.y + 1);
            if (world.ContainsKey (p)) {
                curLoc = p;
                TransitionToNewArea ();
            }
        } else if (Input.GetKeyDown (KeyCode.S)) {
            Point p = new Point (curLoc.x, curLoc.y - 1);
            if (world.ContainsKey (p)) {
                curLoc = p;
                TransitionToNewArea ();
            }
        }
    }

    private void TransitionToNewArea () {
        Destroy (curArea);
        LoadCurrentArea ();
    }

    private void Initialize () {
        if (Directory.Exists ("Assets/Resources/Levels/Areas")) {
            data = Resources.LoadAll ("Levels/Areas", typeof (AreaData)).Cast<AreaData> ().ToList ();
        }

        if (data == null)
            return;

        foreach (var area in data) {
            world.Add (area.Location, area.LevelData);
        }

        gameObject.AddComponent<CoroutineHelper> ();

        LoadCurrentArea ();
    }
}