using System;
using UnityEngine;
public class EnergyComponent : MonoBehaviour {
    public static System.Action<Unit, float> onEnergyChanged = delegate { };
    public UnitData data;
    public Unit owner;
    public bool isAlive => data.CurrentEnergy > 0;
    public float CurrentEnergy => data.CurrentEnergy;

    public void Initialize (UnitData data, Unit owner, bool shouldMakeInstance) {
        this.data = shouldMakeInstance ?
            ScriptableObject.CreateInstance<UnitData> ().Assign (data) :
            data;
        this.owner = owner;
    }

    void Update () {
        AdjustEnergy (data.EnergyRegenRate);
    }

    public bool AdjustEnergy (float amount) {
        var newTotal = data.CurrentEnergy + amount;
        var prevAmount = data.CurrentEnergy;

        if (newTotal > data.MaxEnergy) return false;
        data.CurrentEnergy = Mathf.Clamp (newTotal, 0, data.MaxEnergy);

        if (owner is Hero) onEnergyChanged (owner, prevAmount);

        return true;
    }
}