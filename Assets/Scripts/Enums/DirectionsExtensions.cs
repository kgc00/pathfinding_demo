using System.Collections;
using UnityEngine;
public static class DirectionsExtensions {
    public static Directions GetDirection (this Tile t1, Tile t2) {
        if (t1.Position.y < t2.Position.y)
            return Directions.North;
        if (t1.Position.x < t2.Position.x)
            return Directions.East;
        if (t1.Position.y > t2.Position.y)
            return Directions.South;
        return Directions.West;
    }
    public static Vector3 ToEuler (this Directions d) {
        return new Vector3 (0, (int) d * 90, 0);
    }

    public static Point ToPoint (this Directions d) {
        switch (d) {
            case Directions.West:
                return new Point (1, 0);
            case Directions.South:
                return new Point (0, 1);
            case Directions.East:
                return new Point (-1, 0);
            case Directions.North:
                return new Point (0, -1);
            default:
                return new Point (0, 0);
        }
    }
}