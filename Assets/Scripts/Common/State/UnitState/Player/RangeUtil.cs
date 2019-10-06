using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeUtil : MonoBehaviour {
    private static Dictionary<RangeComponentType, RangeComponent> rangeComponents;
    internal static List<PathfindingData> GetAoERangeFromMousePosition (Point mousePosition, Ability currentAbility) {
        return rangeComponents[currentAbility.AoERangeComponentType]
            .SetRange (currentAbility.AreaOfEffect)
            .SetStartPosFromMouse (mousePosition)
            .GetTilesInRange ();
    }

    public static List<PathfindingData> SurveyBoard (Point startPosition, Board board) {
        if (board != null) {
            return rangeComponents[RangeComponentType.CONSTANT]
                .SetRange (99)
                .SetOwnerPos (startPosition)
                .GetTilesInRange ();
        } else { return new List<PathfindingData> (); }
    }

    internal void Initialize (Board board) {
        if (rangeComponents != null) UpdateBoard (board);
        else CreateRangeComponents (board);
    }

    public static void ClearComponent () {
        rangeComponents.Clear ();
        rangeComponents = null;
    }
    private static void CreateRangeComponents (Board board) {
        var wrapperName = "Range Components";
        var wrapper = GameObject.Find (wrapperName) ? GameObject.Find (wrapperName) :
            new GameObject (wrapperName);

        var constantWrapper = new GameObject ("Constant Range Component");
        constantWrapper.transform.SetParent (wrapper.transform);

        var linearWrapper = new GameObject ("Constant Range Component");
        linearWrapper.transform.SetParent (wrapper.transform);

        rangeComponents = new Dictionary<RangeComponentType, RangeComponent> () {
            {
            RangeComponentType.CONSTANT, new ConstantRange (constantWrapper, board, null)
            }, {
            RangeComponentType.LINE,
            new LinearObstructableRange (linearWrapper, board, null)
            }
        };
    }

    void UpdateBoard (Board board) {
        foreach (var comp in rangeComponents) {
            comp.Value.SetBoard (board);
        }
    }
}