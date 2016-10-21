using UnityEngine;
using System.Collections;

public class ChangeCam : VRObject {

	public Transform posToChange;
	public GameObject cam;
	private Vector3 currentPos;
	private  Quaternion rotToChange;
	private Quaternion currentRot;

	private bool ifPressed = false;


	// Use this for initialization
	void Start () {
		rotToChange = posToChange.transform.rotation;

		currentPos = posToChange.transform.position;
		currentRot = rotToChange;
	}

	public override void end() {

	//	ifPressed = !ifPressed;


		//if (ifPressed) {
			RotateSelf.instance.stopRotation ();
			RotateSelf.instance.inCar = true;
			CarChangeCamPos.instance.goToBunch ();
			CarChangeCamPos.instance.resetAllAnchors ();
			CarChangeCamPos.instance.disableCollider ();
	//	ChangeCamPos.instance.inCarCanvas ();

	}

	// Update is called once per frame
	void Update () {



	}
}
