public class BrainFactory {
    public static Brain CreateBrainFromUnit (Unit owner) {
        Brain brain;
        switch (owner.TypeReference) {
            case UnitTypes.SLIME:
                brain = new SlimeBrain (owner);
                break;
            case UnitTypes.GOBLIN_ARCHER:
                brain = new GoblinArcherBrain (owner);
                break;
            case UnitTypes.GOBLIN_WARRIOR:
                brain = new GoblinWarriorBrain (owner);
                break;
            case UnitTypes.GOBLIN_CHAMPION:
                brain = new GoblinChampionBrain (owner);
                break;
            default:
                brain = null;
                break;
        }
        return brain;
    }
}