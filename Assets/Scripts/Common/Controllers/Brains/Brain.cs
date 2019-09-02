public abstract class Brain {
    protected AbilityComponent abilityComponent;
    protected Unit owner;
    protected Board board;
    protected Brain (Unit owner) {
        this.owner = owner;
        this.abilityComponent = owner.AbilityComponent;
        this.board = owner.Board;
    }
    public abstract PlanOfAction Think ();
}