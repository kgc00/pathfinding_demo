using System;
using UnityEngine;

[Serializable]
public struct AreaStateData {
    public LevelData initialLevel;
    [HideInInspector] public LevelData currentInstance;
    public Directions from;
    public AreaStateData (LevelData initialLevel) {
        this.initialLevel = initialLevel;
        this.currentInstance = initialLevel;
        this.from = Directions.North;
        UnityEngine.Debug.Log (string.Format ("called asd"));
    }
}