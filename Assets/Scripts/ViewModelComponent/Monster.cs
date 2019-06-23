using System;
using System.Collections;
using UnityEngine;

public class Monster : Unit {
    [SerializeField] private WalkingMovement movement;
    [SerializeField] private AIController controller;

    IEnumerator prepState;

    public void Initialize (Board board, Point pos, UnitTypes r) {
        base.Initialize (board, pos, r);

        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);

        controller = gameObject.AddComponent<AIController> ();
        controller.Initialize (board, this, movement);
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            movement.GetTilesInRange (Board);
        }
    }

    public override void SetState (UnitStates state) {
        switch (state) {
            case UnitStates.IDLE:
                this.State = UnitStates.IDLE;
                controller.IdleState ();
                break;
            case UnitStates.PREPARING:
                this.State = UnitStates.PREPARING;
                prepState = controller.PrepState ();
                prepState.MoveNext ();
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