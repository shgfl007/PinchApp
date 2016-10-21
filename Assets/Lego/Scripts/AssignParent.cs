using UnityEngine;
using System.Collections;

public class AssignParent : VRObject {

	public BlockManip bm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public override void end ()
	{
		if (CubeRaycast.instance.isPlacing == false && bm.canBeEdited) {
			bm.summonUI ();
		}
	}
}
