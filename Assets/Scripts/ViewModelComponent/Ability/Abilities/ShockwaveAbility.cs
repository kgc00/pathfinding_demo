using System;
using System.Collections;
using UnityEngine;

public class ShockwaveAbility : AttackAbility {
    public override void Activate () {
        StartCoroutine (countdown (1.15f, () => SpawnShockwave ()));
    }

    private void SpawnShockwave () {
        if (Owner == null) return;

        TilesInRange.ForEach (data => OnAbilityConnected (data.tile.gameObject));

        OnFinished (EnergyCost);
    }

    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
        }
        onComplete ();
    }
    public override void OnAbilityConnected (GameObject targetedTile) {
        try {
            var unit = targetedTile.GetComponent<Tile> ().OccupiedBy;
            var pos = targetedTile.transform.position;
            if (unit != null) {
                unit.HealthComponent.AdjustHealth (-Damage);
            }
            AudioComponent.PlaySound (Sounds.BOMB);
            var vfx = Instantiate (Resources.Load<GameObject> ("Prefabs/Player Impact Visual"), new Vector3 (pos.x, pos.y, Layers.Foreground), Quaternion.identity);
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