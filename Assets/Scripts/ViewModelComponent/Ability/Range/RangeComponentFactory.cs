public class RangeComponentFactory {
    public static RangeComponent RangeComponentFromType (RangeComponentType type) {
        RangeComponent component;
        switch (type) {
            case RangeComponentType.CONSTANT:
                component = new ConstantRange (null, null, null);
                break;
            case RangeComponentType.LINE:
                component = new LinearObstructableRange (null, null, null);
                break;
            case RangeComponentType.SELF:
                component = new SelfRange (null, null, null);
                break;
            default:
                component = null;
                break;
        }
        return component;
    }
}