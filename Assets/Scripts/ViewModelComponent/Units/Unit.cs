using UnityEngine;

[System.Serializable]
public abstract class Unit : MonoBehaviour {
    public virtual Point Position => new Point ((int) Mathf.Round (transform.position.x), (int) Mathf.Round (transform.position.y));
    public virtual Board Board { get; protected set; }
    public static event System.Action<Unit> onUnitDeath = delegate { };

    [SerializeField] public UnitTypes TypeReference;
    public Directions dir;
    protected OccupationState occupationState;
    public AbilityComponent AbilityComponent { get; protected set; }
    public HealthComponent HealthComponent;
    protected Controller controller;
    public UnitState UnitState { get; protected set; }

    public virtual void Initialize (Board board, UnitTypes r) {
        this.Board = board;
        this.TypeReference = r;
        HealthComponent = gameObject.AddComponent<HealthComponent> ();
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
        HealthComponent.Initialize (data, this);
    }

    public virtual void UnitDeath () {
        this.GetComponentInChildren<Renderer> ().enabled = false;
        occupationState?.ExitTileUponDeath ();
        onUnitDeath (this);
    }
}