public class Monster : Unit {
    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);

        controller = gameObject.AddComponent<AIController> ();
        controller.Initialize (this);
    }

    public override void LoadUnitState (UnitData data) {
        base.LoadUnitState (data);
        UnitState = new AIIdleState (this);
        UnitState.Enter ();
    }
}