public class Hero : Unit {
    public override void Initialize (Board board, UnitTypes r, Point spawnLocation) {
        base.Initialize (board, r, spawnLocation);

        controller = gameObject.AddComponent<InputHandler> ();
        controller.Initialize (this);

        UnitState = new PlayerIdleState (this);
        UnitState.Enter ();
    }

    public override void LoadUnitState (UnitData data) {
        base.LoadUnitState (data);

        HealthComponent.Initialize (data, this, false);
        UnitState = new PlayerIdleState (this);
        UnitState.Enter ();
    }
}