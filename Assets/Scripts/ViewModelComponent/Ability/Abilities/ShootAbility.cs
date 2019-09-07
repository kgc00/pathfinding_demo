using UnityEngine;

public class ShootAbility : AttackAbility {
    public override void Activate () {
        var board = Owner.Board;
        var ownerPos = Owner.Position;
        var from = board.TileAt (ownerPos);

        Point targetDir = new Point ((Mathf.Clamp (Target.tile.Position.x -
            ownerPos.x, -1, 1)), (Mathf.Clamp (Target.tile.Position.y -
            ownerPos.y, -1, 1)));

        if (Owner.dir != targetDir.ToDirection ()) {
            var toTurn = from.GetDirection (Target.tile);
            Owner.AbilityComponent.TurnUnit (toTurn);
        }

        var instance = Instantiate (Resources.Load<GameObject> ("Prefabs/Projectile"),
            new Vector3 ((targetDir.x + ownerPos.x),
                (targetDir.y + ownerPos.y), -2),
            Quaternion.identity);

        instance.AddComponent<ProjectileComponent> ().Initialize (targetDir, OnAbilityConnected);

        AudioComponent.PlaySound (Sounds.SHOOT);

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