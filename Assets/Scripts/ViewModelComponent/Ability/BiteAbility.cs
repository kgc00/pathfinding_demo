using UnityEngine;

public class BiteAbility : AttackAbility {
    public override void Activate () {
        var targetUnit = Target.tile.OccupiedBy;
        if (targetUnit != null)
            OnAbilityConnected (targetUnit.gameObject);

        OnFinished (EnergyCost);
    }

    public override void OnAbilityConnected (GameObject targetedUnit) {
        try {
            targetedUnit.GetComponent<Unit> ().HealthComponent.AdjustHealth (-Damage);
        } catch (System.Exception) {
            Debug.Log (string.Format ("unable to get unit script from gameobject"));
        }
    }

    public override void Assign (AbilityData data, Unit owner) {
        this.DisplayName = data.DisplayName;
        this.Range = data.Range;
        this.EnergyCost = data.EnergyCost;
        this.RangeComponentType = data.RangeComponentType;
        this.Damage = data.Damage;
        this.Owner = owner;
    }
}