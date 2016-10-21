//Kitchen Sink
using UnityEngine;
using System.Collections;

public class DepthDemo : VRObject {

	public  bool canDepth = false; // can move into Depth
	public GameObject toMove; // object to Move
	Cursor cursor; // Cursor
	float lastPos, currentPos; //Cursor Positions

	// Update is called once per frame
	void Update () {
	
		if(canDepth){

			lastPos = currentPos;// last cursor position
	
			currentPos = cursor.depth; //current cursor position
			float scaleFactor = lastPos - currentPos; //difference
			scaleFactor *= 50; //scaling the number
			//Apply depth
			toMove.transform.localScale = new Vector3 (toMove.transform.localScale.x + scaleFactor, toMove.transform.localScale.y + scaleFactor, toMove.transform.localScale.z + scaleFactor);

		}

	}

	//When the cursor starts the click
	public override void start(ref Cursor c){
		cursor = c; //assign cursor
	}

	public override void end() {
		canDepth = !canDepth; //toggle depth collection
	}
}
