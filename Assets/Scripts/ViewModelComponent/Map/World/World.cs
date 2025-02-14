using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour, IEventHandler {
    [HideInInspector] public static World Instance;
    // Load all area data and store it in a list
    [SerializeField] private List<AreaData> data;
    public Dictionary<Point, AreaData> world { get; private set; } = new Dictionary<Point, AreaData> ();
    [SerializeField] private Point curLoc = new Point (0, 0);
    private GameObject curArea;
    private AreaStateHandler areaStateHandler;
    private WorldSaveComponent worldSaveComponent;
    public EventManager eventManager;
    public WorldProgressionComponent progressionComponent { get; private set; }
    private void Awake () {
        if (Instance != this && Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        Initialize ();
    }

    void OnDestroy () {
        Unit.onUnitDeath -= RemoveUnitFromAreaInstance;
    }

    private void LoadCurrentArea (AreaStateData transitionTo) {
        GameObject instance = new GameObject ("Area: " + curLoc.ToString ());
        Area area = instance.AddComponent<Area> ();
        eventManager.UpdateArea (area);
        area.Initialize (transitionTo, curLoc);
        curArea = instance;
    }

    void RemoveUnitFromAreaInstance (Unit unit) {
        var data = world[curLoc].areaStateData.currentInstance;
        var toRemove = data.units.Find (item => item.location == unit.SpawnLocation);
        data.units.Remove (toRemove);
    }

    private void Update () {
#if UNITY_EDITOR
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
            areaStateHandler.KillEnemy (a);
        }
#endif

        eventManager.HandleUpdate ();
    }

    // refactor into a handler component?
    public void HandleIncomingEvent (InfoEventArgs curEvent) {
        switch (curEvent.type.eventType) {
            case EventTypes.TransitionEvent:
                TransitionEventArgs transitionEvent = (TransitionEventArgs) curEvent;
                TransitionToNewArea (curLoc + transitionEvent.transitionDirection);
                break;
            case EventTypes.PlayerLoaded:
                if (!worldSaveComponent.HasLoaded) {
                    curEvent.e.Invoke ();
                    worldSaveComponent.SetAbilitiesLoaded ();
                }
                break;
            case EventTypes.AreaCleared:
                progressionComponent.AreaCleared (curArea.GetComponent<Area> ());
                curEvent.e.Invoke ();
                break;
            case EventTypes.PlayerDied:
                TransitionToNewArea (new Point (0, 0), true);
                curEvent.e.Invoke ();
                break;
            case EventTypes.GameCleared:
                RangeUtil.ClearComponent ();
                curEvent.e.Invoke ();
                Destroy (gameObject);
                break;
            default:
                break;
        }
    }

    private void TransitionToNewArea (Point newLoc, bool placeUnitInCenter = false) {
        if (curArea)
            Destroy (curArea);

        if (world[newLoc].areaStateData.currentInstance == null)
            world[newLoc].areaStateData.currentInstance = worldSaveComponent.createLevelInstance (world[newLoc].areaStateData.initialLevel);

        if (placeUnitInCenter) world[newLoc].areaStateData.from = Directions.None;
        else {
            areaStateHandler.SetEnterDirection (out world[newLoc].areaStateData.from, curLoc, newLoc);
        }
        curLoc = newLoc;
        LoadCurrentArea (world[newLoc].areaStateData);
    }

    private void Initialize () {
        data = Resources.LoadAll ("Levels/Areas", typeof (AreaData)).Cast<AreaData> ().ToList ();

        if (data == null) throw new System.Exception ("Unable to load level data- world.cs 112");

        foreach (var area in data) {
            area.areaStateData.currentInstance = null;
            world.Add (area.Location, area);
        }

        gameObject.AddComponent<CoroutineHelper> ();
        eventManager = new EventManager (this, null);
        worldSaveComponent = gameObject.AddComponent<WorldSaveComponent> ();
        worldSaveComponent.InitializePlayerStats ();
        progressionComponent = new WorldProgressionComponent (this);
        progressionComponent.Initialize (world);
        areaStateHandler = gameObject.AddComponent<AreaStateHandler> ();
        areaStateHandler.Initialize (world);
        Unit.onUnitDeath += RemoveUnitFromAreaInstance;
        TransitionToNewArea (new Point (0, 0));
    }
}