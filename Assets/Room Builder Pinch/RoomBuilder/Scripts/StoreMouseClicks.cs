using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class StoreMouseClicks : VRObject
{

	public List<Vector3> mouseClicks = new List<Vector3> ();
	Vector3 mousePosition, targetPosition;
	float distance = 10f;
	public GameObject pointPrefab;
	GameObject point;
	public GameObject wallP;
	GameObject ec; //Everything Container
	// You can change that color, to change the color of the connecting lines
	public GameObject meshCreator;
	private int wallCount;
	bool drawingDone = false;
	Ray ray;
	RaycastHit hit;
	bool pointAdd = true;

	public GameObject dummyToExpand;

    public GameObject door;


	Cursor c;
	bool mUpdate = false;

	void Start() {

		ec = GameObject.Find ("EverythingContainer");
	}
	public override void start (ref Cursor cursorInput) {
		c = cursorInput;
		pointAdd = true;
	}
	public override void end() {
		mUpdate = false;

		if (pointAdd) {
			AddPoint ();
		}
	}



	public override void dualCursorUpdate (ref Cursor left, ref Cursor right)
	{

		pointAdd = false;

		print(pointAdd);

		base.dualCursorUpdate (ref left, ref right);
	}

	public void AddPoint() {
	

		if (!drawingDone) {

			if (pointAdd) {
				//To get the current mouse position
				//mousePosition = Input.mousePosition;
				mousePosition = Camera.main.WorldToScreenPoint (c.transform.position);

				Vector3 cubeScreen = Camera.main.WorldToScreenPoint (transform.position);

				Vector3 cursorToCube = new Vector3 (mousePosition.x, mousePosition.y, cubeScreen.z);
				Vector3 screenToWorld = Camera.main.ScreenToWorldPoint (cursorToCube);
				screenToWorld.y = 50;


				targetPosition = screenToWorld;
				Vector3 currentPos = targetPosition;
				currentPos = new Vector3 (Mathf.Round (currentPos.x),
					Mathf.Round (currentPos.y),
					Mathf.Round (currentPos.z));
				targetPosition = currentPos;
				print(targetPosition);
				foreach (Vector3 wallPoint in mouseClicks) {
					if (targetPosition == wallPoint) {
						if (targetPosition == mouseClicks [0]) {
							CompleteWalls ();
							print ("It's trying to complete Wall");

						}
						return;
					}
				}

				print ("It gets past return");
				mouseClicks.Add (targetPosition);
                if(mouseClicks.Count == 1)
                {
                    point = (GameObject)Instantiate(pointPrefab, targetPosition, Quaternion.identity);
                    //point.transform.position.y = targetPosition.y;
                    point.transform.parent = dummyToExpand.transform;
                }
	

				//targetPosition = point.transform.localPosition;

			//	targetPosition.y = 0;

				//point.transform.localPosition = targetPosition;

				CreateWall ();


			}
		}
	}

	void CreateWall() {
		
		if (mouseClicks.Count > 1) {
			GameObject wall = (GameObject)Instantiate (wallP, mouseClicks[wallCount], Quaternion.identity);

			wall.transform.LookAt (mouseClicks [wallCount + 1]);
			wall.transform.localScale = new Vector3(1,1, Vector3.Distance(mouseClicks[wallCount],mouseClicks[wallCount+1]));
			wall.transform.localScale += new Vector3(0,0, wall.transform.localScale.x/5f);
			wall.transform.parent = dummyToExpand.transform;
			wallCount++;

		}
	}
	void CompleteWalls() {

		meshCreator.GetComponent<PolygonTester> ().startMesh ();
		mouseClicks.Add (mouseClicks [0]);
		CreateWall ();

        door.SetActive(true);

		drawingDone = true;
	}




}
