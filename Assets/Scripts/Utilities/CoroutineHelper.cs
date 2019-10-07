using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour {
    public static CoroutineHelper Instance;
    private Dictionary<object, InterupptibleCoroutine> allInterruptibleRoutines = new Dictionary<object, InterupptibleCoroutine> ();

    private void Awake () {
        if (Instance != this && Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
    }

    /// <summary>
    ///  Wraps coroutine to allow us to cancel it via a niave 3rd party.
    /// <para>
    ///  Assumes only ONE coroutine will be active per object at a time.
    /// </para>
    /// <param name="owner">object to associate as key</param>
    /// <param name="timeToWait">countdown delay</param>
    /// <param name="onComplete">callback used</param>
    /// </summary>
    public Coroutine StartInterruptibleRoutine (object owner, float timeToWait, System.Action onComplete, System.Action onInterrupted) {
        var routine = StartCoroutine (countdown (owner, timeToWait, onComplete));
        allInterruptibleRoutines.Add (owner, new InterupptibleCoroutine (routine, onInterrupted));
        return routine;
    }

    /// <summary>
    ///  Cancels an ongoing coroutine if it exists.  Can be called by a niave 3rd party.
    /// <param name="owner">object to associate as key</param>
    /// </summary>
    public void SafelyInterruptCoroutine (object owner) {
        if (allInterruptibleRoutines.ContainsKey (owner)) {
            StopCoroutine (allInterruptibleRoutines[owner].coroutine);
            allInterruptibleRoutines[owner].onInterrupted ();
            allInterruptibleRoutines.Remove (owner);
        }
    }

    private IEnumerator countdown (object owner, float timeToWait, System.Action onComplete) {
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
        }

        if (allInterruptibleRoutines.ContainsKey (owner)) {
            allInterruptibleRoutines.Remove (owner);
        }

        onComplete ();
    }

    /// old stuff
    /// keeping for convenience

    // public void Stop (Coroutine routine) {
    //     StopCoroutine (routine);
    // }

    // public Coroutine StartCountdown (float timeToWait, System.Action onComplete) {
    //     return StartCoroutine (countdown (timeToWait, onComplete));
    // }
    // public Coroutine CoroutineFromEnumerator (IEnumerator arg) {
    //     return StartCoroutine (arg);
    // }
}