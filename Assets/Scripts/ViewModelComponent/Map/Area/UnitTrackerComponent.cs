using System;
using System.Linq;

public class UnitTrackerComponent {
    int monstersLeft = -1;
    public UnitTrackerComponent () { }
    ~UnitTrackerComponent () {
        Unit.onUnitDeath -= TrackMonsterDeath;
    }
    public void StartTrackingMonstersLeft (Board board) {
        int monstersLeft = board.Units.Select (entry => entry.Value).Where (unit => unit.TypeReference != UnitTypes.HERO).ToList ().Count;
        if (monstersLeft > 0) {
            this.monstersLeft = monstersLeft;
            Unit.onUnitDeath += TrackMonsterDeath;
        }
    }

    private void TrackMonsterDeath (Unit unit) {
        monstersLeft--;
        if (monstersLeft <= 0) {
            EventQueue.AddEvent (new AreaClearedEvent (this, null));
        }
    }
}