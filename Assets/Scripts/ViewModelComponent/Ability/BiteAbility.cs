using UnityEngine;

public class BiteAbility : AttackAbility {
    public override void Activate () {
        var targetUnit = Target.tile.OccupiedBy;
        if (targetUnit != null)
            OnAbilityConnected (targetUnit);

        OnFinished (EnergyCost);
    }

    public override void OnAbilityConnected (Unit targetedUnit) {
        targetedUnit.HealthComponent.AdjustHealth (-Damage);
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