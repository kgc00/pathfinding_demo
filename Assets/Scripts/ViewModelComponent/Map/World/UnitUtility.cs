using UnityEngine;

public static class UnitUtility {
    public static MovementComponent AddMovementComponentFromType (MovementType type, GameObject owner) {
        switch (type) {
            case MovementType.WALKING:
                return owner.AddComponent<WalkingMovement> ();
            default:
                return null;
        }
    }
}