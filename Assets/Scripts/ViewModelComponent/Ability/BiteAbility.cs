using UnityEngine;

public class BiteAbility : AttackAbility {
    public override void Activate (AbilityData data) {
        Debug.Log (string.Format ("hit"));
        var targetUnit = Target.tile.OccupiedBy;
        if (targetUnit != null)
            OnAbilityConnected (Target.tile.OccupiedBy);

        OnFinished (CooldownDuration);
    }

    public override void OnAbilityConnected (Unit targetedUnit) {
        targetedUnit.HealthComponent.AdjustHealth (-1);
        Debug.Log (string.Format ("unit {0} hit for 1 damange with {1}", targetedUnit.name, targetedUnit.HealthComponent.data.CurrentHP));
    }

    public override void Assign (AbilityData data) {
        this.Range = data.Range;
        this.CooldownDuration = data.CooldownDuration;
        this.RangeComponentType = data.RangeComponentType;
    }
}