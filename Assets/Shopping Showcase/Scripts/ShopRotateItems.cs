using UnityEngine;
using System.Collections;

public class ShopRotateItems : VRObject {

	bool canControlRotate = false;
	Vector3 initMousePos;
	private float  startAngle = 0;
	private float rotX, rotY;
	private Vector3 origRot;
	Vector3 lastPos;

	public GameObject camPos;

	public float xAngle;

	Cursor curCursor;
	public bool mUpdate = false;

	// Use this for initialization
	void Start () {
		origRot = this.transform.eulerAngles;
		rotX = origRot.x;
		rotY = origRot.y;
	}

	public override void start (ref Cursor c) {

		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		mUpdate = true;
		curCursor = c;
		canControlRotate = true;
	}

	public override void end() {

		canControlRotate = false;
		mUpdate = false;
	}

	
	// Update is called once per frame
	void Update () {
		

		if (canControlRotate) {

			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);

			float deltaX = initMousePos.x - curCursorPos.x;
			//float deltaY = initMousePos.y - Input.mousePosition.y;
			//rotX -= deltaY * Time.deltaTime  * 10 * -1;
			rotY += deltaX * Time.deltaTime  * 100 ;
			transform.eulerAngles = new Vector3 (xAngle, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);


		}

	}
}
