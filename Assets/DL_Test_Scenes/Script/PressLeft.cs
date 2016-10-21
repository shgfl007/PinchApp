using UnityEngine;
using System.Collections;

public class PressLeft : ButtonBehaviour {

	public Layout_Changer lc;

	public override void end(){
		lc.pressLeft ();
	}
}
