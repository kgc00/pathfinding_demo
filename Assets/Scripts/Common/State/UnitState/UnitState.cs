public class UnitState {
    public Unit Owner;

    public UnitState (Unit Owner) {
        this.Owner = Owner;
    }
    public virtual void Enter () { }
    public virtual UnitState HandleInput (Controller controller) { return null; }
}