using UnityEngine;
using System.Collections;

public class OrbitData : VRObject {

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

	Cursor curCursor;
	public bool mUpdate = false;

	// Use this for initialization
	void Start () {
		origRot = this.transform.eulerAngles;
		rotX = origRot.x;
		rotY = origRot.y;
		ec = GameObject.Find ("CamLocks").transform;
	}

	public override void start (ref Cursor c) {


		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		rotX = ec.transform.eulerAngles.x;

		rotY = ec.transform.eulerAngles.y;
		mUpdate = true;
		curCursor = c;
		if (c == RightCursor.instance) {

			canControlRotate = true;
		} else if (c== LeftCursor.instance) {

			checkForUI = true;
		}
	}

	public override void end() {

		canControlRotate = false;
		mUpdate = false;
		checkForUI = false;
	}






	// Update is called once per frame
	void Update () {

		if(LeftCursor.instance.clicked && RightCursor.instance.clicked) {
			canControlRotate = false;
			checkForUI = false;
			DataVizUIManage.instance.turnOffUI();

		}


		if (canControlRotate) {

			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);
			float deltaX = initMousePos.x - curCursorPos.x;
			float deltaY = initMousePos.y -  curCursorPos.y;
			rotX -= deltaY * Time.deltaTime  * 10 * -1;
			rotY += deltaX * Time.deltaTime * 10 ;
			ec.transform.eulerAngles = new Vector3 (rotX, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);

		}


		if(checkForUI) {

			timeLeft -= Time.deltaTime;

			DataVizUIManage.instance.RingActivation (curCursor.transform.localPosition, true);

			if (timeLeft <= timerToCheck) {

				checkForUI = false;
				timeLeft = timerToCheck;
			DataVizUIManage.instance.turnOnUI (curCursor);

			}

		}



	}
}
