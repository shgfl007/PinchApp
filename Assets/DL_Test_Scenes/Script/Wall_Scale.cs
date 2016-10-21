using UnityEngine;
using System.Collections;


public class Wall_Scale : MonoBehaviour
{
	public GameObject AffectedWall_Left;
	public GameObject AffectedWall_Right;

	private Vector3 prePosition;
	private Vector3 lastPosition;
	private bool isTriggered = false;

	private RaycastHit hit;
	private Vector3 screenPoint, offset;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			//print ("Hit something!");
			if ((hit.collider.gameObject.name == this.name)) {
				if (Input.GetMouseButtonDown (0)) {
					//print ("hit this");
					if (!isTriggered) {
						print ("start dragging");
						isTriggered = true;
						prePosition = this.transform.position;
						screenPoint = Camera.main.WorldToScreenPoint (this.transform.position);
						offset = this.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
					}
				} else if (isTriggered && Input.GetMouseButtonUp (0)) {
					print ("end dragging");
					isTriggered = false;
					lastPosition = this.transform.position;
					ScaleWalls (prePosition, lastPosition);
					MoveWalls (prePosition, lastPosition);
				}
			}

		}
//
		if (isTriggered) {

			print ("dragging");
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			this.transform.transform.position = curPosition;
		}

	

	
	}


	void ScaleWalls(Vector3 oldPos, Vector3 newPos){
		
		float y_distance = newPos.y - oldPos.y;
		float newHeightR = y_distance / 2f + AffectedWall_Right.GetComponent<MeshRenderer> ().bounds.size.y;
		float newHeightL = y_distance / 2f + AffectedWall_Left.GetComponent<MeshRenderer> ().bounds.size.y;
		print ("newHeightR is " + newHeightR);
		float oldHeightR = AffectedWall_Right.GetComponent<MeshRenderer> ().bounds.size.y;
		float oldHeightL = AffectedWall_Left.GetComponent<MeshRenderer> ().bounds.size.y;
		print ("oldHeightR is " + oldHeightR);
		float sRatioR = newHeightR / oldHeightR; float sRatioL = newHeightL / oldHeightL;
		Vector3 rScale = AffectedWall_Right.transform.localScale;
		Vector3 lScale = AffectedWall_Left.transform.localScale;
		rScale.y = rScale.y * sRatioR; lScale.y = lScale.y * sRatioL;

		AffectedWall_Right.transform.localScale = rScale; AffectedWall_Left.transform.localScale = lScale;
//		AffectedWall_Right.transform.localScale += new Vector3(y_distance / 2f, 0f,0f);
//		AffectedWall_Left.transform.localScale += new Vector3(y_distance / 2f, 0f,0f);

		print ("y distance is " + y_distance);

	}

	void MoveWalls(Vector3 oldPos, Vector3 newPos){
		float y_distance = newPos.y - oldPos.y;
		Vector3 Rpos = AffectedWall_Right.transform.position;
		Vector3 Lpos = AffectedWall_Left.transform.position;
		Rpos.y = Rpos.y + y_distance/2f;
		Lpos.y = Lpos.y + y_distance/2f;
		AffectedWall_Right.transform.position = Rpos;
		AffectedWall_Left.transform.position = Lpos;

	}



}
