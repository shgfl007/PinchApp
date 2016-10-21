using UnityEngine;
using System.Collections;

public class OrbitInEnvironment : VRObject {

	bool canControlRotate = false;
	Vector3 initMousePos;
	private float  startAngle = 0;
	private float rotX, rotY;
	private Vector3 origRot;
	Vector3 lastPos;
	private Transform ec;
	public float xAngle;
	Vector3 previousPos;
	bool didNotMove = false;
	Cursor curCursor;

	// Use this for initialization
	void Start () {
		origRot = this.transform.eulerAngles;
		rotX = origRot.x;
		rotY = origRot.y;
		ec = GameObject.Find ("CamLocks").transform;
		print (ec);
	}

	public override void start (ref Cursor c) {

		rotX = ec.transform.eulerAngles.x;
		rotY = ec.transform.eulerAngles.y;

		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		curCursor = c;
//		print ("Parth is tsting outside the if statement");

		if (c == RightCursor.instance) {
			DummyCam.instance.followCamera = false;
			canControlRotate = true;
//			print ("Parth is tsting");

		}
	}

	public override void end() {
		canControlRotate = false;
		DummyCam.instance.followCamera = true;
	}


	// Update is called once per frame
	void Update () {

		if(LeftCursor.instance.clicked && RightCursor.instance.clicked){
			canControlRotate = false;
		}


		if (canControlRotate) {

			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);
			float deltaX = initMousePos.x - curCursorPos.x;
			float deltaY = initMousePos.y - curCursorPos.y;
			rotX -= deltaY * Time.deltaTime  * 20 * -1;
			rotY += deltaX * Time.deltaTime  * 20 * -1 ;
			ec.transform.eulerAngles = new Vector3 (rotX, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);

		}
			



	}
}
