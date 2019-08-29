using UnityEngine;

[System.Serializable]
public abstract class Unit : MonoBehaviour {
    protected Controller controller;
    protected OccupationState occupationState;
    public Directions dir;
    public virtual Point Position => new Point ((int) Mathf.Round (transform.position.x), (int) Mathf.Round (transform.position.y));
    public static event System.Action<Unit> onUnitDeath = delegate { };
    [SerializeField] public UnitTypes TypeReference;
    public virtual Board Board { get; protected set; }
    public UnitState UnitState { get; protected set; }
    public Point SpawnLocation { get; private set; }
    public AbilityComponent AbilityComponent { get; protected set; }
    public HealthComponent HealthComponent;
    public EnergyComponent EnergyComponent;

    public virtual void Initialize (Board board, UnitTypes r, Point spawnLocation) {
        this.Board = board;
        this.TypeReference = r;
        this.SpawnLocation = spawnLocation;
        HealthComponent = gameObject.AddComponent<HealthComponent> ();
        EnergyComponent = gameObject.AddComponent<EnergyComponent> ();
        AbilityComponent = gameObject.AddComponent<AbilityComponent> ();
        transform.localEulerAngles = dir.ToEuler ();
        occupationState = new OccupationState (this, Board);
        occupationState.Enter ();
    }

    protected virtual void Update () {
        occupationState?.Update ();

        // if we returned a new state switch to it
        UnitState state = UnitState?.HandleInput (controller);
        if (state == null)
            return;

        UnitState = state;
        UnitState.Enter ();
    }

    public virtual void LoadUnitState (UnitData data) {
        AbilityComponent.Initialize (data, this);
    }

    public virtual void UnitDeath () {
        this.GetComponentInChildren<Renderer> ().enabled = false;
        occupationState?.ExitTileUponDeath ();
        onUnitDeath (this);
    }
}