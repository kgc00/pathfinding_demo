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
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            Debug.Log ("mouse pos: " + Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));

            // on this unit AND it is in idle state... 
            if (Physics.Raycast (ray, out hit, 50, 1 << 11)) {
                Unit selectedUnit = hit.transform.GetComponent<Unit> ();
                if (selectedUnit.State == UnitStates.IDLE && selectedUnit == owner) {

                    // transition to prep state...
                    Debug.Log ("selected unit");
                    return new PrepState (owner, movement);
                }
            }
        }
        return null;
    }

}