using UnityEngine;
using System.Collections;

public class RoomBuilderMenu : VRObject {
	private static RoomBuilderMenu mInstance;
	public static RoomBuilderMenu instance { get { return mInstance; } }
	bool mUpdate = false;
	float timeToHold = 0.45f;
	float timer = 0;
	public GameObject menu;


	// Use this for initialization
	void Start () {
		if (mInstance == null) {
		
			mInstance = this;
		}

		menu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {


		if(mUpdate){

			timer += Time.deltaTime;

			if(timer > timeToHold){
				mUpdate = false;
				timer = 0;
				enableMenu ();

				
			}
		}
	
	}


	public void enableMenu() {
		menu.SetActive (true);
	}


	public void disableMenu() {
		menu.SetActive (false);

	}


	public override void start(ref Cursor c) {
	
		if (c==LeftCursor.instance) {
			mUpdate = true;
		//	print ("It works");

		} else {
		
			disableMenu ();
		}
	
	
	}	

	public override void end() {

		mUpdate = false;

	}

}
