using System.Collections.Generic;
using UnityEngine;

// Experiment to see if we can build some architecture for generic, re-mappable keys to use for
// our command queries. (so we don't have to hunt code for getkeydowns when we change a command key)
internal class InputHandler : Controller {
    List<System.Func<bool>> confirm = new List<System.Func<bool>> ();
    List<System.Func<bool>> cancel = new List<System.Func<bool>> ();
    List<System.Func<bool>> abilityOne = new List<System.Func<bool>> ();
    List<System.Func<bool>> abilityTwo = new List<System.Func<bool>> ();
    List<System.Func<bool>> abilityThree = new List<System.Func<bool>> ();

    Dictionary<ControlTypes, List<System.Func<bool>>> controls;

    public override void Initialize (Unit owner) {
        base.Initialize (owner);
        confirm.Add (() => Input.GetMouseButtonDown (1));
        confirm.Add (() => Input.GetMouseButtonDown (0));
        cancel.Add (() => Input.GetKeyDown (KeyCode.Escape));
        abilityOne.Add (() => Input.GetKeyDown (KeyCode.Alpha1));
        abilityTwo.Add (() => Input.GetKeyDown (KeyCode.Alpha2));
        abilityThree.Add (() => Input.GetKeyDown (KeyCode.Alpha3));
        controls = new Dictionary<ControlTypes, List<System.Func<bool>>> { { ControlTypes.CONFIRM, confirm },
            { ControlTypes.CANCEL, cancel },
            { ControlTypes.ABILITY_ONE, abilityOne },
            { ControlTypes.ABILITY_TWO, abilityTwo },
            { ControlTypes.ABILITY_THREE, abilityThree }
        };
    }

    // take in a control command enum loop through all associated keys
    // for the command and return any query which is true
    public override bool DetectInputFor (ControlTypes type) {
        foreach (var entry in controls) {
            if (entry.Key == type) {
                foreach (var input in entry.Value) {
                    if (input ())
                        return input ();
                }
            }
        }
        return false;
    }
}