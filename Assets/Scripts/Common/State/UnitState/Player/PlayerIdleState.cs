public class PlayerIdleState : UnitState {
    public static System.Action<Unit, int> onAbilitySet = delegate { };
    public static System.Action onEntered = delegate { };
    AbilityComponent abilityComponent;
    public PlayerIdleState (Unit Owner) : base (Owner) {
        abilityComponent = Owner.AbilityComponent;
    }

    public override UnitState HandleInput (Controller controller) {
        // define our list of acceptable input for this frame and state
        bool[] pressed = new bool[] {
            controller.DetectInputFor (ControlTypes.ABILITY_ONE),
            controller.DetectInputFor (ControlTypes.ABILITY_TWO),
            controller.DetectInputFor (ControlTypes.ABILITY_THREE),
        };

        // loop through them and see if any have been pressed...
        for (int i = 0; i < pressed.Length; i++) {
            if (!pressed[i])
                continue;

            // transition to the next state with that data
            if (abilityComponent.SetCurrentAbility (i)) {
                onAbilitySet (Owner, i);
                return new PlayerPrepState (Owner);
            }
        }

        return null;
    }

    public override void Enter () {
        onEntered ();
    }
}