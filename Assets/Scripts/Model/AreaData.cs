using System;
using UnityEngine;

[Serializable, CreateAssetMenu (fileName = "Area Data", menuName = "Game/Map/Area Data")]
public class AreaData : ScriptableObject {
    public Point Location;
    public AreaStateData areaStateData;

    //  public static bool operator == (AreaData a, AreaData b) {
    //     return a.Location == b.Location && a.areaStateData == b.areaStateData;
    // }
    // public static bool operator != (AreaData a, AreaData b) {
    //     return a.x == b.x && a.y == b.y;
    // }
    // public override bool Equals (object obj) {
    //     if (obj is AreaData) {
    //         AreaData p = (AreaData) obj;
    //         return x == p.x && y == p.y;
    //     }
    //     return false;
    // }
    // public override int GetHashCode () {
    //     return x ^ y;
    // }
}