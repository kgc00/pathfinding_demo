public class IdleState : UnitState {
    AbilityComponent abilityComponent;
    public IdleState (Unit Owner) : base (Owner) {
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
                return null;

            // transition to the next state with that data
            if (abilityComponent.SetCurrentAbility (i))
                return new PrepState (Owner);
        }

        return null;
    }
}