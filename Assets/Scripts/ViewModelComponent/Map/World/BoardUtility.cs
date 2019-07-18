using UnityEngine;

public static class BoardUtility {
    public static Point SetMinV2Int (LevelData ld) {
        int minX = 99;
        int minY = 99;

        for (int i = 0; i < ld.tiles.Count; i++) {
            if (ld.tiles[i].location.x < minX) {
                minX = ld.tiles[i].location.x;
            }
            if (ld.tiles[i].location.y < minY) {
                minY = ld.tiles[i].location.y;
            }
        }
        return new Point (minX, minY);
    }

    public static Point SetMaxV2Int (LevelData ld) {
        int maxX = -99;
        int maxY = -99;

        for (int i = 0; i < ld.tiles.Count; i++) {
            if (ld.tiles[i].location.x > maxX) {
                maxX = ld.tiles[i].location.x;
            }
            if (ld.tiles[i].location.y > maxY) {
                maxY = ld.tiles[i].location.y;
            }
        }
        return new Point (maxX, maxY);
    }

    public static Point mousePosFromScreenPoint () {
        return Camera.main.ScreenToWorldPoint (
            new Vector3 (
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane)).ToPoint ();
    }
}