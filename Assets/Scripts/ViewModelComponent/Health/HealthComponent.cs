using UnityEngine;
public class HealthComponent : MonoBehaviour {
    public UnitData data;
    public Unit owner;
    public void Initialize (UnitData data, Unit owner, bool shouldMakeInstance) {
        this.data = shouldMakeInstance ?
            ScriptableObject.CreateInstance<UnitData> ().Assign (data) :
            data;
        this.owner = owner;
    }

    public void AdjustHealth (int amount) {
        Mathf.Clamp (data.CurrentHP += amount, 0, data.MaxHP);
        Debug.Log (string.Format ("unit {0} is at {1} HP", owner, data.CurrentHP));
        if (data.CurrentHP <= 0) {
            owner.UnitDeath ();
        }
    }
}