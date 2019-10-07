using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Game/Save Data/Unit Data")]
public class UnitData : ScriptableObject {
    public List<AbilityData> EquippedAbilities;
    public int CurrentHP;
    public int MaxHP;
    public float CurrentEnergy;
    public float MaxEnergy;
    public float MovementSpeed;
    public float EnergyRegenRate;
    public UnitTypes UnitType;
    public MovementType MovementType;

    public UnitData Assign (UnitData data) {
        this.EquippedAbilities = data.EquippedAbilities;
        this.CurrentHP = data.CurrentHP;
        this.MaxHP = data.MaxHP;
        this.EnergyRegenRate = data.EnergyRegenRate;
        this.CurrentEnergy = data.CurrentEnergy;
        this.MaxEnergy = data.MaxEnergy;
        this.UnitType = data.UnitType;
        this.MovementSpeed = data.MovementSpeed;
        return this;
    }
}