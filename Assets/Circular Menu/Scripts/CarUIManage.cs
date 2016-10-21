using UnityEngine;
using System.Collections;

public class CarUIManage : MonoBehaviour {

	private static CarUIManage mInstance;
	public static CarUIManage instance { get { return mInstance; } }

	public GameObject rayBlocker;
	public GameObject  UI, aRing, bigBG;//, colorOptions;

	public bool isOn = false;
	Cursor c;

	public Vector3 curPos;

	//public Color lineColor;

	public bool isAllowed= true;

	//public Material curColor;

//	public bool isDraw = false;



	// Use this for initialization
	void Start () {
	
		UI.SetActive (false);
		if (mInstance == null) {
		
			mInstance = this;

		}

	}
	
	// Update is called once per frame
	void Update () {
	/*

		if (isOn) {
		
			if (c.clicked) {
			
			//	Quaternion newRotation = Quaternion.LookRotation (bigBG.transform.position- c.transform.position, Vector3.forward);
			//	newRotation.x = 0;
			//	newRotation.y = 0;

			//	bigBG.transform.rotation = Quaternion.Slerp (bigBG.transform.rotation, newRotation, Time.deltaTime * 8);

			} else {
				isOn = false;
				Invoke("turnOffUI", 0.2f);
			}
		}
		*/

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
