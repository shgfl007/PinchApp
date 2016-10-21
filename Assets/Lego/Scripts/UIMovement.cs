using UnityEngine;
using System.Collections;

public class UIMovement : ButtonBehaviour {

	Cursor c;

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
	
	}


	public override void start(ref Cursor curC) {
		c = curC;
		base.start (ref curC);
	}

	public override void end() {

		BlockUI.instance.canMove (c);
	}

}
