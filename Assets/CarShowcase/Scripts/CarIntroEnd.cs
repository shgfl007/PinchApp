using UnityEngine;
using System.Collections;

public class CarIntroEnd : VRObject {

	public GameObject  ground;

	public override void end() {
		transform.parent.gameObject.SetActive (false);
		ground.SetActive (true);
		RotateSelf.instance.canRotate = true;
		RotateSelf.instance.inCar = false;
		CarChangeCamPos.instance.goToDefault ();
		CarChangeCamPos.instance.resetAllAnchors ();
		CarChangeCamPos.instance.enableCollider ();
		CarChangeCamPos.instance.outCarCanvas ();
	//	enableAll ();
	//	FadeBlack.instance.resetColor ();
	//	FadeBlack.instance.fadeFromBlack = true;
	//	Invoke ("enableAll", 0.1f);
	}
	/*
	void enableAll() {

		ground.SetActive (true);
		RotateSelf.instance.canRotate = true;
		RotateSelf.instance.inCar = false;
		CarChangeCamPos.instance.goToDefault ();
		CarChangeCamPos.instance.resetAllAnchors ();
		CarChangeCamPos.instance.enableCollider ();
		CarChangeCamPos.instance.outCarCanvas ();
	}
*/

}
