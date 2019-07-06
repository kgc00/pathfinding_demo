public class NullOccupationState : OccupationState {
    Unit Occupier;
    Board Board;
    Tile occupiedTile;
    public NullOccupationState (Unit Occupier, Board Board) : base (Occupier, Board) { }

    public override void Enter () { }

    public override void Update () { }

    protected override void EnterNewTile (Tile newTile) { }
}