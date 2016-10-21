using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class HomeScreenMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SceneSelector(int scene) {

		SceneManager.LoadScene (scene); // Loads the selected Scene

	}

}
