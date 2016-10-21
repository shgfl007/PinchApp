using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class TickTick : MonoBehaviour {

	public float time = 11;
	public Text txt;
	// Use this for initialization
	void Start () {
	
		StartCoroutine (Ticker());

	}
	private IEnumerator Ticker() {

		float elapsedTime = time;

		while (elapsedTime > 0) {


			elapsedTime -= Time.deltaTime;
			txt.text = ((int)elapsedTime).ToString ();

			yield return new WaitForEndOfFrame ();
		}

		//Load the New Scene Right Here Dawg!

		GoToxScene.instance.sceneIndex = 3;
		GoToxScene.instance.GoToScene ();
	}

}
