using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu (fileName = "World Data", menuName = "Game/Map/World Data")]
public class WorldData : ScriptableObject {
    public List<AreaData> areaData;
}