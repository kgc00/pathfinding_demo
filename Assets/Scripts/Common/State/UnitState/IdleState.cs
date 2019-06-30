using UnityEngine;

public class IdleState : UnitState {
    public Unit owner;
    public WalkingMovement movement;

    public IdleState (Unit owner, WalkingMovement movement) {
        this.owner = owner;
        this.movement = movement;
    }

    public override void Enter () { }
    public override UnitState HandleInput () {
        // if user clicks left mouse down...
        if (Input.GetMouseButtonDown (0)) {
            Point mousePosition = Camera.main.ScreenToWorldPoint (
                new Vector3 (
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.nearClipPlane)).ToPoint ();

            // on this unit transition to prep state...
            if (owner.Position == mousePosition) {
                return new PrepState (owner, movement);
            }
        }
        return null;
    }
}