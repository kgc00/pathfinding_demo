using UnityEngine;

[System.Serializable]
public class Entrance : Tile {
    private Directions transitionsToThe;

    public override void SetOccupied (Unit occupier) {
        base.SetOccupied (occupier);
        EventQueue.AddEvent (new TransitionEventArgs (this, null, transitionsToThe.ToPoint ()));
    }

    public void SetTransitionDirection (Directions dir) {
        transitionsToThe = dir;
    }
}