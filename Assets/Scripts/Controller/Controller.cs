using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    [SerializeField] protected Board board;
    [SerializeField] protected Unit owner;
    [SerializeField] protected WalkingMovement movement;

    public virtual void Initialize (Board board, Unit owner, WalkingMovement movement) {
        this.board = board;
        this.owner = owner;
        this.movement = movement;
    }

    public virtual void IdleState () { }

    public virtual void PrepState () { }
    public virtual void ActingState () { }

    public virtual void CooldownState () { }
}