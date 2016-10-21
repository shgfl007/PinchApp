using UnityEngine;
using System.Collections;

public class UIRotate : ButtonBehaviour {


	public override void end() {

		BlockUI.instance.canRotate();
	}
}
