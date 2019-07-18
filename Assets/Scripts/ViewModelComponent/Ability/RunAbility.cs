using UnityEngine;

[CreateAssetMenu (menuName = "Game/Ability/Generic/Run")]
public class RunAbility : MovementAbility {
    public override void Activate () {
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
}