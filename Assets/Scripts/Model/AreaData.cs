using System;
using UnityEngine;

[Serializable, CreateAssetMenu (fileName = "Area Data", menuName = "Game/Area Data")]
public class AreaData : ScriptableObject {
    public Point Location;
    public LevelData LevelData;
}