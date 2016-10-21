//Kitchen Sink
using UnityEngine;
using System.Collections;
using Vectrosity;
using System.Collections.Generic;

public class DrawFree : VRObject
{

	private VectorLine line;
	public static Vector3 newPoint;


	Vector3 lineOffset = new Vector3 (0.0f, 0.0f, 10.0f);

	bool allowDraw = false;
	Vector3 currentPos, lastPos;

	Vector3 startPoint;
	public GameObject ec;

	float distance = 0;
//	public float magnificationFactor = 2;
	 Cursor cur;
	bool mUpdate = true;
	bool curClicked = false;
	GameObject parenter;

	void Start ()
	{
		cur = LeftCursor.instance;

	}

	public override void start (ref Cursor c)
	{
		cur = c;
	//	if (cur == LeftCursor.instance) {
			curClicked = true;
	//	}

	}

	public override void end ()
	{
		curClicked = false;
		mUpdate = true;
		parenter = GameObject.Find ("DrawnLine3D");
		if (parenter != null) {
			parenter.gameObject.transform.parent = this.gameObject.transform;
			parenter.gameObject.name = "DrawnLine";
		}
	}


	// This function maps cursorpos to proper worldspace coords

	Vector3 WorldSpaceCords ()
	{
		Vector3 cursorCoord = cur.transform.position;
		Vector3 headCoord = Camera.main.transform.position;
		Vector3 vec = cursorCoord - headCoord;
		float radius = 20;
		Vector3 finalCoord = headCoord + vec.normalized * radius;
		//finalCoord.x *= -1;
		//finalCoord.y *= -1;
		//finalCoord.y -= 2.6f;
	//	finalCoord.z = transform.position.z;
		return finalCoord;
	}

	void Update ()
	{

		//Initiate Draw
		if (cur.clicked && mUpdate && curClicked) {
			mUpdate = false;
			allowDraw = true;
			line = new VectorLine ("DrawnLine3D", new List<Vector3> (), 1, LineType.Continuous, Joins.Fill);//make a new line
			line.points3.Clear (); //clear old array
			line.lineWidth = 4; //set line width
			line.endPointsUpdate = 1; 
			startPoint = WorldSpaceCords ();
		}

		//Draw
		if (cur.clicked && allowDraw && curClicked) {
			Vector3 tempMousePos = WorldSpaceCords ();
			distance = Vector3.Distance (tempMousePos, lastPos);
			//if (distance > 0.005f) {
				line.points3.Add (tempMousePos);
				line.Draw3D ();
		//	}
			lastPos = tempMousePos;
		} else {
			allowDraw = false;//stop drawing
			mUpdate = true; // allow the update to happen again
		}



	}
		


}

