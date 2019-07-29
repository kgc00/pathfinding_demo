public class Monster : Unit {
    public override void Initialize (Board board, UnitTypes r, Point spawnLocation) {
        base.Initialize (board, r, spawnLocation);

        controller = gameObject.AddComponent<AIController> ();
        controller.Initialize (this);
    }

    public override void LoadUnitState (UnitData data) {
        base.LoadUnitState (data);

        HealthComponent.Initialize (data, this, true);
        UnitState = new AIIdleState (this);
        UnitState.Enter ();
    }
}