using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Unit))]
public class UnitTypeInspector : Editor {
    // public UnitType Current {
    //     get {
    //         return (UnitType) target;
    //     }
    // }

    // private void OnEnable () {
    //     if (Current.Prefab == null) {
    //         Current.Prefab = (UnitBehaviour) Resources.Load ("Prefabs/Unit", typeof (UnitBehaviour));
    //     }
    // }

    // public override void OnInspectorGUI () {
    //     DrawDefaultInspector ();

    // }

}