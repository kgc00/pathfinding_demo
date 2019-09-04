using System.Linq;

public class SetupMobRoom {
    private Board board;

    public SetupMobRoom (Board board) {
        this.board = board;
    }

    public static void DisableEntrances (Board board) {
        int monstersLeft = board.Units.Select (entry => entry.Value).Where (unit => unit.TypeReference != UnitTypes.HERO).ToList ().Count;
        if (monstersLeft <= 0) return;

        var entrances = board.Tiles.Where (tile => tile.Value.TypeReference == TileTypes.ENTRANCE || tile.Value.TypeReference == TileTypes.BOSS_ENTRANCE).ToList ();
        entrances.ForEach (entrance => entrance.Value.GetComponent<Entrance> ().SetDisabled ());
    }

    public static void EnableEntrances (Board board) {
        var entrances = board.Tiles.Where (tile => tile.Value.TypeReference == TileTypes.ENTRANCE || tile.Value.TypeReference == TileTypes.BOSS_ENTRANCE).ToList ();
        entrances.ForEach (entrance => entrance.Value.GetComponent<Entrance> ().SetEnabled ());
    }
}