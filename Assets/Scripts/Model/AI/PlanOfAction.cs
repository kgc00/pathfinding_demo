using System.Collections.Generic;

public class PlanOfAction {
    public Ability ability;
    public PathfindingData targetLocation;
    public Targets targetType;
    public List<PathfindingData> tilesInRange;
    public PlanOfAction (Ability ability, PathfindingData targetLocation, Targets targetType, List<PathfindingData> tilesInRange) {
        this.ability = ability;
        this.targetLocation = targetLocation;
        this.targetType = targetType;
        this.tilesInRange = tilesInRange;
    }
    public override string ToString () {
        return string.Format ("{0}, {1}, {2}, {3}", ability, UnityEngine.JsonUtility.ToJson (targetLocation), targetType, tilesInRange.Count);
    }
}