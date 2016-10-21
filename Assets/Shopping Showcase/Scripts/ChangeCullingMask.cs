using UnityEngine;
using System.Collections;

public class ChangeCullingMask : MonoBehaviour {

	public KeyCode k;

	bool switchLevels = true;
	// Use this for initialization
	void Start () {
	
	}


	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (k)) {
			if (switchLevels) {
				ToggleLighting.instance.SetLayerRecursively (this.gameObject, 14, switchLevels);

			} else {
				ToggleLighting.instance.SetLayerRecursively (this.gameObject, 0, switchLevels);

			}
			switchLevels = !switchLevels;


		}

	}




}
