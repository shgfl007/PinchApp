using UnityEngine;
using System.Collections;

public class LookAtHands : MonoBehaviour {

	public Transform targetAnchor;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPosition = new Vector3 (this.transform.position.x, this.transform.position.y, targetAnchor.position.z);

	//	transform.rotation = Quaternion.FromToRotation (transform.up, transform.position - targetPosition) * transform.rotation;

		Quaternion tempRot = Quaternion.LookRotation (transform.position - targetPosition);
		tempRot.eulerAngles = new Vector3 (0, 0, tempRot.eulerAngles.z);

		this.transform.rotation = tempRot;
		//transform.Rotate (Vector3.right, -90);

		//this.transform.LookAt (targetPosition);
	}
}
