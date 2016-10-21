using UnityEngine;
using System.Collections;

public class SpawnBlock : ButtonBehaviour {


	public GameObject bBlock;
	public bool didCreate = true;
	GameObject block ;
	Cursor c;
	public int assignedInt;
	SpawnBlock sb;
	bool cOn = false;

	// Use this for initialization
	void Start () {
		sb = this.gameObject.GetComponent<SpawnBlock> ();
		base.Start ();
	}


	public override void start(ref Cursor cur) 
	{
		c = cur;
		base.start (ref cur);

	}


	public override void end ()
	{
		spawnObjects ();
		UIManage.instance.isDraw = false;
		UIManage.instance.turnOffUI ();

	}


	public void spawnObjects () {

		block = Instantiate (bBlock, Vector3.zero, Quaternion.identity) as GameObject;

		block.gameObject.GetComponent<BlockManip> ().changeColor (UIManage.instance.curColor);

		RightCursor.instance.GetComponent<CubeRaycast> ().placeObject(block.gameObject, c, sb);
		didCreate = false;
		DragToMake.instance.holderToActivate = assignedInt;
		DragToMake.instance.updateImage ();
	}


}
