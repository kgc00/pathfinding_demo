using System.Linq;

public class ActiveState : AreaState {
    private Area area;
    public ActiveState (Area area) { this.area = area; }

    public override void Enter () {
        // hero is initialized on the setup state, we initialize every other type here
        var enemies = area.Board.Units.Where (unit => unit.Value.TypeReference != UnitTypes.HERO);

        foreach (var unit in enemies) {
            area.Board.UnitFactory.ActivateEnemyAt (unit.Key);
        }
    }
    public override AreaState HandleUpdate () {
        return null;
    }
}