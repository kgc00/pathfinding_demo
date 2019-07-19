public class Hero : Unit {
    public UnitState HeroState { get; protected set; }

    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);

        controller = gameObject.AddComponent<InputHandler> ();
        controller.Initialize (this);

        HeroState = new IdleState (this);
    }

    protected override void Update () {
        base.Update ();

        // if we returned a new state switch to it
        UnitState state = HeroState?.HandleInput (controller);
        if (state == null)
            return;

        HeroState = state;
        HeroState.Enter ();
    }
}