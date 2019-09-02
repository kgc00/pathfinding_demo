public class AreaState {
    public virtual void Enter () { }
    public virtual AreaState HandleUpdate () { return null; }
    public virtual void HandleTransition () { }

}