using UnityEngine;
using System.Collections;

public class HoverReveal : VRObject {

	private GameObject hoverObject;

	// Use this for initialization
	void Start () {
		hoverObject = this.gameObject.transform.GetChild (0).gameObject;

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public override void enterHover(Cursor c){
		hoverObject.SetActive (true);
	}

	public override void exitHover( Cursor c){
		hoverObject.SetActive (false);
	}
	public override void end () {
	
		GoToxScene.instance.sceneIndex = 12;
		GoToxScene.instance.GoToScene ();
	
	}

}
