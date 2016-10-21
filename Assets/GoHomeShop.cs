using UnityEngine;
using System.Collections;

public class GoHomeShop : ButtonBehaviour {



	public override void end() {
		GoToxScene.instance.sceneIndex = 5;
		GoToxScene.instance.GoToScene ();
		ShowcaseUIManager.instance.turnOffUI ();
	}

}
