using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    [SerializeField] protected Board board;
    [SerializeField] protected Monster owner;
    [SerializeField] protected WalkingMovement movement;

    public virtual void Initialize (Board board, Monster owner, WalkingMovement movement) {
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