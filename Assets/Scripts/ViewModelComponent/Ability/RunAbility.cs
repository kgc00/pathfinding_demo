using UnityEngine;

public class RunAbility : MovementAbility {
    public override void Activate (AbilityData data) {
        // start a timer with a callback to transition to the next state
        CoroutineHelper.Instance.CoroutineFromEnumerator (
            Movement.Traverse (TilesInRange, Target, () => {
                OnDestinationReached ();
            }));
    }

    // for the case where there is more complex logic we have a place to put it
    public override void OnDestinationReached () {
        OnFinished (CooldownDuration);
    }

    public override void Assign (AbilityData data) {
        this.Range = data.Range;
        this.CooldownDuration = data.CooldownDuration;
        this.RangeComponentType = data.RangeComponentType;
    }
}