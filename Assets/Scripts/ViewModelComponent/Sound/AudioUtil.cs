using UnityEngine;
public class AudioUtil : MonoBehaviour {
    public static AudioSource AudioSourceFromType (Sounds sound) {
        switch (sound) {
            case Sounds.HURT:
                return GameObject.Find ("Hurt").GetComponent<AudioSource> ();
            case Sounds.HURT2:
                return GameObject.Find ("Hurt 2").GetComponent<AudioSource> ();
            case Sounds.SHOOT:
                return GameObject.Find ("Shoot").GetComponent<AudioSource> ();
            case Sounds.BITE:
                return GameObject.Find ("Bite").GetComponent<AudioSource> ();
            case Sounds.BOMB:
                return GameObject.Find ("Bomb").GetComponent<AudioSource> ();
            case Sounds.BOMB_LAUNCHED:
                return GameObject.Find ("Bomb Launched").GetComponent<AudioSource> ();
            case Sounds.ENTRANCE:
                return GameObject.Find ("Entrance").GetComponent<AudioSource> ();
            case Sounds.AREA_CLEARED:
                return GameObject.Find ("Area Cleared").GetComponent<AudioSource> ();
            case Sounds.RUNNING:
                return GameObject.Find ("Running").GetComponent<AudioSource> ();
            case Sounds.PLAYER_DEATH:
                return GameObject.Find ("Player Death").GetComponent<AudioSource> ();
            case Sounds.ERROR:
                return GameObject.Find ("Error").GetComponent<AudioSource> ();
            default:
                return null;
        }
    }

    public static void PlayRandomHurtSound () {
        // pick one of two options randomly
        var seed = UnityEngine.Random.Range (0, 3);
        if (seed > 1) AudioComponent.PlaySound (Sounds.HURT);
        else AudioComponent.PlaySound (Sounds.HURT2);
    }
}