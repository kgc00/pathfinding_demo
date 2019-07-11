using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour {
    [HideInInspector] public World Instance;
    // Load all area data and store it in a list
    [SerializeField] private List<AreaData> data;
    private Dictionary<Point, AreaData> world = new Dictionary<Point, AreaData> ();
    [SerializeField] private Point curLoc = new Point (0, 0);
    private GameObject curArea;
    private AreaStateHandler areaStateHandler;
    private WorldSaveComponent worldSaveComponent;
    private void Awake () {
        if (Instance != this && Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        Initialize ();
    }

    private void LoadCurrentArea (LevelData transitionTo) {
        GameObject instance = new GameObject ("Area: " + curLoc.ToString ());
        Area area = instance.AddComponent<Area> ();
        area.Initialize (transitionTo);
        curArea = instance;
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.A)) {
            Point p = new Point (curLoc.x - 1, curLoc.y);
            if (world.ContainsKey (p)) {

                TransitionToNewArea (p);
            }
        } else if (Input.GetKeyDown (KeyCode.D)) {
            Point p = new Point (curLoc.x + 1, curLoc.y);
            if (world.ContainsKey (p)) {

                TransitionToNewArea (p);
            }
        } else if (Input.GetKeyDown (KeyCode.W)) {
            Point p = new Point (curLoc.x, curLoc.y + 1);
            if (world.ContainsKey (p)) {

                TransitionToNewArea (p);
            }
        } else if (Input.GetKeyDown (KeyCode.S)) {
            Point p = new Point (curLoc.x, curLoc.y - 1);
            if (world.ContainsKey (p)) {

                TransitionToNewArea (p);
            }
        } else if (Input.GetKeyDown (KeyCode.Space)) {
            var a = curArea.GetComponent<Area> ();
            areaStateHandler.RemoveUnit (
                a.Board.Units.First ().Value,
                curLoc,
                a);
        }
    }

    private void TransitionToNewArea (Point newLoc) {
        if (curArea)
            Destroy (curArea);

        if (world[newLoc].areaStateData.currentInstance == null)
            world[newLoc].areaStateData.currentInstance = worldSaveComponent.createLevelInstance (world[newLoc].areaStateData.initialLevel);

        areaStateHandler.SetEnterDirection (out world[newLoc].areaStateData.from, curLoc, newLoc);
        curLoc = newLoc;
        LoadCurrentArea (world[newLoc].areaStateData.currentInstance);
    }

    private void Initialize () {
        if (Directory.Exists ("Assets/Resources/Levels/Areas")) {
            data = Resources.LoadAll ("Levels/Areas", typeof (AreaData)).Cast<AreaData> ().ToList ();
        }

        if (data == null)
            return;

        foreach (var area in data) {
            area.areaStateData.currentInstance = null;
            world.Add (area.Location, area);
        }

        gameObject.AddComponent<CoroutineHelper> ();
        worldSaveComponent = gameObject.AddComponent<WorldSaveComponent> ();
        areaStateHandler = gameObject.AddComponent<AreaStateHandler> ();
        areaStateHandler.Initialize (world);
        TransitionToNewArea (new Point (0, 0));
    }
}