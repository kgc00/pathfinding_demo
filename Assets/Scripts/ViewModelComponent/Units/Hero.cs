public class Hero : Unit {
    public static System.Action<Unit> onUnitCreated = delegate { };
    public override void Initialize (Board board, UnitTypes r, Point spawnLocation) {
        base.Initialize (board, r, spawnLocation);
        this.name = "Hero";

        controller = gameObject.AddComponent<InputHandler> ();
        controller.Initialize (this);

        UnitState = new PlayerIdleState (this);
        UnitState.Enter ();
    }

    public override void LoadUnitState (UnitData data) {
        base.LoadUnitState (data);
        HealthComponent.Initialize (data, this, false);
        EnergyComponent.Initialize (data, this, false);
        EventQueue.AddEvent (new PlayerLoadedEventArgs (this, () => onUnitCreated (this)));
        UnitState = new PlayerIdleState (this);
        UnitState.Enter ();
    }
    public override void UnitDeath () {
        base.UnitDeath ();
        EventQueue.AddEvent (new PlayerDiedEventArgs (this, () => {
            HealthComponent.Refill ();
            EnergyComponent.Refill ();
            AudioComponent.PlaySound (Sounds.PLAYER_DEATH);
        }));
    }
}