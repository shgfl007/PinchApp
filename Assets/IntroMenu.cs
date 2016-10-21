using UnityEngine;
using System.Collections;

public class IntroMenu : MonoBehaviour {
	private static IntroMenu mInstance;
	public static IntroMenu instance { get { return mInstance; } }


	private GameObject menuHolder;
	public GameObject  UI, aRing, bigBG, rayBlocker;//, colorOptions;

	public bool isOn = false;
	Cursor c;
	public Vector3 curPos;
	public bool isAllowed= true;

	private Vector3 origUIPos;

	// Use this for initialization
	void Start () {
		menuHolder = GameObject.Find ("MenuHolder");
		UI.SetActive (false);

		if (mInstance == null) {

			mInstance = this;

		}
		//origUIPos = UI.transform.position;
	}

	// Update is called once per frame
	void Update () {
		/*if (isOn) {

			if (c.clicked) {

			} else {
				isOn = false;
				Invoke("turnOffUI", 0.2f);
			}
		} */
	}

	public void RingActivation(Vector3 pos, bool isActive) {
		if (!isOn) {
			if (isAllowed) {
				//pos.z = 1.2f;
				pos.z = 100;
				this.gameObject.transform.localPosition = pos;
				UI.gameObject.transform.parent = menuHolder.transform;
				aRing.gameObject.transform.parent = menuHolder.transform;
				aRing.SetActive (true);
			}
		} else {
		
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
		UI.gameObject.transform.parent = this.transform;
		aRing.gameObject.transform.parent = this.transform;

		UI.gameObject.transform.localPosition = new Vector3 (0,0,0);
		aRing.gameObject.transform.localPosition = new Vector3 (0,0,0);

		//colorOptions.SetActive (false);

	}

}
