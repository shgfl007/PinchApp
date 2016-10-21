using UnityEngine;
using System.Collections;

public class GoToHomeKS : ButtonBehaviour {


	public override void end(){

		GoToxScene.instance.GoToScene ();
	}
}
