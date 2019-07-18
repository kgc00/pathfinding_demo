using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Unit))]
public class AbilityComponent : MonoBehaviour {
    [SerializeField] public List<Ability> equippedAbilities;
    Unit owner;
    public Ability CurrentAbility { get; private set; }
    RangeComponent rangeComponent;
    Movement movement;

    public void Initialize (UnitData data, Unit owner) {
        this.owner = owner;
        this.movement = owner.Movement;
        equippedAbilities = data.equippedAbilities;
    }

    internal void ActivateWithCallback (System.Action<float> OnFinished) {
        // set callback so we can use it to advance the owner's state on finished
        CurrentAbility.OnFinished = OnFinished;

        CurrentAbility.Activate ();
    }

    // called from unit's idle state when the user selects an ability via keypress
    // will set current ability and provide range component
    public bool SetCurrentAbility (int i) {
        if (i <= equippedAbilities.Count) {
            CurrentAbility = equippedAbilities[i];
            return SetRangeComponent ();
        } else {
            Debug.LogError (string.Format ("tried to set ability to an out of bounds index"));
            return false;
        }
    }

    private bool SetRangeComponent () {
        switch (CurrentAbility.RangeComponentType) {
            case RangeComponentType.CONSTANT:
                rangeComponent = new ConstantRange (owner, CurrentAbility);
                break;
            case RangeComponentType.LINE:
                rangeComponent = new LinearRange (owner, CurrentAbility);
                break;
            case RangeComponentType.SELF:
                rangeComponent = new SelfRange (owner, CurrentAbility);
                break;
            default:
                return false;
        }
        return true;
    }

    internal bool PrepAbility (List<PathfindingData> tilesInRange, PathfindingData selectedTile) {
        if (CurrentAbility == null) {
            Debug.LogError (string.Format ("Should never be null"));
            return false;
        }
        CurrentAbility.TilesInRange = tilesInRange;
        CurrentAbility.Target = selectedTile;
        CurrentAbility.Movement = owner.Movement;
        return true;
    }

    // so we can use this component as an adapter to refer to 
    // whatever concrete implementation is needed at the time
    public List<PathfindingData> GetTilesInRange () {
        return rangeComponent.GetTilesInRange ();
    }
}