using System.Collections;
using UnityEngine;
public class BiteAbility : AttackAbility {
    public override void Activate () {

        StartCoroutine (countdown (1f, () => FinishExecution ()));

    }
    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
        }
        onComplete ();
    }
    private void FinishExecution () {
        if (Owner == null) return;

        var targetUnit = Target.tile.OccupiedBy;
        var from = Owner.Board.TileAt (Owner.Position);

        var toTurn = from.GetDirection (Target.tile);
        Owner.AbilityComponent.TurnUnit (toTurn);

        if (targetUnit != null) OnAbilityConnected (targetUnit.gameObject);

        var vfx = Instantiate (Resources.Load<GameObject> ("Prefabs/Player Impact Visual"), new Vector3 (Target.tile.Position.x, Target.tile.Position.y, Layers.Foreground), Quaternion.identity);
        AudioComponent.PlaySound (Sounds.BITE);
        Destroy (vfx, 0.2f);

        OnFinished (EnergyCost);
    }

    public override void OnAbilityConnected (GameObject targetedUnit) {
        try {
            var unit = targetedUnit.GetComponent<Unit> ();
            unit.HealthComponent.AdjustHealth (-Damage);
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