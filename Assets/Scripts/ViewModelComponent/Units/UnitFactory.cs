using UnityEngine;
public class UnitFactory : MonoBehaviour {
    Board board;
    Transform unitWrapper;

    public void Initialize (Board board, Transform unitWrapper) {
        this.board = board;
        this.unitWrapper = unitWrapper;
    }

    public Unit CreateUnitFromType (Point p, UnitTypes type) {
        Unit unit;
        switch (type) {
            case UnitTypes.HERO:
                unit = Instantiate (Resources.Load ("Prefabs/Hero", typeof (Hero)),
                    new Vector3 (p.x, p.y, -2), Quaternion.identity) as Unit;
                break;
            case UnitTypes.SLIME:
                unit = Instantiate (Resources.Load ("Prefabs/Slime", typeof (Monster)),
                    new Vector3 (p.x, p.y, -2), Quaternion.identity) as Unit;
                break;
            case UnitTypes.GOBLIN_ARCHER:
                Debug.LogError ("unit should not be null and is.");
                unit = null;
                break;
            case UnitTypes.NONE:
                Debug.LogError ("unit should not be null and is.");
                unit = null;
                break;
            default:
                Debug.LogError ("unit should not be null and is.");
                unit = null;
                break;
        }

        unit.name = unit.TypeReference.ToString ();
        unit.transform.SetParent (unitWrapper);
        return unit;
    }

    public void ActivateEnemyAt (Point p) {
        Unit instance = null;
        Unit unit = board.UnitFromStartLocation (p);

        if (!unit || !unit.GetComponent<Monster> ()) {
            Debug.LogError (string.Format ("unable to activate unit"));
            return;
        }

        instance = unit.GetComponent<Monster> ();
        instance.Initialize (board, UnitTypes.SLIME, p);
        unit.LoadUnitState (Resources.Load<UnitData> ("Beastiary/Slime"));
    }

    public void InitializePlayerUnitAt (Point p) {
        Unit unit = board.UnitFromStartLocation (p);

        unit.GetComponent<Unit> ().Initialize (board, unit.TypeReference, p);

        if (unit == null) {
            Debug.LogError ("unit should not be null and is.");
            return;
        }

        unit.transform.SetParent (board.transform.Find ("Units").transform);
        unit.LoadUnitState (WorldSaveComponent.GetPlayerStats ());
    }
}