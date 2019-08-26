using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeUtil : MonoBehaviour {
    private static Board board;
    private static Dictionary<RangeComponentType, RangeComponent> rangeComponents;
    internal static List<PathfindingData> GetAoERange (Point mousePosition, Ability currentAbility) {
        var pathfinding = board.Pathfinding;
        var component = rangeComponents[currentAbility.AoERangeComponentType];
        return component
            .SetAoERange (currentAbility.AreaOfEffect).SetOwnerPos (mousePosition)
            .GetTilesInRange ();
    }

    internal void Initialize (Board _board) {
        board = _board;
        if (rangeComponents == null) {
            rangeComponents = new Dictionary<RangeComponentType, RangeComponent> () { { RangeComponentType.CONSTANT, new ConstantRange (new GameObject (), board, null) }, { RangeComponentType.LINE, new LinearRange (new GameObject (), board, null) }
            };
        }
    }
    // switch (ability.RangeComponentType) {
    //     case RangeComponentType.CONSTANT:
    //         rangeComponent = new ConstantRange (owner.gameObject, owner.Board, ability);
    //         break;
    //     case RangeComponentType.LINE:
    //         rangeComponent = new LinearRange (owner.gameObject, owner.Board, ability);
    //         break;
    //     case RangeComponentType.SELF:
    //         rangeComponent = new SelfRange (owner.gameObject, owner.Board, ability);
    //         break;
    //     default:
    //         return false;
    // }
    // return true;
}