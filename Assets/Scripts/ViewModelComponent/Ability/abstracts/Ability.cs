public abstract class Ability : UnityEngine.ScriptableObject {
    public abstract void OnCalled ();
    public abstract void OnCommited ();
    public abstract void OnFinished ();
}