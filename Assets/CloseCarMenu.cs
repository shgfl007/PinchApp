using UnityEngine;
using System.Collections;

public class CloseCarMenu : VRObject {

	public override void end() {

		CarUIManage.instance.turnOffUI ();

	}
}
