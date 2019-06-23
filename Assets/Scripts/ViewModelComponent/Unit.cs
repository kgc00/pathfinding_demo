using UnityEngine;

[System.Serializable]
public class Unit : MonoBehaviour {
    public Point Position { get; protected set; }
    public Board Board { get; protected set; }
    public UnitStates State;
    [SerializeField] public UnitTypes TypeReference;
    public Directions dir;

    public virtual void Initialize (Board board, Point pos, UnitTypes r) {
        this.Position = pos;
        this.Board = board;
        this.TypeReference = r;
        this.State = UnitStates.IDLE;
        transform.localEulerAngles = dir.ToEuler ();
    }

    public virtual void SetState (UnitStates state) { }

    protected virtual void Update () {
        this.Position = new Point ((int) Mathf.Round (transform.position.x), (int) Mathf.Round (transform.position.y));
    }
}