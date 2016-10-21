using UnityEngine;
using System.Collections;

public class Delete : ButtonBehaviour {


	public override void end() {
	
		BlockUI.instance.DestroyObject ();;

	}

}
