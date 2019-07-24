using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Unit))]
public class AbilityComponent : MonoBehaviour {
    public List<Ability> EquippedAbilities = new List<Ability> ();
    Unit owner;
    public Ability CurrentAbility { get; private set; }
    RangeComponent rangeComponent;
    MovementComponent movement;

    public void Initialize (UnitData data, Unit owner) {
        this.owner = owner;

        // call into a util to dynamically generate the kind of component we should be using
        this.movement = UnitUtility.AddMovementComponentFromType (data.MovementType, owner.gameObject);
        this.movement.Initialize (owner.Board, owner);

        // we load our units from unitdata/abilitydata and convert that into
        // script and class instances
        this.EquippedAbilities = AbilityFactory.CreateAbilitiesFromData (data.EquippedAbilities, owner);
    }

    internal void ActivateWithCallback (System.Action<float> OnFinished) {
        // set callback so we can use it to advance the owner's state on finished
        CurrentAbility.OnFinished = OnFinished;

        CurrentAbility.Activate (null);
    }

    // called from unit's idle state when the user selects an ability via keypress
    // will set current ability and provide range component
    public bool SetCurrentAbility (int i) {
        if (i <= EquippedAbilities.Count) {
            CurrentAbility = EquippedAbilities[i];
            return SetRangeComponent ();
        } else {
            Debug.LogError (string.Format ("tried to set ability to an out of bounds index"));
            return false;
        }
    }

    public bool SetCurrentAbility (Ability ability) {
        var toEquip = EquippedAbilities.Find (abil => ability == abil);
        if (toEquip) {
            CurrentAbility = toEquip;
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
        CurrentAbility.Movement = movement;
        return true;
    }

    // so we can use this component as an adapter to refer to 
    // whatever concrete implementation is needed at the time
    public List<PathfindingData> GetTilesInRange () {
        return rangeComponent.GetTilesInRange ();
    }
}