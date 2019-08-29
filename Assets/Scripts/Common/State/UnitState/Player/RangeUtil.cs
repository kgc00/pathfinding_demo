using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeUtil : MonoBehaviour {
    private static Board board;
    private static Dictionary<RangeComponentType, RangeComponent> rangeComponents;
    internal static List<PathfindingData> GetAoERange (Point mousePosition, Ability currentAbility) {
        return rangeComponents[currentAbility.AoERangeComponentType]
            .SetAoERange (currentAbility.AreaOfEffect)
            .SetOwnerPos (mousePosition)
            .GetTilesInRange ();
    }

    internal void Initialize (Board _board) {
        board = _board;
        rangeComponents = new Dictionary<RangeComponentType, RangeComponent> () {
            {
            RangeComponentType.CONSTANT, new ConstantRange (new GameObject (), board, null)
            }, {
            RangeComponentType.LINE,
            new LinearRange (new GameObject (), board, null)
            }
        };
    }
}