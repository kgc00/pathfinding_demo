public abstract class AttackAbility : Ability {
    public int Damage;
    public abstract void OnAbilityConnected (Unit targetedUnit);
}