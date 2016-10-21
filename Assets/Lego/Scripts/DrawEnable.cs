using UnityEngine;
using System.Collections;

public class DrawEnable : ButtonBehaviour {

	public int assignedInt;


	public override void end ()
	{
		UIManage.instance.isDraw = true;
		DragToMake.instance.holderToActivate = assignedInt;
		DragToMake.instance.toggleDrawingImage ();
		UIManage.instance.turnOffUI ();

	}





}
