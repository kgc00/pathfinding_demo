using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour {
    public static void LoadScene (string name) {
        SceneManager.LoadScene (name);
    }
    public void DirectLoadScene (string name) {
        SceneManager.LoadScene (name);
    }
}