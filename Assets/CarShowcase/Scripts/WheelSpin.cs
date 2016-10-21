using UnityEngine;
using System.Collections;

public class WheelSpin : VRObject {


	Vector2 previousMousePosition = Vector3.zero;
	Vector2 currentMousePosition = Vector3.zero;
	bool previousFrameMouseDown = false;

	bool canRotate = false;
	//float ang;
	public bool mUpdate = false;
	Vector2 mousePos;
	Cursor curCursor;

	// Use this for initialization
	void Start () {
	
	}

	public override void start(ref Cursor c) {
		curCursor = c;

		mUpdate = true;

		Vector3 cTemp = Camera.main.WorldToScreenPoint (curCursor.transform.position);
		mousePos = new Vector2(cTemp.x, 
			cTemp.y);
		previousMousePosition = mousePos;
		currentMousePosition = mousePos;
	}

	public override void end() {
	//	baseAngle = ang;
		mUpdate = false;
		previousFrameMouseDown = false;	

	}

	// Update is called once per frame
	void Update () {



		if (mUpdate) {

			Vector3 cTemp = Camera.main.WorldToScreenPoint (curCursor.transform.position);
			mousePos = new Vector2(cTemp.x, 
				cTemp.y);
				previousMousePosition = currentMousePosition;
				currentMousePosition = mousePos;

			Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
			Vector2 screenPositionXY = new Vector2(screenPosition.x, screenPosition.y);
			Vector2 previousPositionVector = previousMousePosition - screenPositionXY;
			Vector2 currentPositionVector = currentMousePosition - screenPositionXY;


			if (previousPositionVector != -currentPositionVector )
			{
				float rotationAmount = ReturnSignedAngleBetweenVectors(previousPositionVector,
					currentPositionVector);

				transform.RotateAroundLocal(Vector3.forward, rotationAmount * Time.deltaTime *-1);
			}


		}

	}


	private float ReturnSignedAngleBetweenVectors(Vector2 vectorA, Vector2 vectorB)
	{
		Vector3 vector3A = new Vector3(vectorA.x, vectorA.y, 0f);
		Vector3 vector3B = new Vector3(vectorB.x, vectorB.y, 0f);

		if (vector3A == vector3B)
			return 0f;

		// refVector is a 90cw rotation of vector3A
		Vector3 refVector = Vector3.Cross(vector3A, Vector3.forward);
		float dotProduct = Vector3.Dot(refVector, vector3B);

		if (dotProduct > 0)
			return -Vector3.Angle(vector3A, vector3B);
		else if (dotProduct < 0)
			return Vector3.Angle(vector3A, vector3B);
		else
			throw new System.InvalidOperationException("the vectors are opposite");
	}



}
