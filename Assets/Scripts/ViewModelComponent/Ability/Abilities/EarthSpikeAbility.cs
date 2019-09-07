using System.Collections;
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

        var coroutine = StartCoroutine (countdown (1f, () => SpawnSpike ()));
    }

    private void SpawnSpike () {
        if (Owner == null) return;

        var instance = Instantiate (Resources.Load<GameObject> ("Prefabs/Projectile"),
            new Vector3 (Target.tile.Position.x,
                Target.tile.Position.y, Layers.Foreground),
            Quaternion.identity);

        instance.AddComponent<DestinationComponent> ().Initialize (Target.tile.Position, OnAbilityConnected);

        AudioComponent.PlaySound (Sounds.BOMB_LAUNCHED);

        OnFinished (EnergyCost);
    }
    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
        }
        onComplete ();
    }

    public override void OnAbilityConnected (GameObject projectile) {
        try {
            var unit = Owner.Board.TileAt (projectile.transform.position.ToPoint ()).OccupiedBy;
            unit.HealthComponent.AdjustHealth (-Damage);
            var vfx = Instantiate (Resources.Load<GameObject> ("Prefabs/Player Impact Visual"), new Vector3 (unit.Position.x, unit.Position.y, Layers.Foreground), Quaternion.identity);
            AudioComponent.PlaySound (Sounds.BOMB);
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