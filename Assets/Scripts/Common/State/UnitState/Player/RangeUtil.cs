using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeUtil : MonoBehaviour {
    private static Board board;
    private static Dictionary<RangeComponentType, RangeComponent> rangeComponents;
    internal static List<PathfindingData> GetAoERangeFromMousePosition (Point mousePosition, Ability currentAbility) {
        return rangeComponents[currentAbility.AoERangeComponentType]
            .SetRange (currentAbility.AreaOfEffect)
            .SetStartPosFromMouse (mousePosition)
            .GetTilesInRange ();
    }

    internal static List<PathfindingData> SurveyBoard (Point startPosition, Board board) {
        return rangeComponents[RangeComponentType.CONSTANT]
            .SetRange (99)
            .SetOwnerPos (startPosition)
            .GetTilesInRange ();
    }

    internal void Initialize (Board _board) {
        board = _board;
        rangeComponents = new Dictionary<RangeComponentType, RangeComponent> () {
            {
            RangeComponentType.CONSTANT, new ConstantRange (new GameObject ("Constant Range Component"), board, null)
            }, {
            RangeComponentType.LINE,
            new LinearRange (new GameObject ("Linear Range Component"), board, null)
            }
        };
    }
}