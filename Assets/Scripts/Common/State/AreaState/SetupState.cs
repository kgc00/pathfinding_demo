public class SetupState : AreaState {
    private Area area;
    public SetupState (Area area) { this.area = area; }

    public override void Enter () { }
    public override AreaState HandleUpdate () {
        return null;
    }
}