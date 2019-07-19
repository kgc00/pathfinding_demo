using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {
    // set in inspector
    public int Range;
    public float CooldownDuration;
    public RangeComponentType RangeComponentType;

    // fields populated at runtime
    [HideInInspector] public List<PathfindingData> TilesInRange;
    [HideInInspector] public PathfindingData Target;
    [HideInInspector] public System.Action<float> OnFinished;
    [HideInInspector] public MovementComponent Movement;

    public virtual void Activate () { }
}