using UnityEngine;
using System.Collections;

public class LegoRecenterCardboard : ButtonBehaviour {


	public override void end() {
		Cardboard.SDK.Recenter ();
		UIManage.instance.isDraw = false;
		UIManage.instance.turnOffUI ();
	}

}
