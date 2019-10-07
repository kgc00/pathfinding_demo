using UnityEngine;
class InterupptibleCoroutine {
    public Coroutine coroutine;
    public System.Action onInterrupted;
    public InterupptibleCoroutine (Coroutine coroutine, System.Action onInterrupted) {
        this.coroutine = coroutine;
        this.onInterrupted = onInterrupted;
    }
}