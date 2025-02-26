using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	/*public void LoadLevel(string name) {
		Debug.Log ("Load level requested for " + name);
		Application.LoadLevel (name);
	}*/

        public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

	public void QuitRequest () {
		Debug.Log ("I quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
	}
}
