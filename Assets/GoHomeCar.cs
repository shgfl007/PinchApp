using UnityEngine;
using System.Collections;

public class GoHomeCar : ButtonBehaviour {



	bool cOn = false;
	Cursor c;

	// Use this for initialization
	void Start () {
		base.Start ();
	}

	public override void end () {

			GoToxScene.instance.sceneIndex = 5;
			GoToxScene.instance.GoToScene ();
		CarUIManage.instance.turnOffUI ();
	}

}
