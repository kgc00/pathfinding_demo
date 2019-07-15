using System.Collections;
using UnityEngine;

public class Hero : Unit {
    private UnitState HeroState;

    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);

        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);

        HeroState = new IdleState (this, (WalkingMovement) movement);
    }

    protected override void Update () {
        base.Update ();

        // if we returned a new state switch to it
        UnitState state = HeroState?.HandleInput ();
        if (state == null)
            return;

        HeroState = state;
        HeroState.Enter ();
    }
}