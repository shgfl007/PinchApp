using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManip : VRObject {

	public bool mUpdate = false;
	public Cursor c;
	public bool isBeingDropped = true;
	public BoxCollider[]  blockCollider;
	//public BoxCollider baseCollider;

	public bool canBeEdited = true;
	public GameObject objectUnder;
	BlockManip objectUnderManip;

	GameObject canvasHolder;

	public List<GameObject> matToChange = new List<GameObject>();

	public bool canMove = false;
	public GameObject objToRotate;

	public Vector3 uiPosition;

	public bool movedAfter = false;
	// Use this for initialization
	void Start () {
	
		canvasHolder = GameObject.Find ("CanvasHolder");
		DisableCollider ();
	}


	public override void start (ref Cursor cursorInput)
	{
		c = cursorInput;


	}
	public override void end() {

		if (CubeRaycast.instance.isPlacing == false && canBeEdited && !UIManage.instance.isDraw) {

			//print ("it gets here");
			summonUI ();
		}
		//print (gameObject.name + "This object can be edited: " + canBeEdited);

	}
	
	// Update is called once per frame
	void Update () {

	}


	public void RotateObject() {

		objToRotate.gameObject.transform.Rotate(0, 90, 0);
	}


	public void DisableCollider () {
		for (int i = 0; i < blockCollider.Length; i++) {
			blockCollider [i].enabled = false;
		}
		//baseCollider.enabled = false;

	}

	public void EditObjectUnder (BlockManip g) {
		//objectUnder = g;
//		objectUnder = go;
		objectUnderManip = g;
		objectUnderManip.canBeEdited = false;
		//objectUnderManip.DisableCollider ();
	}


//	public void DisableTopColliders () {
	
//		for (int i = 0; i < blockCollider.Length; i++) {
//			blockCollider [i].enabled = false;
//		}
//	}

	public void resetEditing() {
	
		movedAfter = true;

	}

	public void EnableCollider() {
		for (int i = 0; i < blockCollider.Length; i++) {
			blockCollider [i].enabled = true;
		}
	//	baseCollider.enabled = true;

	}


	public void FreeObjectUnder () {
		if(objectUnderManip != null){
			objectUnderManip.canBeEdited = true;
		//	objectUnderManip.EnableCollider ();
		}
	//	print (gameObject.name + "This object can be edited: " + canBeEdited);


	}

	public void changeColor (Material mat) {

		Material m = mat;
		Renderer rend;

		foreach (GameObject g in matToChange) {
			//print ("numbers");
			rend = g.GetComponent<MeshRenderer> ();
			rend.material = m;
		}

//		print ("It enters the Change Color Function properly");

	}


	public void summonUI() {
		BlockUI.instance.TurnOn ();
		canvasHolder.transform.parent = this.transform;
		canvasHolder.transform.localPosition = uiPosition;
		BlockUI.instance.curObject = this.gameObject;
		BlockUI.instance.curBlock = this.gameObject.GetComponent<BlockManip> ();
	}
		




}
