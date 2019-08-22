using UnityEngine;
public class HealthComponent : MonoBehaviour {
    public static System.Action<Unit, int> onHealthChanged = delegate { };
    public UnitData data;
    public Unit owner;
    public bool isAlive => data.CurrentHP > 0;
    public int CurrentHP => data.CurrentHP;
    public void Initialize (UnitData data, Unit owner, bool shouldMakeInstance) {
        this.data = shouldMakeInstance ?
            ScriptableObject.CreateInstance<UnitData> ().Assign (data) :
            data;
        this.owner = owner;
    }

    public void AdjustHealth (int amount) {
        var prevAmount = data.CurrentHP;
        Mathf.Clamp (data.CurrentHP += amount, 0, data.MaxHP);
        if (data.CurrentHP <= 0) owner.UnitDeath ();

        if (owner is Hero) onHealthChanged (owner, prevAmount);
    }
}