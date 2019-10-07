using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Ability : MonoBehaviour {
    // populated from ability data
    public int Range;
    public int AreaOfEffect;
    public float EnergyCost;
    public RangeComponentType RangeComponentType;
    public RangeComponentType AoERangeComponentType;
    public Unit Owner;
    public string DisplayName;
    public string Description;
    public Targets TargetType;
    public bool AutoTargets;

    // fields populated at runtime
    [HideInInspector] public List<PathfindingData> TilesInRange;
    [HideInInspector] public PathfindingData Target;
    [HideInInspector] public System.Action<float> OnFinished;
    [HideInInspector] public MovementComponent Movement;
    public abstract void Activate ();
    public abstract void Assign (AbilityData data, Unit owner);
}