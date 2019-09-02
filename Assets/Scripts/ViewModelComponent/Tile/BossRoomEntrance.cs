using UnityEngine;

[System.Serializable]
public class BossRoomEntrance : Entrance {
    private Directions transitionsToThe;
    private string key = "Boss 1";

    public void SetLockedStatus (bool shouldUnlock) {
        if (shouldUnlock) SetEnabled ();
        else SetDisabled ();
    }
}