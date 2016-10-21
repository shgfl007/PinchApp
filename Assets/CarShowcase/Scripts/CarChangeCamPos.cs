using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.VR;


public class CarChangeCamPos : MonoBehaviour {

	private static CarChangeCamPos mInstance;
	public static CarChangeCamPos instance {get{return mInstance;}}

	public List<GameObject> toTransform;
	//public GameObject grid, tC;
	public Vector3 targetPos;
	public Quaternion targetRot;

	public bool canMove, canRotate = false;

	public GameObject  a3;

	bool mOrbiting = false;

	public BoxCollider[] bc;


	Vector3 initCursorPos = new Vector3();
	bool initCursorPosSet = false;


	public GameObject homeCanvas, resetCanvas;

	Cursor c;

	float orbitSpeed = 0.0025f;
	public bool canOrbit = false;
	public bool reachedDestination = false;

	// Use this for initialization
	void Start () {
		if (mInstance == null) {

			mInstance = this;
		}



	}


	void letsOrbit() {

		if (LeftCursor.instance.hold)
		{
		if (!initCursorPosSet)
			{
				initCursorPos = LeftCursor.instance.transform.localPosition;
				initCursorPosSet = true;
			}
			Vector3 relativePos = LeftCursor.instance.transform.localPosition - initCursorPos;

			//transform.RotateAround(transform.position, transform.right, relativePos.y * orbitSpeed);

			transform.RotateAround(transform.position, transform.up, relativePos.x * orbitSpeed);
		}
		else
			initCursorPosSet = false;



	
	}
	// Update is called once per frame
	void Update () {

		if(canOrbit){
		letsOrbit ();
		}

		if(canMove) {

			if (Vector3.Distance (transform.position, targetPos) > 0.2f) {
			
				transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * 2f);
					
				reachedDestination = false;

			} else {
				canMove = false;
				reachedDestination = true;
			//	print ("reached destination");
			}

		}
			
		if(canRotate) {


			Quaternion fromRot = transform.rotation;
			if (Quaternion.Angle (fromRot, targetRot) > 0.3f) {
				fromRot = Quaternion.Lerp (fromRot, targetRot, Time.deltaTime / 1.2f);
				transform.rotation = fromRot;
			} else {
			
				canRotate = false;
			}
			}

	} // end of update


	public void resetAllAnchors() {
	
		//a1.SetActive (true);
		//a2.SetActive (true);
		a3.SetActive (true);
	}


	public void inCarCanvas() {
	//	homeCanvas.SetActive (false);
	//	resetCanvas.SetActive (true);
	}

	public void outCarCanvas() {
//		homeCanvas.SetActive (true);
///		resetCanvas.SetActive (false);
	}

	public void disableCollider() {

		for (int i = 0; i < bc.Length; i++) {
		
			bc [i].enabled = false;
		}

	}

	public void enableCollider() {


		for (int i = 0; i < bc.Length; i++) {

			bc [i].enabled = true;
		}

	}


	public void goToBunch() {

		targetPos = toTransform [0].transform.position;
		targetRot = toTransform [0].transform.rotation;
		canMove = true;
		canRotate = true;
		ManualHeadRot.instance.translateView ();
		//canRotate
	}

	public void goToRear() {

		targetPos = toTransform [1].transform.position;
		targetRot = toTransform [1].transform.rotation;
		canMove = true;
	//	canRotate = true;
	//	ManualHeadRot.instance.translateView ();
		//canRotate
	}

	public void goToTop() {

		targetPos = toTransform [2].transform.position;
		targetRot = toTransform [2].transform.rotation;
		canMove = true;
	//	canRotate = true;
		//ManualHeadRot.instance.translateView ();
		//canRotate
	}

	public void goToLights() {

		targetPos = toTransform [3].transform.position;
		targetRot = toTransform [3].transform.rotation;
		canMove = true;
		//canRotate = true;
	///	ManualHeadRot.instance.translateView ();
		//canRotate
	}


	public void goToDefault () {
		targetPos = toTransform [4].transform.position;
		targetRot = toTransform [4].transform.rotation;
		canMove = true;
		//canRotate = true;
		//ManualHeadRot.instance.translateView ();

	}


}
