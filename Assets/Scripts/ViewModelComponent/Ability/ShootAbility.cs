using UnityEngine;

public class ShootAbility : AttackAbility {
    public override void Activate () {
        var targetUnit = Target.tile.OccupiedBy;
        if (targetUnit != null)
            OnAbilityConnected (Target.tile.OccupiedBy);

        OnFinished (CooldownDuration);
    }

    public override void OnAbilityConnected (Unit targetedUnit) {
        targetedUnit.HealthComponent.AdjustHealth (-Damage);
        Debug.Log (string.Format ("unit {0} was hit for {1} leaving unit with {2} HP", targetedUnit.name, Damage, targetedUnit.HealthComponent.data.CurrentHP));
    }

    public override void Assign (AbilityData data) {
        this.Range = data.Range;
        this.CooldownDuration = data.CooldownDuration;
        this.RangeComponentType = data.RangeComponentType;
        this.Damage = data.Damage;
    }
}