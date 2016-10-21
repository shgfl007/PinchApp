using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.VR;


public class ShopChangeCamPos : MonoBehaviour {

	private static ShopChangeCamPos mInstance;
	public static ShopChangeCamPos instance {get{return mInstance;}}

	public List<GameObject> toTransform;
	public Vector3 targetPos;
	public Quaternion targetRot;



	public bool canMove, canRotate = false;
	private GameObject go;

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

	//	Invoke ("goToBunch", 10);



	}



	// Update is called once per frame
	void Update () {

		if(canMove) {

			if (Vector3.Distance (transform.position, targetPos) > 0.2f) {
			
				transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * 1.4f);
					
			} else {
				canMove = false;

				if(go != null) {

					go.gameObject.GetComponent<ItemInfo> ().playAnimations ();
				}
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





	public void goToBunch() {

		targetPos = toTransform [1].transform.position;
	//	targetRot = toTransform [1].transform.rotation;
		canMove = true;
		//canRotate = true;
	//	ManualHeadRot.instance.translateView ();
	//	StartCoroutine (JumpPosition());


		//canRotate
	}
	IEnumerator JumpPosition() {
		yield return new WaitForSeconds (3.5f);
		FadeBlack.instance.fadeInBetween = true;
		yield return new WaitForSeconds (0.6f);
		transform.position = toTransform [1].transform.position;

	}

	public void goToTee(Vector3 pos, Quaternion rot, GameObject gThis) {

		//pos.x += 4;
		//pos.y += 4;
		//pos.z += 4;
		targetPos = pos;
	//	targetRot = rot;
		canMove = true;
		//canRotate = true;
		//ManualHeadRot.instance.translateView ();
		go = gThis;

		//canRotate
	}


	public void goToSelectItem(Vector3 pos, Quaternion rot) {

		//pos.x += 4;
		//pos.y += 4;
		//pos.z += 4;
		targetPos = pos;
		//targetRot = rot;
		canMove = true;
//		print ("it moves the camera some what :D");

	}



}
