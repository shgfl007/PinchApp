using UnityEngine;
using System.Collections;

public class RoomResetButton : ButtonBehaviour {

	public override void end() {
		Cardboard.SDK.Recenter ();
		UIManage.instance.isDraw = false;
		RoomReset.toReset = true;
		UIManage.instance.turnOffUI ();

	}
}
