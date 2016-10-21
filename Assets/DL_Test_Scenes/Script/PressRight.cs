using UnityEngine;
using System.Collections;

public class PressRight : ButtonBehaviour {

	public Layout_Changer lc;

	public override void end(){
		lc.pressRight ();
	}
}
