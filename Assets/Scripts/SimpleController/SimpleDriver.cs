using UnityEngine;

public abstract class SimpleDriver : MonoBehaviour {
    [SerializeField] protected Board board;
    [SerializeField] protected Unit owner;
    [SerializeField] protected MovementComponent movement;

    public virtual void Initialize (Board board, Unit owner, MovementComponent movement) {
        this.board = board;
        this.owner = owner;
        this.movement = movement;
    }

    public virtual void EnterIdle () { }
    public virtual void IdleState () { }
    public virtual void EnterPrep () { }
    public virtual void PrepState () { }
    public virtual void EnterActing () { }
    public virtual void ActingState () { }
    public virtual void EnterCooldown () { }
    public virtual void CooldownState () { }
}