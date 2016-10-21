using UnityEngine;
using System.Collections;

public class CloseIntroMenu : VRObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void end() {

		IntroMenu.instance.turnOffUI ();

	}
}
