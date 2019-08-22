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

        CurrentAbility.Activate ();
    }

    // called from unit's idle state when the user selects an ability via keypress
    // will set current ability and provide range component
    public bool SetCurrentAbility (int i) {
        if (i >= EquippedAbilities.Count) return false;

        var val = SetCurrentAbility (EquippedAbilities[i]);
        return val;
    }

    public int IndexOfCurrentAbility () {
        for (int i = 0; i < EquippedAbilities.Count; i++)
            if (CurrentAbility == EquippedAbilities[i])
                return i;

        return -1;
    }

    public bool SetCurrentAbility (Ability ability) {
        var toEquip = EquippedAbilities.Find (abil => ability == abil);
        if (!toEquip) {
            Debug.LogError (string.Format ("tried to set ability to an out of bounds index"));
            return false;
        }
        if (toEquip.EnergyCost > owner.EnergyComponent.CurrentEnergy) return false;

        if (!SetRangeComponent (toEquip)) return false;

        CurrentAbility = toEquip;
        return true;
    }

    // refactor to initialize once at start of game and reuse references
    private bool SetRangeComponent (Ability ability) {
        switch (ability.RangeComponentType) {
            case RangeComponentType.CONSTANT:
                rangeComponent = new ConstantRange (owner, ability);
                break;
            case RangeComponentType.LINE:
                rangeComponent = new LinearRange (owner, ability);
                break;
            case RangeComponentType.SELF:
                rangeComponent = new SelfRange (owner, ability);
                break;
            default:
                return false;
        }
        return true;
    }

    internal bool PrepAbility (List<PathfindingData> tilesInRange, PathfindingData selectedTile) {
        if (CurrentAbility == null) {
            Debug.LogError (string.Format ("Need to set a current ability before we prep it."));
            return false;
        }

        if (tilesInRange == null) { Debug.Log (string.Format ("tiles in range was never set")); return false; }
        if (selectedTile == null) { Debug.Log (string.Format ("selectedTile was never set")); return false; }
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