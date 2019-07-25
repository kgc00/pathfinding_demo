using UnityEngine;

[CreateAssetMenu (menuName = "Game/Ability/Data")]
public class AbilityData : ScriptableObject {
    // set in inspector
    public int Range;
    public float CooldownDuration;
    public int Damage;
    public RangeComponentType RangeComponentType;
    public Abilities AbilityType;
}