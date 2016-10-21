using UnityEngine;
using System.Collections;

public class BackCar : VRObject {

	public Transform xCamPos;
	public GameObject cMain;
	public GameObject infoOn;


	public override void end() {

		movePoints();


	}

	public void movePoints() {
		CarChangeCamPos.instance.goToRear();
		RotateSelf.instance.canRotate = false;
		this.gameObject.SetActive (false);
		infoOn.SetActive (true);
	}


}
