using UnityEngine;
using System.Collections;

public class PressOK : ButtonBehaviour {

	public Layout_Changer lc;

	public override void end(){
		lc.panelsFadeOut ();
	}
}
