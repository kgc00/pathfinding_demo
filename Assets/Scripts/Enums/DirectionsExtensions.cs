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
        if (t1.Position.x > t2.Position.x)
            return Directions.West;
        return Directions.None;
    }
    public static Vector3 ToEuler (this Directions d) {
        return new Vector3 (0, 0, (int) d * 90);
    }

    public static Point ToPoint (this Directions d) {
        switch (d) {
            case Directions.North:
                return new Point (0, 1);
            case Directions.South:
                return new Point (0, -1);
            case Directions.West:
                return new Point (1, 0);
            case Directions.East:
                return new Point (-1, 0);
            default:
                return new Point (0, 0);
        }
    }
}