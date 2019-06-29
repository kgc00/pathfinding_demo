using System.Collections;
using UnityEngine;

public class Hero : Unit {
    [SerializeField] private WalkingMovement movement;
    [SerializeField] private PlayerController controller;
    [SerializeField] private bool isDebug;
    IEnumerator prepState;

    private UnitState HeroState;

    public void Initialize (Board board, Point pos, UnitTypes r) {
        base.Initialize (board, pos, r);

        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);
        movement.isDebug = isDebug;

        // controller = gameObject.AddComponent<PlayerController> ();
        // controller.Initialize (board, this, movement);

        HeroState = new IdleState (this, movement);
    }

    protected override void Update () {
        base.Update ();

        // if we returned a new state switch to it
        UnitState state = HeroState.HandleInput ();
        if (state == null)
            return;

        HeroState = state;
        HeroState.Enter ();
    }

    public override void SetState (UnitStates state) {

        this.State = state;

        switch (state) {
            case UnitStates.IDLE:
                controller.EnterIdleState ();
                break;
            case UnitStates.PREPARING:
                controller.EnterPrepState ();
                break;
            case UnitStates.ACTING:
                controller.EnterActingState ();
                break;
            case UnitStates.COOLDOWN:
                controller.EnterCooldownState ();
                break;
            default:
                break;
        }
    }
}