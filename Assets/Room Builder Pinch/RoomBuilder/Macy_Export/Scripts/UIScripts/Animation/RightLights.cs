using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightLights : MonoBehaviour {
	
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
		rightUIParameters.position = new Vector3 (-2.8f, -0.15f, -8.8f);
		rightUIParameters.rotation = new Vector3 (0, 244f, 0);
		clickObject.add ("RightUI", rightUIParameters);
		
		// LeftUI
		ObjectParameters leftUIParameters = new ObjectParameters ();
		leftUIParameters.position = new Vector3 (30f, 0, -30f);
		leftUIParameters.alpha = 0;
		clickObject.add ("LeftUI", leftUIParameters);
		
		// Featured Article
		ObjectParameters featuredParameters = new ObjectParameters ();
		featuredParameters.position = new Vector3 (0, 0, 40);
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