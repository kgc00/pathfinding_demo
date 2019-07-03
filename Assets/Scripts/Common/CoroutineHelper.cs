using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour {
    public static CoroutineHelper Instance;

    private void Awake () {
        if (Instance != this && Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
    }

    public void Stop (Coroutine routine) {
        StopCoroutine (routine);
    }

    public Coroutine StartCountdown (float timeToWait, System.Action onComplete) {
        return StartCoroutine (countdown (timeToWait, onComplete));
    }

    private IEnumerator countdown (float timeToWait, System.Action onComplete) {
        while (timeToWait > 0) {
            timeToWait -= Time.deltaTime;
            yield return null;
        }
        onComplete ();
    }

    public Coroutine CoroutineFromEnumerator (IEnumerator arg) {
        return StartCoroutine (arg);
    }
}