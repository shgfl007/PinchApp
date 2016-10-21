using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftLights : MonoBehaviour {
	
	ThereAndBack clickObject;
	bool initDone = false;
	
	void Start() {
	}
	
	void Update() {
	}
	
	public void onClickGo() {
		if (!initDone) {
			init ();
		}
		clickObject.go (1.1f);
	}
	
	void init() {
		clickObject = new ThereAndBack ();

		// RightUI
		ObjectParameters rightUIParameters = new ObjectParameters ();
		rightUIParameters.position = new Vector3 (-30f, -0.15f, -30f);
		rightUIParameters.alpha = 0;
		clickObject.add ("RightUI", rightUIParameters);
		
		// LeftUI
		ObjectParameters leftUIParameters = new ObjectParameters ();
		leftUIParameters.position = new Vector3 (3.3f, 0, -8.3f);
		leftUIParameters.rotation = new Vector3 (0, 117, 0);
		clickObject.add ("LeftUI", leftUIParameters);
		
		// Featured Article
		ObjectParameters featuredParameters = new ObjectParameters ();
		featuredParameters.position = new Vector3 (0, 0, -30);
		featuredParameters.alpha = 0;
		clickObject.add ("Featured", featuredParameters);

		/*
		// Floor
		ObjectParameters floorParameters = new ObjectParameters ();
		floorParameters.alpha = 0;
		clickObject.add ("Flooring", floorParameters);
		*/
		
		initDone = true;
	}
}