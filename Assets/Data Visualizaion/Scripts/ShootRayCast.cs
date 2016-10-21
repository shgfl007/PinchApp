using UnityEngine;
using System.Collections;

public class ShootRayCast : MonoBehaviour {

	Vector3 fwd;
	RaycastHit hit;

	public bool isR, isL;


	Cursor currentCursor;
	Vector3 cursorScreen;
	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	//	transform.TransformDirection (Vector3.forward);

		if (isR) {
			cursorScreen = Camera.main.WorldToScreenPoint (RightCursor.instance.transform.position);
		} else if (isL) {
			cursorScreen = Camera.main.WorldToScreenPoint (LeftCursor.instance.transform.position);

		}
			Ray screenRay = Camera.main.ScreenPointToRay(cursorScreen);

		if(Physics.Raycast(screenRay,  out hit)) 
		{
		//	print (hit.transform.gameObject.name);
			if (hit.transform.gameObject.tag == "data") {
				if (hit.transform.gameObject.GetComponent<PointData> ().collected == false) {
					PointManager.instance.collected.Add (hit.transform.gameObject);
					hit.transform.gameObject.GetComponent<PointData> ().collected = true;
					hit.transform.gameObject.GetComponent<PointData> ().changeColor ();
				}
			}


		}
	}
}
