using UnityEngine;
public class HealthComponent : MonoBehaviour {
    public UnitData data;
    public Unit owner;
    public void Initialize (UnitData data, Unit owner) {
        this.data = data;
        this.owner = owner;
    }

    public void AdjustHealth (int amount) {
        Mathf.Clamp (data.currentHP += amount, 0, data.maxHP);
        if (data.currentHP == 0) {
            owner.UnitDeath ();
        }
    }
}