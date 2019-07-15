using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Unit))]
public class AbilityComponent : MonoBehaviour {
    [SerializeField] public List<Ability> equippedAbilities;

    public void Initialize () {

    }
}