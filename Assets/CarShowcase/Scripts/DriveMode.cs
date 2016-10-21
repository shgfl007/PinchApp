using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DriveMode : ButtonBehaviour {

	bool cOn = false;
	public GameObject backInfo;
	public Sprite drive, back;
	public Image img;
	public GameObject mt, outMesh;

	bool turnOn = false;

	void Start() {
		//print ("It gets here as well");
	//	bb = GetComponent<ButtonBehaviour> ();
		//img = GetComponent<Image> ();
		mt.SetActive (false);
		base.Start ();

	}

	public override void end() {

		if (RotateSelf.instance.inCar) {
			RotateSelf.instance.canRotate = true;
			RotateSelf.instance.inCar = false;
			CarChangeCamPos.instance.goToDefault ();
			CarChangeCamPos.instance.resetAllAnchors ();
			CarChangeCamPos.instance.enableCollider ();
			backInfo.SetActive (false);
			img.sprite = drive;
			mt.SetActive (false);
			outMesh.SetActive (true);


			cOn = false;

		} else {

			RotateSelf.instance.stopRotation ();
			RotateSelf.instance.inCar = true;
			CarChangeCamPos.instance.goToBunch ();
			CarChangeCamPos.instance.resetAllAnchors ();
			CarChangeCamPos.instance.disableCollider ();
			Invoke ("ToggleTexture", 2.25f);
			cOn = false;
			img.sprite = back;
		}
		CarUIManage.instance.turnOffUI ();

	}
	public void ToggleTexture() {
		mt.SetActive (true);
		outMesh.SetActive (false);
	}



}
