using UnityEngine;
using System.Collections;

public class DisablePanel : VRObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void start(ref Cursor cursorInput){}

	public override void end() {

		gameObject.transform.parent.gameObject.SetActive (false);

	}

}
