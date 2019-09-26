public class OccupationState {
    Unit Occupier;
    Board Board;
    Tile occupiedTile;
    public bool isLevelEditor = false;
    public OccupationState (Unit Occupier, Board Board) {
        this.Occupier = Occupier;
        this.Board = Board;
        this.occupiedTile = null;
    }

    public virtual void Enter () {
        if (isLevelEditor || Occupier.OccupationException)
            return;

        occupiedTile = Board.TileAt (Occupier.Position);
        occupiedTile.SetOccupied (Occupier);
    }

    public virtual void Update () {
        if (isLevelEditor)
            return;

        if (occupiedTile == null)
            return;

        Tile tile = Board.TileAt (Occupier.Position);
        if (tile == occupiedTile)
            return;

        EnterNewTile (tile);
    }

    protected virtual void EnterNewTile (Tile newTile) {
        // if for some reason we are leaving an occupied tile,
        // make sure to set the occupied reference to the correct unit
        Unit u = Board.UnitAt (occupiedTile.Position);
        if (u) occupiedTile.SetOccupied (u);
        else occupiedTile.SetUnoccupied ();

        //update the occupied tile var
        occupiedTile = newTile;
        occupiedTile.SetOccupied (Occupier);
    }

    public virtual void ExitTileUponDeath () {
        occupiedTile?.SetUnoccupied ();
    }
}