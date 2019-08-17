using UnityEngine;

public class ProjectileComponent : MonoBehaviour {
    Point start;
    Board board;
    System.Action<Unit> OnConnected;
    float speed = 1.5f;
    public Point Direction;

    //optional param for setting projectile speed
    public void Initialize (Point dir, System.Action<Unit> callback, float speed = 1.5f) {
        start = transform.position.ToPoint ();
        this.Direction = dir;
        this.OnConnected = callback;
        this.speed = speed;
        this.board = FindObjectOfType<Board> ().GetComponent<Board> ();
    }

    void Update () {
        if (board) {
            MoveGameObject ();
            Point p = AssignPosIfInBounds ();
            CollisionLogic (p);
        } else {
            // destroy object when we switch rooms
            Destroy (gameObject);
        }
    }

    private void MoveGameObject () {
        transform.Translate (new Vector3 (
            Direction.x * Time.deltaTime * speed,
            Direction.y * Time.deltaTime * speed, 0
        ));
    }

    private Point AssignPosIfInBounds () {
        Point p = transform.position.ToPoint ();
        if (!board.TileAt (p))
            Destroy (gameObject);
        return p;
    }

    private void CollisionLogic (Point p) {
        var unit = board.UnitAt (p);
        if (unit) {
            OnConnected (unit);
            Destroy (gameObject);
        }
    }
}