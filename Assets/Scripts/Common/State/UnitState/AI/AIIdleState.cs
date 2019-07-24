using UnityEngine;
public class AIIdleState : UnitState {
    AbilityComponent abilityComponent;
    float randomDelay;
    float timeLeft;
    public AIIdleState (Unit Owner) : base (Owner) {
        abilityComponent = Owner.AbilityComponent;
        this.randomDelay = Random.Range (0, 2);
    }

    public override void Enter () {
        timeLeft = randomDelay;
    }
    public override UnitState HandleInput (Controller controller) {
        // transition to the next state with that data
        timeLeft -= Time.deltaTime;
        return timeLeft <= 0 ? new AIPrepState (Owner) : null;
    }
}