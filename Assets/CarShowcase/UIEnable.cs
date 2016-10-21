using UnityEngine;
using System.Collections;

public class UIEnable: VRObject {


	bool checkForUI = false;

	float timerToCheck = 0.5f;

	float timeLeft = 0.5f;
	Cursor curCursor;
	public bool mUpdate = false;

	// Use this for initialization
	void Start () {

	}



	public override void start (ref Cursor c) {

		mUpdate = true;
		curCursor = c;
	 if (c== LeftCursor.instance) {
			if (CarUIManage.instance.isOn == false) {
				checkForUI = true;
			} else {
				checkForUI = false;
			}

		}
	}

	public override void end() {
		mUpdate = false;
		checkForUI = false;
	}


	// Update is called once per frame
	void Update () {

		if(LeftCursor.instance.clicked && RightCursor.instance.clicked){
			checkForUI = false;
			CarUIManage.instance.turnOffUI();

		}



		if(checkForUI) {

			timeLeft -= Time.deltaTime;
			CarUIManage.instance.RingActivation (curCursor.transform.localPosition, true);

			if (timeLeft <= timerToCheck) {
					checkForUI = false;
					timeLeft = timerToCheck;
					CarUIManage.instance.turnOnUI (curCursor);

			
			}
		}



	}
}
