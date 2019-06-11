using UnityEngine;

public static class TypeHelper {
    public static void TypeFromEnum (TileTypes tt) {
        switch (tt) {
            case TileTypes.DIRT:
                // return a reference to the monobehaviour type
                Debug.Log ("dirt");
                break;
                // return TileTypes.tile;
            case TileTypes.WALL:
                Debug.Log ("wall");
                break;
                // return TileNames.wall;
            default:
                Debug.Log ("nada");
                break;
                // return "null";
        }
    }
    // void StringFromType (T t) { }
}