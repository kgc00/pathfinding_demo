using System.Linq;

public class ActiveState : AreaState {
    private Area area;
    private UnitTrackerComponent tracker;
    public ActiveState (Area area, UnitTrackerComponent tracker) {
        this.area = area;
        this.tracker = tracker;
    }

    public override void HandleTransition () {
        UnityEngine.Debug.Log (string.Format ("called"));
        this.tracker.StopTrackingMonstersLeft ();
    }

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