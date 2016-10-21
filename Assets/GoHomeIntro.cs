using UnityEngine;
using System.Collections;

public class GoHomeIntro : ButtonBehaviour {



	public override void end() {
		GoToxScene.instance.sceneIndex = 5;
		GoToxScene.instance.GoToScene ();
		IntroMenu.instance.turnOffUI ();
	
	}




}
