using UnityEngine;
using System.Collections;

public class RemoveSelf : VRObject {

	public GameObject globalMenu;
	public ManualHeadRot mh;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void start (ref Cursor c){
	}
	public override void end() {

	//	print ("Canvas : it gets here");
		globalMenu.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
//		mh.enabled = true;
	}
}
