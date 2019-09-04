using UnityEngine;

[System.Serializable]
public class BossRoomEntrance : Entrance {
    private Directions transitionsToThe;
    public void SetLockedStatus (bool shouldUnlock) {
        if (shouldUnlock) SetEnabled ();
        else SetDisabled ();
    }
    public override void SetEnabled () {
        base.SetEnabled ();
        var res = Resources.Load ("Materials/BossEntrance", typeof (Material)) as Material;
        GetComponent<Renderer> ().material.color = res.color;

    }
}