using UnityEngine;

public class EarthSpikeAbility : AttackAbility {
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
            new Vector3 (Target.tile.Position.x,
                Target.tile.Position.y, Layers.Foreground),
            Quaternion.identity);

        instance.AddComponent<DestinationComponent> ().Initialize (Target.tile.Position, OnAbilityConnected);

        OnFinished (EnergyCost);
    }

    public override void OnAbilityConnected (GameObject projectile) {
        try {
            var unit = Owner.Board.TileAt (projectile.transform.position.ToPoint ()).OccupiedBy;
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