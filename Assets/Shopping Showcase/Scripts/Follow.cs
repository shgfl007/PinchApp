using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	private static Follow mInstance;
	public static Follow instance {get{return mInstance;}}

	public Transform target;

	// Use this for initialization
	void Start () {

		if (mInstance == null) {
			mInstance = this;
		}
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	public void ChangePos() {
		transform.position = target.position;

	}
}
