using UnityEngine;

[System.Serializable]
public class BossRoomEntrance : Entrance {
    private Directions transitionsToThe;
    public void SetLockedStatus (bool shouldUnlock) {
        if (shouldUnlock) SetEnabled ();
        else SetDisabled ();
    }
}