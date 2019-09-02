using UnityEngine;

public class ProjectileComponent : MonoBehaviour {
    Point start;
    Board board;
    System.Action<GameObject> OnConnected;
    float speed = 1.5f;
    public Point Direction;

    //optional param for setting projectile speed
    public void Initialize (Point dir, System.Action<GameObject> callback, float speed = 1.5f) {
        start = transform.position.ToPoint ();
        this.Direction = dir;
        this.OnConnected = callback;
        this.speed = speed;
        this.board = FindObjectOfType<Board> ().GetComponent<Board> ();
    }

    void Update () {
        // destroy object when we switch rooms
        if (!board) {
            Destroy (gameObject);
            return;
        }

        MoveGameObject ();
        Point? p = AssignPosIfInBounds ();
        CollisionLogic (p);
    }

    private void MoveGameObject () {
        transform.Translate (new Vector3 (
            Direction.x * Time.deltaTime * speed,
            Direction.y * Time.deltaTime * speed, 0
        ));
    }

    private Point? AssignPosIfInBounds () {
        Point p = transform.position.ToPoint ();
        if (!board.TileAt (p)) {
            Destroy (gameObject);
            return null;
        }
        if (!board.TileAt (p).isWalkable) {
            OnConnected (board.TileAt (p).gameObject);
            Destroy (gameObject);
        }
        return p;
    }

    private void CollisionLogic (Point? p) {
        if (p == null) return;

        var unit = board.UnitAt ((Point) p);
        if (unit) {
            OnConnected (unit.gameObject);
            Destroy (gameObject);
        }
    }
}