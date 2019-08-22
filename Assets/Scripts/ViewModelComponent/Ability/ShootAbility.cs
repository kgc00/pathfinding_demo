using UnityEngine;

public class ShootAbility : AttackAbility {
    public override void Activate () {
        var ownerPos = Owner.Position;
        Point dir = new Point ((Mathf.Clamp (Target.tile.Position.x -
            ownerPos.x, -1, 1)), (Mathf.Clamp (Target.tile.Position.y -
            ownerPos.y, -1, 1)));

        var instance = Instantiate (Resources.Load<GameObject> ("Prefabs/Projectile"),
            new Vector3 ((dir.x + ownerPos.x),
                (dir.y + ownerPos.y), -2),
            Quaternion.identity);

        instance.AddComponent<ProjectileComponent> ().Initialize (dir, OnAbilityConnected);

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