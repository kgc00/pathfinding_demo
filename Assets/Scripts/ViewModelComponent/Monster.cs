using UnityEngine;

public class Monster : Unit {
    [SerializeField] private WalkingMovement movement;
    [SerializeField] private AIController controller;
    [SerializeField] private bool isDebug;

    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);
        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);
        movement.isDebug = isDebug;

        controller = gameObject.AddComponent<AIController> ();
        controller.Initialize (board, this, movement);
    }
}