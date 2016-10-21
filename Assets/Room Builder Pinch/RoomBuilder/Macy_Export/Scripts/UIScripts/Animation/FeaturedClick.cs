using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FeaturedClick : MonoBehaviour {

	ThereAndBack clickObject;
	bool initDone = false;
	bool videoPlaying = false;
//	public MediaPlayerCtrl videoTexture;

	void Start() {
//		videoTexture = GameObject.Find ("VideoManager").GetComponent<MediaPlayerCtrl> ();
	}

	void Update() {
	}

	public void onClickGo() {
		if (!initDone) {
			init ();
		}
		if (!videoPlaying) {
//			videoTexture.Play ();
			videoPlaying = true;
		}
		else {
//			videoTexture.Pause();
			videoPlaying = false;
		}

		clickObject.go ();
	}

	void init() {
		clickObject = new ThereAndBack ();

		// Video
		ObjectParameters videoParameters = new ObjectParameters ();
		videoParameters.position = new Vector3 (0, 0.28f, -5f); // Set final parameters
		videoParameters.alpha = 1;
		videoParameters.rotation = new Vector3 (350, 180f, 0);
		clickObject.add ("Video", videoParameters);
		
		// RightUI
		ObjectParameters rightUIParameters = new ObjectParameters ();
		rightUIParameters.position = new Vector3 (-7f, -0.15f, 28.6f); // Set final parameters
		rightUIParameters.alpha = 0;
		clickObject.add ("RightUI", rightUIParameters);
		
		// LeftUI
		ObjectParameters leftUIParameters = new ObjectParameters ();
		leftUIParameters.position = new Vector3 (10.8f, 0, 23f); // Set final parameters
		leftUIParameters.alpha = 0;
		clickObject.add ("LeftUI", leftUIParameters);
		
		// Featured Article
		ObjectParameters featuredParameters = new ObjectParameters ();
		featuredParameters.position = new Vector3 (0, 0, 1); // Set final parameters
		featuredParameters.alpha = 0;
		clickObject.add ("Featured", featuredParameters);

		initDone = true;
	}
}