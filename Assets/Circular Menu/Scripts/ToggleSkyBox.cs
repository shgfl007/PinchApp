using UnityEngine;
using System.Collections;

public class ToggleSkyBox : ButtonBehaviour {

	public Material matA;

	public override void end() {
		UIManage.instance.isDraw = false;
		RenderSettings.skybox = matA;
		UIManage.instance.turnOffUI ();

	}






}
