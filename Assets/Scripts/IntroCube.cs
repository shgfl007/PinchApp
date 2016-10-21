using UnityEngine;
using System.Collections;
using Vectrosity;
using System.Collections.Generic;

public class IntroCube : VRObject {


	public bool canControlRotate = false;
	public bool canControlScale = false;
	public bool canControlDrag = false;

	public bool featureComplete = false;


	public Transform origStats, origCam;
	public Transform dropStat;

	private float  startAngle = 0;
	private float rotX, rotY;
	Vector3 initMousePos;
	Cursor curCursor;
	public int index;
	bool bothClicked = false;
	Vector3 originalScale;

	public GameObject dropTrigger;

	private VectorLine line;
	public static Vector3 newPoint;
	Vector3 lastPos;
	Vector3 startPoint;
	bool drawUpdate = true;
	bool curClicked = false;
	GameObject parenter;
	float distance;

	//public GameObject dropTrigger;

	public bool canControlOrbit = false;
	public bool canControlMenu = false;
	public GameObject camLocks;

	public GameObject nextButton;


	// Use this for initialization
	void Start () {

		dropTrigger.SetActive (false);
		rotX = transform.rotation.eulerAngles.x;
		rotY = transform.rotation.eulerAngles.y;
	}

	// Update is called once per frame
	void Update () {
		if (featureComplete) {
			nextButton.SetActive (true);
			featureComplete = false;
		}

		if (index == 0) {
			RayCast.instance.StopZooming = true;
			rotateCube();

		}

		if (index == 1) {
			scaleCube ();
			RayCast.instance.StopZooming = true;

			dropTrigger.SetActive (false);

		}


		if (index == 2) {
			RayCast.instance.StopZooming = true;

			dragDrop ();
			dropTrigger.SetActive (true);
		}

		if (index == 3) {
			RayCast.instance.StopZooming = true;

			draw ();
			dropTrigger.SetActive (false);

		}

		if (index == 4) {
			RayCast.instance.StopZooming = false;

			if (Vector3.Distance (camLocks.transform.position, origCam.transform.position) > 0.5f) {
				featureComplete = true;
			}
		}

		if (index == 5) {
			RayCast.instance.StopZooming = true;
			orbit ();

		}

		if (index == 6) {

			menu ();
		}


	}
	float timerToCheck = 0.5f;

	float timeLeft = 0.5f;

	bool preventCheck = false;

	public void menu() {


		if (LeftCursor.instance.clicked && !preventCheck && IntroMenu.instance.isOn == false) {

			canControlMenu = true;
		} else {
			canControlMenu = false;
		}

		if(canControlMenu) {

			timeLeft -= Time.deltaTime;

			IntroMenu.instance.RingActivation (LeftCursor.instance.transform.localPosition, true);

			if (timeLeft <= timerToCheck) {

				featureComplete = true; // for menu!

				canControlMenu = false;
				timeLeft = timerToCheck;
				IntroMenu.instance.turnOnUI (LeftCursor.instance);

			}
		} 



	}


	bool saveOrbitData = true;

	public void orbit() {

		if (RightCursor.instance.clicked) {
			canControlOrbit = true;
			if (saveOrbitData) {
				rotX = camLocks.transform.rotation.eulerAngles.x;
				rotY = camLocks.transform.rotation.eulerAngles.y;
				initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);

				saveOrbitData = false;
			}

		} else {
			canControlOrbit = false;
			saveOrbitData = true;
		}

		if (canControlOrbit) {

			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);
			float deltaX = initMousePos.x - curCursorPos.x;
			float deltaY = initMousePos.y - curCursorPos.y;
			rotX -= deltaY * Time.deltaTime  * 2 * -1 ;
			rotY += deltaX * Time.deltaTime  * 2 ;
			camLocks.transform.eulerAngles = new Vector3 (rotX, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);


		}

		float angle = Quaternion.Angle (camLocks.transform.rotation, origCam.transform.rotation);

		print (angle);

		if(angle > 10) {
			featureComplete = true; // rotate
		}


	}

	public override void start (ref Cursor c) {

		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		if (index == 0) {
			canControlRotate = true;
			rotX = transform.rotation.eulerAngles.x;
			rotY = transform.rotation.eulerAngles.y;
		}

		if (index == 2) {
			canControlDrag = true;

		}

		if (index == 6) {
			if (IntroMenu.instance.isOn) {
				preventCheck = true;
			} else {
				preventCheck = false;
			}
		}


		curCursor = c;
	}

	public override void end() {
		canControlRotate = false;
		canControlScale = false;
		canControlDrag = false;
		canControlOrbit = false;
		canControlMenu = false;


		if (preventCheck) {
			IntroMenu.instance.turnOffUI ();
			preventCheck = false;
		}




	}


	public void dragDrop() {

		if (canControlDrag) {
			Vector3 worldToScreen = Camera.main.WorldToScreenPoint(curCursor.transform.position);
			Vector3 cubeScreen = Camera.main.WorldToScreenPoint(transform.position);
			Vector3 cursorToCube = new Vector3(worldToScreen.x, worldToScreen.y, cubeScreen.z);
			Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(cursorToCube); //fetch screen to world coordinates
			//screenToWorld.y = transform.position.y;
			screenToWorld.z = transform.position.z;
			transform.position = screenToWorld;

		}

		//	print (Vector3.Distance (dropTrigger.transform.position, transform.position));

		if (Vector3.Distance (dropTrigger.transform.position, transform.position) < 1) {
			//print ("it gets here");
			featureComplete = true;
		}

	}

	bool parentLine = false;
	public GameObject drawParent;


	//this is to clear draw line this function gets called from intro manager <3
	public void deleteDrawLines() {

		foreach(Transform child in drawParent.transform){
			Destroy (child.gameObject);
		}

	}

	public void draw() {

		//Initiate Draw
		if (RightCursor.instance.clicked || LeftCursor.instance.clicked) {

			curClicked = true;
			if (RightCursor.instance.clicked) {
				curCursor = RightCursor.instance;

			} else {

				curCursor = LeftCursor.instance;
			}

			if (drawUpdate) {

				line = new VectorLine ("DrawnLine3D", new List<Vector3> (), 1, LineType.Continuous, Joins.Fill);//make a new line
				line.points3.Clear (); //clear old array
				line.lineWidth = 4; //set line width
				line.endPointsUpdate = 1; 
				startPoint = WorldSpaceCords ();
				drawUpdate = false;
				parentLine = true;
			}

		} else {
			if (parentLine) {

				parenter = GameObject.Find ("DrawnLine3D");
				if (parenter != null) {
					parenter.gameObject.transform.parent = drawParent.gameObject.transform;
					parenter.gameObject.name = "DrawnLine";
				}
				parentLine = false;
				featureComplete = true;
			}

			curClicked = false;
			drawUpdate = true;
		}


		//Draw
		if (curClicked) {

			Vector3 tempMousePos = WorldSpaceCords ();
			distance = Vector3.Distance (tempMousePos, lastPos);
			if (distance > 0.005f) {

				line.points3.Add (tempMousePos);
				line.Draw3D ();
			}
			lastPos = tempMousePos;

		} 


	}

	// This function maps cursorpos to proper worldspace coords

	Vector3 WorldSpaceCords ()
	{
		Vector3 cursorCoord = curCursor.transform.position;
		Vector3 headCoord = Camera.main.transform.position;
		Vector3 vec = cursorCoord - headCoord;
		float radius = transform.position.z;
		Vector3 finalCoord = headCoord + vec.normalized * radius;
		finalCoord.x *= -1;
		finalCoord.y *= -1;
		finalCoord.y -= 2.6f;
		finalCoord.z *= -1* curCursor.depth;;
		return finalCoord;
	}


	// for rotate function

	public void rotateCube() {
		if (canControlRotate) {


			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (curCursor.transform.position);
			float deltaX = initMousePos.x - curCursorPos.x;
			float deltaY = initMousePos.y - curCursorPos.y;
			rotX -= deltaY * Time.deltaTime  * 10 * -1;
			rotY += deltaX * Time.deltaTime  * 10 ;
			transform.eulerAngles = new Vector3 (rotX, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (curCursor.transform.position);

			float angle = Quaternion.Angle (transform.rotation, origStats.rotation);

			if(angle > 5) {
				featureComplete = true; // rotate
			}


		}
	}

	public float Remap(float value, float from1, float to1, float from2, float to2) {

		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	// for scale function
	public void scaleCube() {

		if (!LeftCursor.instance.clicked || !RightCursor.instance.clicked){
			bothClicked = false; //if one cursor released stop scaling
			originalScale = transform.localScale;
			canControlScale = false;
		}

		if (bothClicked && canControlScale) //do this when both cursor clicked
		{

			float curDistance = Vector3.Distance (RightCursor.instance.transform.position, LeftCursor.instance.transform.position);

			float scaleValue = Remap (curDistance, 0.2f, 0.7f, 0, 2);

			float scaleRate = (1 + scaleValue);

			if (scaleRate > 2) {

				scaleRate = 2;
			}


			if (scaleRate < 1) {
				scaleRate = 1;
			}
			transform.localScale = new Vector3 (scaleRate, scaleRate, scaleRate);
			//	transform.localScale = newScale; //assign new scale
			RayCast.instance.StopZooming = true;

		}

		float scaleDistance = transform.localScale.x - origStats.transform.localScale.x;

		if (scaleDistance > 0.2f) {
			featureComplete = true;
		}



	}

	//if both cursor clicked the cube
	public override void dualCursorUpdate(ref Cursor left, ref Cursor right)
	{
		if(index == 1){
			bothClicked = true; //when both cursors get clicked set this to true
			canControlScale = true;
		}
		base.dualCursorUpdate(ref left, ref right);
	}





	public void resetCube () {

		StopAllCoroutines ();
		StartCoroutine (resetCubeParams ());
		StartCoroutine (resetCamParams ());

	}

	IEnumerator resetCamParams() {
		float elapsedTime = 0;
		while (elapsedTime < 2f) {


			camLocks.transform.position = Vector3.Lerp (camLocks.transform.position, origCam.position, (elapsedTime / 0.5f));
			camLocks.transform.rotation = Quaternion.Lerp (camLocks.transform.rotation, origCam.rotation, (elapsedTime / 0.5f));

			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

	}
	IEnumerator resetCubeParams() {

		float elapsedTime = 0;
		while (elapsedTime < 0.5f) {

			if (index == 2) {
				transform.position = Vector3.Lerp (transform.position, dropStat.position, (elapsedTime / 0.5f));
				transform.rotation = Quaternion.Lerp (transform.rotation, dropStat.rotation, (elapsedTime / 0.5f));

			} else {
				transform.position = Vector3.Lerp (transform.position, origStats.position, (elapsedTime / 0.5f));
				transform.rotation = Quaternion.Lerp (transform.rotation, origStats.rotation, (elapsedTime / 0.5f));

			}
			transform.localScale = Vector3.Lerp (transform.localScale, origStats.localScale, (elapsedTime / 0.5f));
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

	}
}
