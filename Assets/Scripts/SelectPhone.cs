using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectPhone : MonoBehaviour {

	public int phoneId = 0; // Phone Ids

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void phoneSelector(int i) {
		phoneId = i; // Assign Dropdown menu option
        PlayerPrefs.SetInt("phoneID", phoneId);
        //print(phoneId); // Log the phone Option
    }

	//Proceed to next scene
	public void proceedToNextScene() {
		SceneManager.LoadScene (1);
	}


}
