using UnityEngine;

public class PlayerCooldownState : UnitState {
    private UnitState state;
    float cooldownDuration;
    public PlayerCooldownState (Unit Owner, float cooldownDuration) : base (Owner) {
        this.cooldownDuration = cooldownDuration;
    }
    public override void Enter () {
        BoardVisuals.RemoveTilesFromHighlightsByUnit (Owner);
        BoardVisuals.RemoveIndicator (Owner);
        this.UpdateState ();

        // can add some small delay to prevent acting constantly 

        // Color baseColor = TempChangeColor ();
        // // start a timer with a callback to transition to the next state
        // CoroutineHelper.Instance.StartCountdown (cooldownDuration,
        //     () => this.UpdateState (baseColor));
    }

    public void UpdateState () {
        // should probably only fire when area is in setupstate
        if (!Owner) return;

        if (Owner is Hero) {
            EventQueue.AddEvent (new AreaStateChangeEventArgs (Owner, null, AreaStateTypes.Active));
        }

        state = new PlayerIdleState (Owner);
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