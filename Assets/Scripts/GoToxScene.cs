using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class GoToxScene : MonoBehaviour {
	private static GoToxScene mInstance;
	public static GoToxScene instance { get { return mInstance; } }
	public int sceneIndex = 5;

	// Use this for initialization
	void Start () {
	
		if (mInstance == null) {
		
			mInstance = this;
		}

		Invoke ("delayLoad", 0.1f);


	}

	void delayLoad() {

		if (FadeBlack.instance != null) {
			FadeBlack.instance.resetColor ();
			FadeBlack.instance.fadeFromBlack = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToScene () {
		if (FadeBlack.instance != null) {
			
			FadeBlack.instance.fadeToBlack = true;
			Invoke ("changeScene", 1.0f);
		} else {
		
			changeScene ();
		}
	}

	private void changeScene() {


		if (TrackerScript.instance != null) {
			TrackerScript.instance.changeScene ();
		
		}

		Invoke ("testingDelay", 0.2f);
	}

	public void testingDelay () {

		SceneManager.LoadScene (sceneIndex);


	}

}
