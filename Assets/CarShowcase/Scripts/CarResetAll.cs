using UnityEngine;
using System.Collections;

public class CarResetAll : VRObject {

	public GameObject backInfo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void end() {

		RotateSelf.instance.canRotate = true;
		RotateSelf.instance.inCar = false;
		CarChangeCamPos.instance.goToDefault ();
		CarChangeCamPos.instance.resetAllAnchors ();
		CarChangeCamPos.instance.enableCollider ();
		CarChangeCamPos.instance.outCarCanvas ();
		backInfo.SetActive (false);
	}

}
