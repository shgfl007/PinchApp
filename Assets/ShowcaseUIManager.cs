using UnityEngine;
using System.Collections;

public class ShowcaseUIManager : MonoBehaviour {

	private static ShowcaseUIManager mInstance;
	public static ShowcaseUIManager instance { get { return mInstance; } }

	public GameObject rayBlocker;
	public GameObject  UI, aRing, bigBG;//, colorOptions;

	public bool isOn = false;
	Cursor c;

	public Vector3 curPos;
	public bool isAllowed= true;
	// Use this for initialization
	void Start () {
		UI.SetActive (false);
		if (mInstance == null) {

			mInstance = this;

		}
	}
	

	public void RingActivation(Vector3 pos, bool isActive) {
		if (!isOn) {

			if (isAllowed) {
				pos.z = 30;
				this.gameObject.transform.localPosition = pos;
				aRing.SetActive (true);
			}
		}else {

			turnOffUI ();
		}
	}


	public void turnOnUI (Cursor curC) {

		if (isAllowed) {
			c = curC;
			isOn = true;
			rayBlocker.SetActive (true);
			UI.SetActive (true);
		}
	}                                      


	public void turnOffUI () {

		isOn = false;
		rayBlocker.SetActive (false);
		UI.SetActive (false);
		aRing.SetActive (false);
		//colorOptions.SetActive (false);

	}


}
