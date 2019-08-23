using System;
using UnityEngine;

public class DestinationComponent : MonoBehaviour {
    Point destination;
    Board board;
    Action<GameObject> onConnected;

    //optional param for setting projectile speed
    public void Initialize (Point destination, Action<GameObject> callback) {
        this.destination = destination;
        this.onConnected = callback;
        this.board = FindObjectOfType<Board> ().GetComponent<Board> ();
    }

    void Update () {
        CleanUp ();
        HandleCollision ();
    }

    private void CleanUp () {
        // destroy object when we switch rooms
        if (!board) {
            Destroy (gameObject);
            return;
        }
    }

    private void HandleCollision () {
        var p = this.transform.position.ToPoint ();

        if (p == destination || !board.TileAt (p).isWalkable) {
            onConnected (gameObject);
            Destroy (gameObject);
        }
        // i don't think it's possible to shoot a dest component out of bounds, 
        // so we don't need to check for that case.
    }
}