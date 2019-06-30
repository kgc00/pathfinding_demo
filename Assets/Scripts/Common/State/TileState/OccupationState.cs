public class OccupationState {
    Unit Occupier;
    Board Board;
    Tile occupiedTile;
    public OccupationState (Unit Occupier, Board Board) {
        this.Occupier = Occupier;
        this.Board = Board;
    }

    public void Enter () {
        occupiedTile = Board.TileAt (Occupier.Position);
        occupiedTile.SetOccupied (Occupier);
    }

    public void Update () {
        if (occupiedTile == null)
            return;

        Tile tile = Board.TileAt (Occupier.Position);
        if (tile == occupiedTile)
            return;

        EnterNewTile (tile);
    }

    private void EnterNewTile (Tile tile) {
        occupiedTile.SetUnoccupied ();
        occupiedTile = tile;
        occupiedTile.SetOccupied (Occupier);
    }
}