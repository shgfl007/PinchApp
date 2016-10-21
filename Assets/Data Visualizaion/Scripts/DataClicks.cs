using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class DataClicks : VRObject {

	PointData pd;
	public GameObject g;


	public bool canCollect = false;

	public bool mUpdate = false;


	private bool initSelecting = true;

	float timeLeft = 0.4f;

	// Use this for initialization
	void Start () {
		pd = GetComponent<PointData> ();
		//g.SetActive (false);

	}

	public override void start(ref Cursor c){
	
		mUpdate = true;
		DataChangeCamPos.instance.canOrbit = false;


	}

	public override void end() {

		mUpdate = false;
		PointManager.instance.stopCheckRayCast ();

		if (!initSelecting) {
			PointManager.instance.endSelecting ();
		}

		initSelecting = true;
		DataChangeCamPos.instance.canOrbit = true;


	}


	public override void enterHover(Cursor c) {
	
		//g.SetActive (true);
		//pd.

	}

	public override void exitHover(Cursor c) {
		g.SetActive (false);
		PointManager.instance.stopCheckRayCast ();

	}
	
	// Update is called once per frame
	void Update () {
	

		if (mUpdate) {

			if (timeLeft > 0) {
				timeLeft -= Time.deltaTime;

			} else {

				if(initSelecting) {
					PointManager.instance.initSelecting ();
					initSelecting = false;
					
				}
				if(RightCursor.instance.clicked){

					PointManager.instance.checkR = true;
				//	print ("Right Clicked");
				}

				else if (LeftCursor.instance.clicked) {
					PointManager.instance.checkL = true;

					//print ("Left Clicked");
				}


				PointManager.instance.checkRayCast ();

				
			}
			
		}


	}
}
