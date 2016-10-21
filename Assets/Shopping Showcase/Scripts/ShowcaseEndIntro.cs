using UnityEngine;
using System.Collections;

public class ShowcaseEndIntro : VRObject {

	public GameObject UIToHide;
	public GameObject ec;

	// Use this for initialization
	void Awake () {
		UIToHide.SetActive (false);
		ec.SetActive (false);

	}

	// Update is called once per frame
	void Update () {

	}
	public override void start(ref Cursor c) {

	}

	public override void end() {

		UIToHide.SetActive (true);
		ec.SetActive (true);
		this.gameObject.transform.parent.parent.gameObject.SetActive (false);
	}
}
