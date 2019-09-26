using UnityEngine;

[System.Serializable]
public class Entrance : Tile {
    private Directions transitionsToThe;
    ///<summary>
    /// Sets whether the entrance will trigger if a unit steps on it
    ///</summary>
    public bool isEnabled { get; private set; } = true;
    public virtual void SetDisabled () {
        var res = Resources.Load ("Materials/Entrance_Disabled", typeof (Material)) as Material;
        GetComponent<Renderer> ().material.color = res.color;
        isEnabled = false;
    }
    public virtual void SetEnabled () {
        var res = Resources.Load ("Materials/Entrance", typeof (Material)) as Material;
        GetComponent<Renderer> ().material.color = res.color;
        isEnabled = true;
    }

    public override void SetOccupied (Unit occupier) {
        base.SetOccupied (occupier);
        if (occupier is Hero && isEnabled && Board.Area.State is ActiveState) {
            AudioComponent.StopSound (Sounds.RUNNING);
            AudioComponent.PlaySound (Sounds.ENTRANCE);
            EventQueue.AddEvent (new TransitionEventArgs (this, null, transitionsToThe.ToPoint ()));
        }
    }

    public void SetTransitionDirection (Directions dir) {
        transitionsToThe = dir;
    }
}