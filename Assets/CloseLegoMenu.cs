using UnityEngine;
using System.Collections;

public class CloseLegoMenu : VRObject {

	public override void end() {

		UIManage.instance.turnOffUI ();

	}
}
