using UnityEngine;

public class EditorState : UnitState {

    public EditorState () { }

    public override void Enter () { }
    public override UnitState HandleInput () {
        return null;
    }
}