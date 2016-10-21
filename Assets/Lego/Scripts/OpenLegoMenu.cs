using UnityEngine;
using System.Collections;

public class OpenLegoMenu : VRObject {


	bool checkForUI = false;

	float timerToCheck = 0.5f;

	float timeLeft = 0.5f;

 

	Cursor curCursor;
	public bool mUpdate = false;


	public override void start (ref Cursor c) {

		BlockUI.instance.TurnOff ();
		curCursor = c;
	if (c== LeftCursor.instance && !UIManage.instance.isDraw ) {
			if (!UIManage.instance.isOn) {
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
			UIManage.instance.turnOffUI();

		}




		if(checkForUI) {

			timeLeft -= Time.deltaTime;
			UIManage.instance.RingActivation (LeftCursor.instance.transform.localPosition, true);

			if (timeLeft <= timerToCheck) {
				checkForUI = false;
				timeLeft = timerToCheck;
				UIManage.instance.turnOnUI (LeftCursor.instance);

			}


		}



	}
}
