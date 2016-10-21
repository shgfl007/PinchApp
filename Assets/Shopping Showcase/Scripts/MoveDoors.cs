using UnityEngine;
using System.Collections;

public class MoveDoors : VRObject {
	private static MoveDoors mInstance;
	public static MoveDoors instance {get{return mInstance;}}


	public GameObject posToBe;
	public GameObject posToBe2;

	public GameObject d1, d2;
	bool canMove = false;

	// Use this for initialization
	void Start () {
		if (mInstance == null) {

			mInstance = this;
		}

	}

	public override void end() {
		canMove = true;
		ShopChangeCamPos.instance.goToBunch ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (canMove && Vector3.Distance (this.transform.position, posToBe.transform.position )> 2) {
			d1.transform.position = Vector3.MoveTowards (d1.transform.position, posToBe.transform.position, Time.deltaTime * 10);
			d2.transform.position = Vector3.MoveTowards (d2.transform.position, posToBe2.transform.position, Time.deltaTime * 10);


		} else {
			canMove = false;

		}


		if (!canMove && Vector3.Distance (d1.transform.position, posToBe.transform.position) < 2) {
			this.GetComponent<BoxCollider> ().enabled = false;
		}

	
	}
}
