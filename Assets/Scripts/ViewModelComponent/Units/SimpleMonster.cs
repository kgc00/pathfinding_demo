using UnityEngine;

public class SimpleMonster : Unit {
    [SerializeField] private SimpleDriver driver;
    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);
        // Movement = gameObject.AddComponent<WalkingMovement> ();
        // Movement.Initialize (board, this, 3);

        driver = gameObject.AddComponent<SimpleAIDriver> ();
        driver.Initialize (board, this, (WalkingMovement) Movement);
    }
}