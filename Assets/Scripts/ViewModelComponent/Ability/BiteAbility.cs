using UnityEngine;

public class BiteAbility : AttackAbility {
    public override void Activate () {
        var targetUnit = Target.tile.OccupiedBy;
        if (targetUnit != null)
            OnAbilityConnected (targetUnit);

        OnFinished (CooldownDuration);
    }

    public override void OnAbilityConnected (Unit targetedUnit) {
        targetedUnit.HealthComponent.AdjustHealth (-Damage);
    }

    public override void Assign (AbilityData data, Unit owner) {
        this.DisplayName = data.DisplayName;
        this.Range = data.Range;
        this.CooldownDuration = data.CooldownDuration;
        this.RangeComponentType = data.RangeComponentType;
        this.Damage = data.Damage;
        this.Owner = owner;
    }
}