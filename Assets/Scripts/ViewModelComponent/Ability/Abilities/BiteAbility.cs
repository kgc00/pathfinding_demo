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
            var unit = targetedUnit.GetComponent<Unit> ();
            unit.HealthComponent.AdjustHealth (-Damage);
            var vfx = Instantiate (Resources.Load<GameObject> ("Prefabs/Player Impact Visual"), new Vector3 (unit.Position.x, unit.Position.y, Layers.Foreground), Quaternion.identity);
            Destroy (vfx, 0.2f);
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
        this.Description = data.Description;
        this.Owner = owner;
        this.TargetType = data.TargetType;
    }
}