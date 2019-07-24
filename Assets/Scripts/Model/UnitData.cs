using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Game/Save Data/Unit Data")]
public class UnitData : ScriptableObject {
    public List<AbilityData> EquippedAbilities;
    public int CurrentHP;
    public int MaxHP;
    public UnitTypes UnitType;
    public MovementType MovementType;
    public void Assign (List<AbilityData> a, int chp, int mhp, UnitTypes t) {
        this.EquippedAbilities = a;
        this.CurrentHP = chp;
        this.MaxHP = mhp;
        this.UnitType = t;
    }
    public void Assign (UnitData data) {
        this.EquippedAbilities = data.EquippedAbilities;
        this.CurrentHP = data.CurrentHP;
        this.MaxHP = data.MaxHP;
        this.UnitType = data.UnitType;
    }
}