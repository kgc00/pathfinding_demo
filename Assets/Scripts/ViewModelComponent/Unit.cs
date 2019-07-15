using UnityEngine;

[System.Serializable]
public abstract class Unit : MonoBehaviour {
    public virtual Point Position => new Point ((int) Mathf.Round (transform.position.x), (int) Mathf.Round (transform.position.y));
    public virtual Board Board { get; protected set; }

    [SerializeField] protected Movement movement;
    [SerializeField] public UnitTypes TypeReference;
    public Directions dir;
    private OccupationState occupationState;
    private AbilityComponent abilityComponent;
    public HealthComponent HealthComponent;

    public virtual void Initialize (Board board, UnitTypes r) {
        this.Board = board;
        this.TypeReference = r;
        HealthComponent = gameObject.AddComponent<HealthComponent> ();
        abilityComponent = gameObject.AddComponent<AbilityComponent> ();
        transform.localEulerAngles = dir.ToEuler ();
        occupationState = new OccupationState (this, Board);
        occupationState.Enter ();
    }

    protected virtual void Update () {
        occupationState?.Update ();
    }
    public virtual void LoadUnitState (UnitData data) {
        if (abilityComponent && data) {
            abilityComponent.equippedAbilities = data.equippedAbilities;
        }
        HealthComponent.Initialize (data, this);
    }
    public virtual void UnitDeath () {
        this.GetComponentInChildren<Renderer> ().enabled = false;
        Board.DeleteUnitAtViaPoint (Position);
    }
}