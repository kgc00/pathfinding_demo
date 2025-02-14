using System;
using UnityEngine;
public class HealthComponent : MonoBehaviour {
    public static System.Action<Unit, int> onHealthChanged = delegate { };
    public UnitData data;
    public Unit owner;
    public bool isDead => data.CurrentHP <= 0;
    public int CurrentHP => data.CurrentHP;
    public void Initialize (UnitData data, Unit owner, bool shouldMakeInstance) {
        this.data = shouldMakeInstance ?
            ScriptableObject.CreateInstance<UnitData> ().Assign (data) :
            data;
        this.owner = owner;
    }

    public void AdjustHealth (int amount) {
        var prevAmount = data.CurrentHP;
        data.CurrentHP = Mathf.Clamp (data.CurrentHP + amount, 0, data.MaxHP);

        if (data.CurrentHP < prevAmount) AudioUtil.PlayRandomHurtSound ();

        if (data.CurrentHP <= 0) owner.UnitDeath ();

        if (owner is Hero) onHealthChanged (owner, prevAmount);
    }

    internal void Refill () {
        var prevAmount = data.CurrentHP;
        data.CurrentHP = data.MaxHP;

        if (owner is Hero) onHealthChanged (owner, prevAmount);
    }
}