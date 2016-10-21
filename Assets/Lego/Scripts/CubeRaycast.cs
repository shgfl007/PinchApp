using UnityEngine;
using System.Collections;

public class CubeRaycast : MonoBehaviour {
	
	private static CubeRaycast mInstance;
	public static CubeRaycast instance { get { return mInstance; } }

	public bool canPlace = false;
	GameObject currentGO;
	Cursor c;
	Vector3 updatePos;
	SpawnBlock sb;
	public GameObject ec;

	public bool isPlacing = false;

	public bool isPlaced = false;

	// Use this for initialization
	void Start () {

		if(mInstance == null){
			mInstance = this;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (canPlace) {
			Vector3 cursorScreen = Camera.main.WorldToScreenPoint (c.transform.position);
			Ray screenRay = Camera.main.ScreenPointToRay (cursorScreen);
			RaycastHit hit;
			isPlacing = true;
			UIManage.instance.isAllowed = false;
			RayCast.instance.StopZooming = true;

			DragToMake.instance.canMake = false;

			if (Physics.Raycast (screenRay, out hit)) {
				if (hit.transform.gameObject.tag == "TopCollider") { 


				//	print (hit.transform.gameObject.transform.parent.gameObject.name);

					currentGO.transform.eulerAngles = new Vector3 (0, 0, 0);
			//		updatePos = hit.transform.parent.transform.TransformPoint (Vector3.up);
					updatePos = hit.transform.TransformPoint (Vector3.up);

					currentGO.transform.position = updatePos;
				//	currentGO.GetComponent<BlockManip> ().DisableCollider();


					if (c.clicked) {
		

						if (sb != null) {
							sb.didCreate = true;
						}

						if (isPlaced) {

							currentGO.GetComponent<BlockManip> ().FreeObjectUnder ();
							isPlaced = false;
						}

						currentGO.transform.position = updatePos;
						currentGO.GetComponent<BlockManip> ().EnableCollider();
						currentGO.transform.parent = ec.transform;
						if (hit.transform.parent.transform.gameObject.GetComponent<BlockManip> () != null) {
							BlockManip tempManip = hit.transform.parent.transform.gameObject.GetComponent<BlockManip> ();
							currentGO.gameObject.GetComponent<BlockManip> ().EditObjectUnder (tempManip);
							//print ("dance baby");
						}
						canPlace = false;
						Invoke ("allowUI", 0.2f);
						isPlacing = false;
						DragToMake.instance.canMake = true;
						RayCast.instance.StopZooming = false;

					}				
				
				} else {
				
					Vector3 temp = c.transform.position;
					Vector3 worldToScreen = Camera.main.WorldToScreenPoint (temp);
					Vector3 cubeScreen = Camera.main.WorldToScreenPoint (currentGO.transform.position);
					Vector3 cursorToCube = new Vector3 (worldToScreen.x, worldToScreen.y, cubeScreen.z);
					Vector3 screenToWorld = Camera.main.ScreenToWorldPoint (cursorToCube);
					screenToWorld.y = 10;
					currentGO.transform.position = screenToWorld;
				}

			}



		} 

	}

	public void allowUI () {
		UIManage.instance.isAllowed = true;

	}

	public void placeObject ( GameObject go, Cursor curC, SpawnBlock sbT) {

		currentGO = go;
		canPlace = true;
		c = curC;
		if (sbT != null) {
			sb = sbT;
		}
	}

}
