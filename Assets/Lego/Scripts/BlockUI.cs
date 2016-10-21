using UnityEngine;
using System.Collections;

public class BlockUI : MonoBehaviour {

	private static BlockUI mInstance;
	public static BlockUI instance { get { return mInstance; } }

	public GameObject uiOn;

	public GameObject curObject;
	public BlockManip curBlock;
	GameObject ec;
	// Use this for initialization
	void Start () {
		ec = GameObject.Find ("EverythingContainer");

		if(mInstance == null){
			mInstance = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeColor (Material mat) {
		curBlock.changeColor (mat);
	}

	public void canRotate () {
	
		curBlock.RotateObject ();
	}

	public void DestroyObject () {
		this.transform.parent = ec.transform;
		TurnOff ();
		curBlock.FreeObjectUnder ();
		Destroy (curObject.gameObject);
	}

	public void canMove (Cursor curC) {
		curBlock.DisableCollider ();
		curBlock.FreeObjectUnder ();
	//	Cursor c = RightCursor.instance;
		CubeRaycast.instance.isPlaced = true;
		RightCursor.instance.GetComponent<CubeRaycast> ().placeObject(curObject.gameObject, curC, null);
		TurnOff ();

	}

	public void TurnOn() {
		uiOn.SetActive (true);
	}

	public void TurnOff() {
		uiOn.SetActive (false);
		if (ColorPicker.instance != null) {
			ColorPicker.instance.toggleColors (false);
		}
	}


}
