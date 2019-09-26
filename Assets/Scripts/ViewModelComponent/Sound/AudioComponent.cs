using UnityEngine;

public class AudioComponent : MonoBehaviour {
    public static AudioComponent Instance;
    void Awake () {
        if (Instance != null) {
            Destroy (gameObject);
        } else {
            Instance = this;
        }
        DontDestroyOnLoad (gameObject);
    }

    public static void PlaySound (Sounds sound) {
        AudioUtil.AudioSourceFromType (sound).Play ();
    }
    public static void StopSound (Sounds sound) {
        var source = AudioUtil.AudioSourceFromType (sound);
        if (source.isPlaying) {
            source.Stop ();
        }
    }
}