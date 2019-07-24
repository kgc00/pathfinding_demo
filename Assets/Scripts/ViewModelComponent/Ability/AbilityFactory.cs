using System.Collections.Generic;
using UnityEngine;

public class AbilityFactory : MonoBehaviour {
    public static List<Ability> CreateAbilitiesFromData (List<AbilityData> data, Unit owner) {
        List<Ability> retVal = new List<Ability> ();
        foreach (var ability in data) {
            Ability instance;
            switch (ability.AbilityType) {
                case Abilities.RUN_ABILITY:
                    instance = owner.gameObject.AddComponent<RunAbility> ();
                    instance.Assign (ability);
                    retVal.Add (instance);
                    break;
                case Abilities.BITE_ABILITY:
                    instance = owner.gameObject.AddComponent<BiteAbility> ();
                    instance.Assign (ability);
                    retVal.Add (instance);
                    break;
                default:
                    break;
            }
        }
        return retVal;
    }
}