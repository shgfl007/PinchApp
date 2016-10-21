using UnityEngine;
using System.Collections;

public class GoToBunch : ButtonBehaviour {

	Cursor c;
	bool cOn = false;



	// Update is called once per frame
	void Update () {

		if (cOn && DataVizUIManage.instance.isOn == false) {
			changeCam ();
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

	public void changeCam() {
	
		PointManager.instance.changeToBunch ();
		DataChangeCamPos.instance.goToBunch();
	}



}

