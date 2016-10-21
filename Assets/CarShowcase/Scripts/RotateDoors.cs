using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateDoors : VRObject {


	public Transform open, close;
	public bool canRotate = false;
	public bool isOpen = true;
	private Quaternion currentTarget;
	bool allowCheck = true;


	public override void end () {

		canRotate = true;

	}

	// Update is called once per frame
	void Update () {


		if (isOpen && RotateSelf.instance.inCar && allowCheck) {
		
			canRotate = true;
			allowCheck = false;
		}


		if(canRotate) {

			Quaternion fromRot = transform.rotation;

			if (isOpen) {
				currentTarget = close.rotation;

			} else {

				currentTarget = open.rotation;
			}

			if (Quaternion.Angle(fromRot, currentTarget) > 0.5f ) {
				fromRot = Quaternion.Lerp (fromRot, currentTarget, Time.deltaTime * 2f);
				transform.rotation = fromRot;
			} else {
				canRotate = false;
				isOpen = !isOpen;
				allowCheck = true;
				//print ("stopped moving");
			}

		}


	
	}
}
