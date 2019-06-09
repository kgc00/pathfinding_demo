using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Tile))]
public class TileTypeInspector : Editor {
    // public TileType Current
    // {
    //     get
    //     {
    //         return (TileType)target;
    //     }
    // }

    // private void OnEnable()
    // {
    //     if (Current.Prefab == null)
    //     {
    //         Current.Prefab = (TileBehaviour)Resources.Load("Prefabs/Tile", typeof(TileBehaviour));
    //     }
    // }

    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();
    // }

}