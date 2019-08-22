using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Ability : MonoBehaviour {
    // populated from ability data
    public int Range;
    public float EnergyCost;
    public RangeComponentType RangeComponentType;
    public Unit Owner;
    public string DisplayName;

    // fields populated at runtime
    [HideInInspector] public List<PathfindingData> TilesInRange;
    [HideInInspector] public PathfindingData Target;
    [HideInInspector] public System.Action<float> OnFinished;
    [HideInInspector] public MovementComponent Movement;
    public abstract void Activate ();
    public abstract void Assign (AbilityData data, Unit owner);
}