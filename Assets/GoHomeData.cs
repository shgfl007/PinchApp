using UnityEngine;
using System.Collections;

public class GoHomeData : ButtonBehaviour {



	bool cOn = false;
	Cursor c;

	// Use this for initialization
	void Start () {
		base.Start ();
	}

	void Update () {

		if (cOn && DataVizUIManage.instance.isOn == false) {
			//Go Home
			GoToxScene.instance.sceneIndex = 5;
			GoToxScene.instance.GoToScene ();
			//	UIManage.instance.isDraw = false;
			cOn = false;
		}
	}

	public override void start (ref Cursor currentCurosr){
		c = currentCurosr;


	}

	public override void enterHover(Cursor currentCursor)	{
		c = currentCursor;

		if(currentCursor == LeftCursor.instance){
			cOn = true;
		}
		base.enterHover (currentCursor);
	}

	public override void exitHover(Cursor c)	{

		if (cOn && DataVizUIManage.instance.isOn && c== LeftCursor.instance) {
			cOn = false;

			base.exitHover (c);
		}


	}
}
