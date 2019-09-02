using UnityEngine;

[System.Serializable]
public class Entrance : Tile {
    private Directions transitionsToThe;
    public bool isEnabled { get; private set; } = true;
    public void SetDisabled () {
        isEnabled = false;
    }
    public void SetEnabled () {
        isEnabled = true;
    }
    public override void SetOccupied (Unit occupier) {
        base.SetOccupied (occupier);
        if (occupier is Hero && isEnabled)
            EventQueue.AddEvent (new TransitionEventArgs (this, null, transitionsToThe.ToPoint ()));
    }

    public void SetTransitionDirection (Directions dir) {
        transitionsToThe = dir;
    }
}