using UnityEngine;
using System.Collections;

public class RotateItems : VRObject {

	bool canControlRotate = false;
	Vector3 initMousePos;
	private float  startAngle = 0;
	private float rotX, rotY;
	private Vector3 origRot;
	Vector3 lastPos;
	public Transform ec;
	public GameObject camPos;

	bool checkForUI = false;

	float timerToCheck = 0.5f;

	float timeLeft = 0.5f;

	public float xAngle;

	Vector3 previousPos;
	bool didNotMove = false;

	Cursor curCursor;
	public bool mUpdate = false;

	// Use this for initialization
	void Start () {
		origRot = this.transform.eulerAngles;
		rotX = origRot.x;
		rotY = origRot.y;
		ec = GameObject.Find ("CamLocks").transform;
	}



	Vector3 WorldSpaceCords () {

		//this was to keep units same for draw + menu detection
		Vector3 cursorCoord = LeftCursor.instance.transform.position;
		Vector3 headCoord = Camera.main.transform.position;
		Vector3 vec = cursorCoord - headCoord;
		float radius = 80;
		if (radius > headCoord.y)
			radius = headCoord.y;
		Vector3 finalCoord = headCoord + vec.normalized * radius;
		return finalCoord;
	}


	public override void start (ref Cursor c) {

		BlockUI.instance.TurnOff ();


		rotX = ec.transform.eulerAngles.x;
		rotY = ec.transform.eulerAngles.y;


		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		mUpdate = true;
		curCursor = c;
		if (c == RightCursor.instance && !UIManage.instance.isDraw ) {
			canControlRotate = true;
		} else if (c== LeftCursor.instance && !UIManage.instance.isDraw ) {
			if (!UIManage.instance.isOn) {
				checkForUI = true;
			} else {
				checkForUI = false;
			}
		}
	}

	public override void end() {
		canControlRotate = false;
		mUpdate = false;
		checkForUI = false;
	}


	// Update is called once per frame
	void Update () {

		if(LeftCursor.instance.clicked && RightCursor.instance.clicked){
			canControlRotate = false;
			checkForUI = false;
			UIManage.instance.turnOffUI();

		}


		if (canControlRotate) {

			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);

			float deltaX = initMousePos.x - curCursorPos.x;
			float deltaY = initMousePos.y - curCursorPos.y;
			rotX -= deltaY * Time.deltaTime  * 2 * -1;
			rotY += deltaX * Time.deltaTime  * 2 * -1 ;
			ec.transform.eulerAngles = new Vector3 (rotX, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);

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
