using UnityEngine;

public class Monster : Unit {
    [SerializeField] private AIController controller;
    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);
        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);

        controller = gameObject.AddComponent<AIController> ();
        controller.Initialize (board, this, (WalkingMovement) movement);
    }
}