using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Game/Save Data/Character Select Data")]
public class CharacterSelectData : ScriptableObject {
    public string Name;
    public string IconName;
    public PlayableUnits UnitType;
}