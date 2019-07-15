using System;
using UnityEngine;

[Serializable, CreateAssetMenu (fileName = "Area Data", menuName = "Game/Map/Area Data")]
public class AreaData : ScriptableObject {
    public Point Location;
    public AreaStateData areaStateData;
}