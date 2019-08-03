using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData : ScriptableObject {

    public List<TileSpawnData> tiles;

    public List<UnitSpawnData> units;
}