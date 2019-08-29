using UnityEngine;
public class UnitFactory : MonoBehaviour {
    public static Unit CreateUnitFromType (Point p, UnitTypes type) {
        Unit unit = null;
        object instance;
        switch (type) {
            case UnitTypes.HERO:
                instance = Instantiate (Resources.Load ("Prefabs/Hero", typeof (Hero)),
                    new Vector3 (p.x, p.y, -2), Quaternion.identity) as Hero;
                unit = instance as Unit;
                break;
            case UnitTypes.MONSTER:
                instance = Instantiate (Resources.Load ("Prefabs/Monster", typeof (Monster)),
                    new Vector3 (p.x, p.y, -2), Quaternion.identity) as Monster;
                unit = instance as Unit;
                break;
            default:
                Debug.LogError ("unit should not be null and is.");
                break;
        }

        return unit;
    }
}