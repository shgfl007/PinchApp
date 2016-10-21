using UnityEngine;
using System.Collections;

public class GoHomeLego : ButtonBehaviour {



	public override void end() {
		GoToxScene.instance.sceneIndex = 5;
		GoToxScene.instance.GoToScene ();
		UIManage.instance.turnOffUI ();
	}


}
