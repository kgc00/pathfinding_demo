using System;
using System.Collections;
using UnityEngine;

public class Hero : Unit {
    [SerializeField] private WalkingMovement movement;
    [SerializeField] private PlayerController controller;
    [SerializeField] private bool isDebug;
    IEnumerator prepState;

    public void Initialize (Board board, Point pos, UnitTypes r) {
        base.Initialize (board, pos, r);

        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);
        movement.isDebug = isDebug;

        controller = gameObject.AddComponent<PlayerController> ();
        controller.Initialize (board, this, movement);
    }

    protected override void Update () {
        base.Update ();
    }

    public override void SetState (UnitStates state) {
        switch (state) {
            case UnitStates.IDLE:
                this.State = UnitStates.IDLE;
                controller.IdleState ();
                break;
            case UnitStates.PREPARING:
                this.State = UnitStates.PREPARING;
                controller.PrepState ();
                break;
            case UnitStates.ACTING:
                this.State = UnitStates.ACTING;
                controller.ActingState ();
                break;
            case UnitStates.COOLDOWN:
                this.State = UnitStates.COOLDOWN;
                controller.CooldownState ();
                break;
            default:
                break;
        }
    }
}