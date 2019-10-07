using System;
using UnityEngine;

class CharacterSelector : MonoBehaviour {
    CharacterSelectUI characterSelectUI;
    void Update () {
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            MoveCursorLeft ();
        } else if (Input.GetKeyDown (KeyCode.RightArrow)) {
            MoveCursorRight ();
        } else if (Input.GetKeyDown (KeyCode.A)) {
            MoveCursorLeft ();
        } else if (Input.GetKeyDown (KeyCode.D)) {
            MoveCursorRight ();
        }
    }

    void MoveCursorLeft () {
        var newVal = mod_floor (characterSelectUI.ActiveItemIndex - 1, characterSelectUI.PanelItems.Count);
        characterSelectUI.UpdateActiveItem (newVal);
    }
    void MoveCursorRight () {
        var newVal = mod_floor (characterSelectUI.ActiveItemIndex + 1, characterSelectUI.PanelItems.Count);
        characterSelectUI.UpdateActiveItem (newVal);
    }

    private int mod_floor (int difference, int size) {
        return ((difference % size) + size) % size;
    }

    internal CharacterSelector Initialize (CharacterSelectUI characterSelectUI) {
        this.characterSelectUI = characterSelectUI;
        return this;
    }
}