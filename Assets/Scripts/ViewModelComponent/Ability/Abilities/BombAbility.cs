using System;
using System.Linq;
using UnityEngine;

public class BombAbility : AttackAbility {
    public Func<GameObject> onExplosion = () => { return null; };
    public override void Activate () {
        var board = Owner.Board;
        var ownerPos = Owner.Position;
        var from = board.TileAt (ownerPos);

        Point targetDir = new Point ((Mathf.Clamp (Target.tile.Position.x -
            ownerPos.x, -1, 1)), (Mathf.Clamp (Target.tile.Position.y -
            ownerPos.y, -1, 1)));

        var instance = Instantiate (Resources.Load<GameObject> ("Prefabs/Projectile"),
            new Vector3 ((targetDir.x + ownerPos.x),
                (targetDir.y + ownerPos.y), -2),
            Quaternion.identity);

        if (Owner.dir != targetDir.ToDirection ()) {
            var toTurn = from.GetDirection (Target.tile);
            Owner.AbilityComponent.TurnUnit (toTurn);
        }

        instance.AddComponent<ProjectileComponent> ().Initialize (targetDir, OnAbilityConnected);
        instance.AddComponent<DestinationComponent> ().Initialize (Target.tile.Position, OnAbilityConnected);

        OnFinished (EnergyCost);
    }

    public override void OnAbilityConnected (GameObject obj) {
        var rangeComponent = new SelfAndConstantRange (obj, Owner.Board, this);
        rangeComponent.range = this.AreaOfEffect;
        Explode (rangeComponent);
    }

    private void Explode (SelfAndConstantRange rangeComponent) {
        // range component is calculating range based on owner pos, rather than projectile pos
        var tiles = rangeComponent.GetTilesInRange ();
        tiles.Where (data => data.tile.IsOccupied ())
            .Select (data => data.tile.OccupiedBy).ToList ()
            .ForEach (unit => {
                unit.HealthComponent.AdjustHealth (-Damage);
            });

        tiles.ForEach (tile => {
            var vfx = Instantiate (Resources.Load<GameObject> ("Prefabs/Player Impact Visual"), new Vector3 (tile.tile.Position.x, tile.tile.Position.y, Layers.Foreground), Quaternion.identity);
            Destroy (vfx, 0.2f);
        });
    }

    public override void Assign (AbilityData data, Unit owner) {
        this.DisplayName = data.DisplayName;
        this.Range = data.Range;
        this.EnergyCost = data.EnergyCost;
        this.RangeComponentType = data.RangeComponentType;
        this.Damage = data.Damage;
        this.AreaOfEffect = data.AreaOfEffect;
        this.Description = data.Description;
        this.AoERangeComponentType = data.AoERangeComponentType;
        this.Owner = owner;
        this.TargetType = data.TargetType;
    }
}