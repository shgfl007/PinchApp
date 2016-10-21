using UnityEngine;
using System.Collections;

public class UIManage : MonoBehaviour {

	private static UIManage mInstance;
	public static UIManage instance { get { return mInstance; } }


	public GameObject rayBlocker, UI, aRing, bigBG;//, colorOptions;

	GameObject menuHolder;

	public bool isOn = false;
	Cursor c;

	public Vector3 curPos;

	public Color lineColor;

	public bool isAllowed= true;

	public Material curColor;

	public bool isDraw = false;



	// Use this for initialization
	void Start () {
		menuHolder = GameObject.Find ("MenuHolder");

		UI.SetActive (false);
		if (mInstance == null) {
		
			mInstance = this;

		}

	}
	
	// Update is called once per frame
	void Update () {
	

		if (isDraw) {
			RayCast.instance.StopZooming = true;
		}
		else {
			RayCast.instance.StopZooming = false;


		}
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
		}*/

	}


	public void RingActivation(Vector3 pos, bool isActive) {
		if (!isOn) {
			
			if (isAllowed) {
				pos.z = 200;
				this.gameObject.transform.localPosition = pos;
			//	UI.gameObject.transform.parent = menuHolder.transform;
				//aRing.gameObject.transform.parent = menuHolder.transform;
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
			BlockUI.instance.TurnOff ();
			UI.SetActive (true);
		}
	}                                      


	public void turnOffUI () {
	
		isOn = false;
		rayBlocker.SetActive (false);
		UI.SetActive (false);
		aRing.SetActive (false);
	//	UI.gameObject.transform.parent = this.transform;
	//	aRing.gameObject.transform.parent = this.transform;

		//UI.gameObject.transform.localPosition = new Vector3 (0,0,0);
		//aRing.gameObject.transform.localPosition = new Vector3 (0,0,0);


	}



}
