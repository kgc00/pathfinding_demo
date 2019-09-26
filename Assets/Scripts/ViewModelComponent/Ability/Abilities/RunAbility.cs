using UnityEngine;

public class RunAbility : MovementAbility {
    public override void Activate () {
        if (Owner is Hero) {
            AudioComponent.PlaySound (Sounds.RUNNING);
        }
        // start a timer with a callback to transition to the next state
        StartCoroutine (Movement.Traverse (TilesInRange, Target, () => {
            OnDestinationReached ();
        }));
    }

    // for the case where there is more complex logic we have a place to put it
    public override void OnDestinationReached () {
        if (Owner is Hero) AudioComponent.StopSound (Sounds.RUNNING);
        OnFinished (EnergyCost);
    }

    public override void Assign (AbilityData data, Unit owner) {
        this.DisplayName = data.DisplayName;
        this.Range = data.Range;
        this.EnergyCost = data.EnergyCost;
        this.RangeComponentType = data.RangeComponentType;
        this.Owner = owner;
        this.Description = data.Description;
        this.TargetType = data.TargetType;
    }
}