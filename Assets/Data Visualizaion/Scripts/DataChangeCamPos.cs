using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.VR;


public class DataChangeCamPos : MonoBehaviour {

	private static DataChangeCamPos mInstance;
	public static DataChangeCamPos instance {get{return mInstance;}}

	public List<GameObject> toTransform;
	public GameObject grid, tC;
	public Vector3 targetPos;
	public Quaternion targetRot;


	public GameObject c1, c2, c3, c4;

	bool canMove, canRotate = false;


	bool mOrbiting = false;

	Vector3 initCursorPos = new Vector3();
	bool initCursorPosSet = false;

	Cursor c;

	float orbitSpeed = 0.0025f;
	public bool canOrbit = false;


	// Use this for initialization
	void Start () {
		if (mInstance == null) {

			mInstance = this;
		}



	}
		
	// Update is called once per frame
	void Update () {


		if(canMove) {

			if (Vector3.Distance (transform.position, targetPos) > 0.2f) {
			
				transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * 4);
					
			} else {
				canMove = false;
			}

		}



		if(canRotate) {


			Quaternion fromRot = transform.rotation;
			if (Quaternion.Angle (fromRot, targetRot) > 2) {
				fromRot = Quaternion.Lerp (fromRot, targetRot, Time.deltaTime / 0.8f);
				transform.rotation = fromRot;
			} else {
			
				canRotate = false;
			}
			}

	}


	public void goToPoint (Vector3 obPos, Quaternion obRot) {
	
		targetPos = obPos;
		targetPos.x += 2;
		targetPos.z += 2;
		targetRot = transform.rotation;
		canMove = true;
		canRotate = true;
		//ManualHeadRot.instance.translateView ();
		
	}


	public void goToBunch() {

		targetPos = toTransform [1].transform.position;
		targetRot = toTransform [1].transform.rotation;
		canMove = true;
		canRotate = true;
		ManualHeadRot.instance.translateView ();
		grid.SetActive (false);

		c1.SetActive (true);
		c2.SetActive (true);
		c3.SetActive (true);
		c4.SetActive (true);
		tC.SetActive (false);

		//canRotate
	}

	public void goToNormal() {

		targetPos = toTransform [0].transform.position;
		targetRot = toTransform [0].transform.rotation;
		canMove = true;
		canRotate = true;
		ManualHeadRot.instance.translateView ();
		grid.SetActive (true);
		c1.SetActive (false);
		c2.SetActive (false);
		c3.SetActive (false);
		c4.SetActive (false);
		tC.SetActive (false);


	}


	public void goToTimeLine() {

		targetPos = toTransform [2].transform.position;
		targetRot = toTransform [2].transform.rotation;
		canMove = true;
		canRotate = true;
		ManualHeadRot.instance.translateView ();
		grid.SetActive (false);
		c1.SetActive (false);
		c2.SetActive (false);
		c3.SetActive (false);
		c4.SetActive (false);
		tC.SetActive (true);

	}




}
