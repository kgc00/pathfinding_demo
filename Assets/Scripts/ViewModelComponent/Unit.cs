using UnityEngine;

[System.Serializable]
public class Unit : MonoBehaviour {
    public virtual Point Position => new Point ((int) Mathf.Round (transform.position.x), (int) Mathf.Round (transform.position.y));
    public virtual Board Board { get; protected set; }

    [SerializeField] public UnitTypes TypeReference;
    public Directions dir;
    private OccupationState occupationState;

    public virtual void Initialize (Board board, UnitTypes r) {
        this.Board = board;
        this.TypeReference = r;
        transform.localEulerAngles = dir.ToEuler ();
        occupationState = new OccupationState (this, Board);
        occupationState.Enter ();
    }

    protected virtual void Update () {
        occupationState.Update ();
    }
}