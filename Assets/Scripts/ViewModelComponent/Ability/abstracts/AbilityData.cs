using UnityEngine;

[CreateAssetMenu (menuName = "Game/Ability/Data")]
public class AbilityData : ScriptableObject {
    // set in inspector
    public string DisplayName;
    public int Range;
    public float EnergyCost;
    public int Damage;
    public int AreaOfEffect; // 0 (none), 1, 2
    public string Description;
    public RangeComponentType RangeComponentType;
    public RangeComponentType AoERangeComponentType;
    public Abilities AbilityType;
    public Targets TargetType;
}