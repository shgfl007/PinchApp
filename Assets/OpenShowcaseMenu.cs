using UnityEngine;
using System.Collections;

public class OpenShowcaseMenu : VRObject {
	bool checkForUI = false;

	float timerToCheck = 0.5f;

	float timeLeft = 0.5f;
	// Use this for initialization
	void Start () {
	
	}

	public override void start (ref Cursor c) {

 		if (c== LeftCursor.instance ) {
			if (!ShowcaseUIManager.instance.isOn) {
				checkForUI = true;
			} else {
				checkForUI = false;
			}
		}

	}

	public override void end() {
		checkForUI = false;
	}

	// Update is called once per frame
	void Update () {

		if(LeftCursor.instance.clicked && RightCursor.instance.clicked){
			checkForUI = false;
			ShowcaseUIManager.instance.turnOffUI();

		}

		if(checkForUI) {

			timeLeft -= Time.deltaTime;
			ShowcaseUIManager.instance.RingActivation (LeftCursor.instance.transform.localPosition, true);

			if (timeLeft <= timerToCheck) {

				checkForUI = false;
				timeLeft = timerToCheck;
				ShowcaseUIManager.instance.turnOnUI (LeftCursor.instance);

			}


		}
	}
}
