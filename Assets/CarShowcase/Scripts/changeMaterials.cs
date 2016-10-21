using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class changeMaterials : MonoBehaviour {


	private static changeMaterials mInstance;
	public static changeMaterials instance { get { return mInstance; } }


	public  Material toMat;
	//public Material toMat, tempMat;
	public List<GameObject> matToChange;

	int randomMat;
//	bool isOpen = false;

	//public GameObject toTurnOn;
	public GameObject car;
	public Animator carAnim;
	// Use this for initialization
	void Start () {

		if (mInstance == null) {
		
			mInstance = this;
		}

	}

	/*
	public void turnOnColors () {
		toTurnOn.SetActive (true);

	}

	public void turnOffColors () {
		toTurnOn.SetActive (false);

	}

*/
	public void assignMat (Material mati){
		toMat = mati;

	}

	public void changeMat() {

		carAnim.Play ("CarSwitch", -1, 0);
		Invoke ("changeColors", 0.5f);


		//toMat = tempMat;

	}

	void changeColors () {
		foreach(GameObject f in matToChange){

			f.gameObject.GetComponent<Renderer> ().material = toMat;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.O)) {
			//tempMat = matToChange [0].GetComponent<Renderer> ().material;

		}

	}
}
