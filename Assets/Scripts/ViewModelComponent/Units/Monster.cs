public class Monster : Unit {
    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);
        Movement = gameObject.AddComponent<WalkingMovement> ();
        Movement.Initialize (board, this, 3);
    }
}