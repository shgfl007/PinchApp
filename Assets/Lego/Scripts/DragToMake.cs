using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DragToMake : VRObject {
	
	private static DragToMake mInstance;
	public static DragToMake instance { get { return mInstance; } }

	public Image img;
	public Sprite[] spr = new Sprite[3];
	public List<GameObject> pHolders = new List<GameObject>();
	public List<GameObject> spawners = new List<GameObject>();


	public List<GameObject> matToChange = new List<GameObject>();

	public GameObject objectToSpawn;

	//public List<GameObject> children;

	public GameObject rayBlocker;

	public bool didMake = false;
	public bool didMove = false;
	bool clickedLeft = false;

	bool createObject = false;

	public int holderToActivate = 0;
	//public SpriteRenderer sr;
	Cursor c;
	GameObject block;
	int mappedDistance;
	Material mat;

	public bool canMake = true;
	public int lastHolder = 0;


	// Use this for initialization
	void Start () {

		if (mInstance == null) {
		
			mInstance = this;
		}

		rayBlocker.SetActive (false); // deactivate RayBlocker
		objectToSpawn = pHolders [holderToActivate]; //first object to activate
		lastHolder = holderToActivate; // last holder if someone switches to the drawing mode right away
		objectToSpawn.SetActive(false); //disable the object until function starts
		//addChildren ();
		resetEverything (); //reset everythign incase if you screwed up on some weird settings
		Invoke("updateColor", 0.01f);
	//	updateColor (); // change material
	}

	//Changing Material of Object
	public void updateColor () {
		foreach (GameObject g in matToChange){

			Renderer tempRend = g.gameObject.GetComponent<MeshRenderer> ();
			tempRend.material = UIManage.instance.curColor;
		}
	}


	//To update the hand image to last selected Object
	public void updateImage() {
		img.sprite = spr[holderToActivate];
		objectToSpawn = pHolders [holderToActivate];
		lastHolder = holderToActivate;
	}


// To toggle the drawing mode image
	public void toggleDrawingImage() {

		//to Change to drawing Image
		if (UIManage.instance.isDraw) {

			img.sprite = spr [4];
		} else {
		
			img.sprite = spr[lastHolder];
		}

		
	}

	public override void start(ref Cursor curC) {
	
		c = curC;

		//only initiate if drawing is disabled
		if (UIManage.instance.isDraw == false) {

			rayBlocker.SetActive (true);
			objectToSpawn.SetActive (true);
			didMake = true;
			RayCast.instance.StopZooming = true;
			updatePosition ();
			//WorldSpaceCords ();
			didMove = false;

			//addChildren ();
		} 

	}


	/*public void addChildren() {
	
		foreach (Transform child in objectToSpawn.transform) {
			children.Add(child.transform.gameObject);
		}
	}*/

	public override void end() {

		//if drawing is false then run this
		if (UIManage.instance.isDraw == false) {
			didMake = false;

			if (!didMove && canMake) {
				int spawnIndex = (holderToActivate * 3) + mappedDistance;
				//print ("Spawn Index was " + spawnIndex);
				block = Instantiate (spawners [spawnIndex], Vector3.zero, Quaternion.identity) as GameObject;
				block.gameObject.GetComponent<BlockManip> ().changeColor(UIManage.instance.curColor);

				RightCursor.instance.gameObject.GetComponent<CubeRaycast> ().placeObject (block.gameObject, c, null);
			}
			resetEverything ();
		} else {
			//turn off drawing to return to object creation
			UIManage.instance.isDraw = false;
			holderToActivate = lastHolder;
			toggleDrawingImage ();
		}
	}


	void WorldSpaceCords () {
		Vector3 tempR = RightCursor.instance.transform.position;
		Vector3 tempL = LeftCursor.instance.transform.position;
		float getAverageXPosition = (tempL.x + tempR.x)/2;
		float getAverageZPosition = (tempL.z + tempR.z)/2;
		Vector3 tempAvgVal = new Vector3 (getAverageXPosition, tempL.y, getAverageZPosition);

		Vector3 cursorCoord = tempAvgVal;
		Vector3 headCoord = Camera.main.transform.position;
		Vector3 vec = cursorCoord - headCoord;
		float radius = 80;
		if (radius > headCoord.y)
			radius = headCoord.y;
		//	Debug.Log(headCoord.y);
		Vector3 finalCoord = headCoord + vec.normalized * radius;


		objectToSpawn.transform.position = finalCoord;

		//return finalCoord;
	}

	void updatePosition () {

		//update position -> to be updated
		Vector3 tempR = RightCursor.instance.transform.position;
		Vector3 tempL = LeftCursor.instance.transform.position;
		float getAverageXPosition = (tempL.x + tempR.x)/2;
		float getAverageZPosition = (tempL.z + tempR.z)/2;
		Vector3 tempAvgVal = new Vector3 (getAverageXPosition, tempL.y, getAverageZPosition);
		Vector3 worldToScreen = Camera.main.WorldToScreenPoint (tempAvgVal);
		Vector3 cubeScreen = Camera.main.WorldToScreenPoint (objectToSpawn.transform.position);
		Vector3 cursorToCube = new Vector3 (worldToScreen.x, worldToScreen.y, cubeScreen.z);
		Vector3 screenToWorld = Camera.main.ScreenToWorldPoint (cursorToCube);
		screenToWorld.y = 48.7f;
		screenToWorld.z = -24.5f;
		objectToSpawn.transform.position = screenToWorld;
	//	objectToSpawn.transform.LookAt (transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

	}

	// Update is called once per frame
	void Update () {



		if (UIManage.instance.isDraw == false) {

			if (didMake && canMake) {

				if (LeftCursor.instance.clicked) {
					clickedLeft = true;
					didMove = true;

				}


			}

			if (clickedLeft && RightCursor.instance.clicked && LeftCursor.instance.clicked) {

				float curDistance = Vector3.Distance (RightCursor.instance.transform.position, LeftCursor.instance.transform.position);
				mappedDistance = (int)Remap (curDistance, 0.2f, 0.7f, 0, 2);

				float scaleValue = Remap (curDistance, 0.2f, 0.7f, 0, 2);

				float scaleRate = (1 + scaleValue) * 10;

				if (scaleRate > 30) {

					scaleRate = 30;
				}

				objectToSpawn.transform.localScale = new Vector3 (scaleRate, scaleRate, scaleRate);
				RayCast.instance.StopZooming = true;
				/*
				if (mappedDistance == 0) {

					children [0].SetActive (true);
					children [1].SetActive (false);
					children [2].SetActive (false);

				} else if (mappedDistance == 1) {
					children [0].SetActive (true);
					children [1].SetActive (true);
					children [2].SetActive (false);
				} else if (mappedDistance == 2) {

					children [0].SetActive (true);
					children [1].SetActive (true);
					children [2].SetActive (true);
				}
				*/




				updatePosition ();
				//WorldSpaceCords ();

		//		print ("THe mapped distance is " + mappedDistance); <<

				//	print (Vector3.Distance (RightCursor.instance.transform.position, LeftCursor.instance.transform.position));

				//Make Magic Here

			} else if (clickedLeft && !RightCursor.instance.clicked || LeftCursor.instance.clicked && clickedLeft) {
		

				if (mappedDistance > 2) {
					mappedDistance = 2;
				}

				int spawnIndex = (holderToActivate * 3) + mappedDistance;

				block = Instantiate (spawners [spawnIndex]);
				block.gameObject.GetComponent<BlockManip> ().changeColor(UIManage.instance.curColor);

				RightCursor.instance.gameObject.GetComponent<CubeRaycast> ().placeObject (block.gameObject, c, null);

				//children [0].SetActive (false);
				//children [1].SetActive (false);
				//children [2].SetActive (false);
				resetEverything ();

				//objectToSpawn.gameObject.SetActive (false);
			} 

		} else {
		
		
			
		}
	}

	public void resetEverything () {
		//children.Clear ();
		RayCast.instance.StopZooming = false;

		objectToSpawn.transform.localScale = new Vector3 (10, 10, 10);
		clickedLeft = false;		
		rayBlocker.SetActive (false);
		objectToSpawn.SetActive (false);
		didMake = false;
		RayCast.instance.StopZooming = false;
		mappedDistance = 0;
	//	didMove = false;
	}


	public float Remap(float value, float from1, float to1, float from2, float to2) {
	
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}


}
