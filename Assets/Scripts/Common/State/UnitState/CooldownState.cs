using UnityEngine;

public class CooldownState : UnitState {
    private UnitState state;
    float cooldownDuration;
    public CooldownState (Unit Owner, float cooldownDuration) : base (Owner) {
        this.cooldownDuration = cooldownDuration;
    }
    public override void Enter () {
        BoardVisuals.RemoveTilesFromHighlightsByUnit (Owner);
        Color baseColor = TempChangeColor ();
        // start a timer with a callback to transition to the next state
        CoroutineHelper.Instance.StartCountdown (cooldownDuration,
            () => this.UpdateState (baseColor));
    }

    public void UpdateState (Color c) {
        // should probably only fire when area is in setupstate
        if (Owner is Hero) {
            EventQueue.AddEvent (new AreaStateChangeEventArgs (Owner, null, AreaStateTypes.Active));
        }
        Owner.gameObject.GetComponentInChildren<Renderer> ().material.color = c;
        state = new IdleState (Owner);
    }

    // return null until UpdateState is called;
    public override UnitState HandleInput (Controller controller) { return state; }

    private Color TempChangeColor () {
        Renderer rend = Owner.gameObject.GetComponentInChildren<Renderer> ();
        Color baseColor = rend.material.color;
        rend.material.color = Color.black;
        return baseColor;
    }
}