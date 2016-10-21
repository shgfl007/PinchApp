using UnityEngine;
using System.Collections;

public class RaycastDistance : MonoBehaviour {

	public GameObject avatar;
	public float distance = 20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//RayCast.instance.StopZooming = true;
		//print("this object's transform direction is " + avatar.transform.TransformDirection(avatar.transform.position));
		Vector3 direction = avatar.transform.TransformDirection (avatar.transform.position);
		RaycastHit hit;
		Vector3 tempdirection = direction;
		tempdirection.x = -tempdirection.x; tempdirection.y = -tempdirection.y;
		tempdirection.z = -tempdirection.z;
		if (Physics.Raycast (avatar.transform.position, tempdirection, out hit, distance)) {
			//if(hit.collider.gameObject.transform.parent.name)
			print ("hit " + hit.collider.gameObject.transform.parent.name);
			RayCast.instance.StopZooming = true;
		}
	
	}
}
