using UnityEngine;
using System.Collections;

public class ToggleLighting : MonoBehaviour {

	private static ToggleLighting mInstance;
	public static ToggleLighting instance {get{return mInstance;}}

	public GameObject[] l = new GameObject[2];

	GameObject prevObject;

	// Use this for initialization
	void Start () {
		if (mInstance == null) {
			mInstance = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeLightLevels (bool changeDirection) {

		for (int i = 0; i < l.Length ; i++) {
		
			l[i].gameObject.GetComponent<LightSlider>().lightDirection(changeDirection);
		}

	}

	public void SetLayerRecursively (GameObject go, int layerNumber, bool switchLevels) {

		if (prevObject != null && prevObject.gameObject != go.gameObject) {
			foreach (Transform trans in prevObject.GetComponentsInChildren<Transform>(true)) {
				trans.gameObject.layer = 0;
			}
		}
		if (go != null) {
			foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
				trans.gameObject.layer = layerNumber;
			}
		} 

		prevObject = go.gameObject;
		prevObject.gameObject.name = go.gameObject.name;
	//	if (prevObject.gameObject.name != go.gameObject.name) {
		//	print (prevObject.gameObject.name);
	//	}
			changeLightLevels (switchLevels);
	}

	public void lightBack () {

		changeLightLevels (false);


	}


}
