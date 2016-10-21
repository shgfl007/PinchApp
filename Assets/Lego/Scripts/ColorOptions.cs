using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorOptions : ButtonBehaviour{

	bool cOn = false;
	public Material mat;
	public Image colorDot, handIcon;
	public Color cl;

	public override void end() {
		UIManage.instance.curColor = mat;
		UIManage.instance.lineColor = cl;
		colorDot.color = cl;
		handIcon.color = cl;
		DragToMake.instance.updateColor ();
		UIManage.instance.turnOffUI ();
	}




}
