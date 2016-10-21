using UnityEngine;
using System.Collections;

public class RotateSelf : VRObject {

	private static RotateSelf mInstance;
	public static RotateSelf instance { get { return mInstance; } }


	public bool canRotate = true;
	bool canControlRotate = false;
	Vector3 initMousePos;
	private float  startAngle = 0;
	private float rotX, rotY;
	private Vector3 origRot;
	private float rotSpeed = 0.5f;
	Vector3 lastPos;
	public bool inCar = false;

	Cursor curCursor;
	public bool mUpdate = false;
	// Use this for initialization
	void Start () {

		if (mInstance == null) {
			mInstance = this;

		}

		origRot = this.transform.eulerAngles;
		rotX = origRot.x;
		rotY = origRot.y;
	
	}

	public override void start (ref Cursor c) {
	
		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		mUpdate = true;
		//canRotate = false;
		canControlRotate = true;
		curCursor = c;
	}

	public override void end() {
	//	if (!inCar) {
	//		canRotate = true;
	//	}
		canControlRotate = false;
		mUpdate = false;
	}

	public void stopRotation() {

	//	inCar = !inCar;
		canRotate = false;
		//transform.eulerAngles = new Vector3 (0,0,0);
	}


	public void startRotation () {

		canRotate = true;

	}

	// Update is called once per frame
	void Update () {





		if (canControlRotate && !inCar) {

			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);

				float deltaX = initMousePos.x - curCursorPos.x;
			//float deltaY = initMousePos.y - Input.mousePosition.y;
			//rotX -= deltaY * Time.deltaTime  * 10 * -1;
			rotY += deltaX * Time.deltaTime  * 3 ;
			transform.eulerAngles = new Vector3 (0, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);


		}


		if(canRotate) {

			transform.Rotate (Vector3.up * (2 * Time.deltaTime));

		}

	}
}
