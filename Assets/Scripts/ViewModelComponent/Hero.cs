using System.Collections;
using UnityEngine;

public class Hero : Unit {
    [SerializeField] private WalkingMovement movement;
    [SerializeField] private bool isDebug;
    IEnumerator prepState;
    private UnitState HeroState;

    public override void Initialize (Board board, UnitTypes r) {
        base.Initialize (board, r);

        movement = gameObject.AddComponent<WalkingMovement> ();
        movement.Initialize (board, this, 3);
        movement.isDebug = isDebug;

        HeroState = new IdleState (this, movement);
    }

    protected override void Update () {
        base.Update ();

        // if we returned a new state switch to it
        UnitState state = HeroState.HandleInput ();
        if (state == null)
            return;

        HeroState = state;
        HeroState.Enter ();
    }

    public override void InLevelEditor () {
        base.InLevelEditor ();
        HeroState = new EditorState ();
        Destroy (movement);
    }
}