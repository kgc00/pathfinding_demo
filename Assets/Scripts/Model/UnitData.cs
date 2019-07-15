using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Game/Save Data/Unit Data")]
public class UnitData : ScriptableObject {
    public List<Ability> equippedAbilities;
    public int currentHP;
    public int maxHP;
    public UnitTypes type;
    public void Assign (List<Ability> a, int chp, int mhp, UnitTypes t) {
        this.equippedAbilities = a;
        this.currentHP = chp;
        this.maxHP = mhp;
        this.type = t;
    }
    public void Assign (UnitData data) {
        this.equippedAbilities = data.equippedAbilities;
        this.currentHP = data.currentHP;
        this.maxHP = data.maxHP;
        this.type = data.type;
    }
}