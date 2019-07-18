using UnityEngine;
public abstract class Controller : MonoBehaviour {
    protected Unit owner;

    public virtual void Initialize (Unit owner) {
        this.owner = owner;
    }
    public virtual bool DetectInputFor (ControlTypes type) { return false; }
}