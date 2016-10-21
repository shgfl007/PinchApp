using UnityEngine;
using System.Collections;

using Vectrosity;
using System.Collections.Generic;

public class DrawToDistance : VRObject
{

	private VectorLine line;
	public static Vector3 newPoint;


	Vector3 lineOffset = new Vector3(0.0f, 0.0f, 10.0f);

	bool allowDraw = false;
	GameObject vectorCanvas;
	Vector3 currentPos, lastPos;
	public Texture2D lineTex ;
	public Texture2D capTex;

	public GameObject cm;

	Vector3 startPoint;



	float distance = 0;
	public float defaultLineWidth = 3;
	public float magnificationFactor = 20;
	public Cursor cur;
	bool mUpdate = true;

//	public Color[] cl = new Color[3];

	void Start()
	{

	}


	public override void start (ref Cursor c) {

	}


	public override void end () {
	
		mUpdate = true;
	}




	Vector3 WorldSpaceCords () {
		Vector3 cursorCoord =cur.transform.position;
		Vector3 headCoord = Camera.main.transform.position;
		Vector3 vec = cursorCoord - headCoord;
		float radius = 80;
		//	Debug.Log(headCoord.y);
		Vector3 finalCoord = headCoord + vec.normalized * radius;
		//finalCoord.y -= 8f;
		//finalCoord.x -= 5f;

		//finalCoord.z *= -1 * cur.depth * 2 ;

		return finalCoord;
	}

	void Update() {


		if (cur.clicked && UIManage.instance.isDraw && mUpdate) {
		
			mUpdate = false;
			allowDraw = true;
			line = new VectorLine("DrawnLine3D", new List<Vector3>(), defaultLineWidth , LineType.Continuous, Joins.Fill);
			//line.endCap = "rounded";
			line.points3.Clear();
			line.color = UIManage.instance.lineColor;
		//	line.color = cl[2];
			line.lineWidth = 4;
			line.endPointsUpdate = 1;
			startPoint = WorldSpaceCords();
		
		}




		if (cur.clicked && allowDraw && UIManage.instance.isDraw) {
			//line.SetColor (cl[2]);


			Vector3 tempMousePos =  WorldSpaceCords();
			distance = Vector3.Distance (tempMousePos, lastPos);
			if (distance > 0.04f) {
				float lw = defaultLineWidth - Vector3.Distance (startPoint, tempMousePos) / magnificationFactor;
				if (lw < 2) {

					lw = 2;
				}

				line.lineWidth = lw;

				line.points3.Add (tempMousePos);
				line.Draw3D ();
			}

			lastPos = tempMousePos;

		} else {
			allowDraw = false;
			mUpdate = true;
		}



	}

	void makeNewRightLine () {


	}
	void makeNewLeftLine () {


	}


}

