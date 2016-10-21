using UnityEngine;
using System.Collections;

public class DummyCam : MonoBehaviour {



	private static DummyCam mInstance;
	public static DummyCam instance {get{return mInstance;}}

	public bool followCamera = true;
	public GameObject cm;
	public bool setParent;



	// Use this for initialization
	void Start () {


		if (mInstance == null) {
		
			mInstance = this;
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		if (followCamera && SpaceManager.instance.isSpaceBuilder) {
			cm.transform.parent = null;
			transform.position = cm.transform.position;
			setParent = true;
		} else {
			if (setParent) {
				Vector3 tempPos = transform.position;
				tempPos.z += 10f;
				transform.position = tempPos;
				cm.transform.parent = transform;
				followCamera = false;
				setParent = false;
			}
		}



	
	}
}
