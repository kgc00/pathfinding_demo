using System;
using UnityEngine;

[Serializable]
public struct AreaStateData {
    public LevelData initialLevel;
    [HideInInspector] public LevelData currentInstance;
    public Directions from;
    public AreaTypes areaType;
}