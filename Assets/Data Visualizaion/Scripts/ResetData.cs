using UnityEngine;
using System.Collections;

public class ResetData : ButtonBehaviour {

	Cursor c;
	bool cOn = false;



	// Update is called once per frame
	void Update () {

		if (cOn && DataVizUIManage.instance.isOn == false) {
			movePoints ();
			cOn = false;
		}


	}

	public override void enterHover(Cursor currentCursor)	{
		c = currentCursor;

		cOn = true;
	}

	public override void exitHover(Cursor c)	{

		if (cOn && DataVizUIManage.instance.isOn) {
			cOn = false;

		}

	}

	public void movePoints() {
		PointManager.instance.resetAll ();
		PointManager.instance.moveToNormal ();
		DataChangeCamPos.instance.goToNormal();

	}





}
