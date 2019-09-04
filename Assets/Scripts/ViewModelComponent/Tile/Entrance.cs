using UnityEngine;

[System.Serializable]
public class Entrance : Tile {
    private Directions transitionsToThe;
    public bool isEnabled { get; private set; } = true;
    public virtual void SetDisabled () {
        Debug.Log (string.Format ("disabled"));

        var res = Resources.Load ("Materials/Entrance_Disabled", typeof (Material)) as Material;
        GetComponent<Renderer> ().material.color = res.color;
        isEnabled = false;
    }
    public virtual void SetEnabled () {
        Debug.Log (string.Format ("enabled"));
        var res = Resources.Load ("Materials/Entrance", typeof (Material)) as Material;
        GetComponent<Renderer> ().material.color = res.color;
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