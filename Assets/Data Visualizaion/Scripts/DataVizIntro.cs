using UnityEngine;
using System.Collections;

public class DataVizIntro : VRObject {

	public GameObject UIToHide;
	public GameObject ec;
	public ManualHeadRot mh;

	public GoToNormal ox;
	// Use this for initialization
	void Awake () {
		UIToHide.SetActive (false);
		ec.SetActive (false);
		mh.enabled = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
	public override void start(ref Cursor c) {
		
	}

	public override void end() {


		FadeBlack.instance.fadeInBetween = true;
		Invoke ("turnOn", 1f);
		this.gameObject.transform.parent.parent.gameObject.SetActive (false);

	}

	void turnOn(){
	
		UIToHide.SetActive (true);
		ec.SetActive (true);
		mh.enabled = true;
		ox.movePoints ();
		DataChangeCamPos.instance.canOrbit = true;
	}



}
