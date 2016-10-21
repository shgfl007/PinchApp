using UnityEngine;
using System.Collections;

public class EndStartScreen : VRObject {

	public GameObject introScreen;
	public Animator anim;
	bool stopClicks = false;

	// Use this for initialization
	void Start () {
	
	}


	public override void end() {

		if (!stopClicks) {
			Invoke ("StartAnim", 0.9f);
			anim.enabled = true;
			stopClicks = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartAnim() {
		introScreen.SetActive (false);

	}

}
