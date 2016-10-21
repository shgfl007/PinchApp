using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HomeCarouselScript : VRObject {

	bool click = true;
	bool newScrollPos = false;
	Transform content;
	Transform closestChild;
	//float actualX = 8f; // TODO: Switch to local position
	Vector3 actualPosition = new Vector3 (8, 0, 0);
	//float exitVelocity = 0;
	float maxSpeedNoScroll = 1f;
	//Vector3 previousPosition, currentPosition;
	float scrolledDist = 0;

	public override void start (ref Cursor cursorInput)
	{
		single = cursorInput;
		mode = "single";
		click = true;
		content = null;
		newScrollPos = true;
		scrolledDist = 0;
	}
	
	public override void end ()
	{
		// Set actualX (position of the scroll rect is constant)
		actualPosition = transform.parent.parent.parent.position;

		// Loop through all boards to see which is the "current" board
		Transform parent = gameObject.transform.parent.parent;

		// Find closest child to x coordinate:
		float difference = 10000;
		for (int i = 0; i < parent.childCount; i++){
			Transform child = parent.GetChild(i);
			//float newDiff = Mathf.Abs(child.transform.position.x - actualX);
			float newDiff = (actualPosition - child.transform.position).magnitude;
			if (newDiff < difference) {
				//Debug.Log(child.name + " x: " + child.transform.position.x);
				difference = newDiff;
				closestChild = child;
			}
		}

		if (click) {
			// Open content if clicked
			OpenContentScript.openContent(GameObject.Find("Home Interface"), 15, "Video");
		}

		// Start moving to nearest board
		StartCoroutine ("moveToNearestTile");
	}

	IEnumerator moveToNearestTile() {
		Vector3 difference = closestChild.position - actualPosition;
		Vector3 amount;
		bool usingExitVelocity = false;

		// Determine velocity
		// (false && exitVelocity != 0 && Mathf.Abs (exitVelocity * 1.5f) < Mathf.Abs (difference)) {
			//amount = (difference/Mathf.Abs (difference)) * Mathf.Abs (exitVelocity); // Direction (+/-) * magnitude
		//	usingExitVelocity = true;
		//}
		//else {
			amount = difference / 20;
		//}

		// Get object to move
		Transform parent = gameObject.transform.parent.parent;

		// While the position is not met, keep moving
		while (difference.magnitude > 0.01f) {
			difference -= amount;

			// Alter position
			Vector3 newLocation = new Vector3(parent.position.x, parent.position.y, parent.position.z);
			newLocation = newLocation - amount;
			parent.position = newLocation;

			// Logging
			//Debug.Log(closestChild.name + ", " + parent.name + ", " + amount + ", " + newLocation.x);
			if (usingExitVelocity) {
				//Debug.Log ("exit velocity");
			}

			// Wait 0.015 seconds before making the next move
			yield return new WaitForSeconds(0.015f);
		}

		// Set the dot
		closestChild.parent.parent.parent.FindChild("Dots").GetComponent<DotScript>().setActiveTileIndex(closestChild.GetSiblingIndex ());
	}

	public override void AfterUpdate()
	{
		if (newScrollPos) {
			newScrollPos = false;
			return;
		}

		if (single.verticalSpeed > maxSpeedNoScroll && single.horizontalSpeed > maxSpeedNoScroll) {
			click = false;
			//Debug.Log("click off");
		}

		// Do scroll of parent
		if (content == null)
			content = single.heldTransform.parent.parent;
		scrolledDist += Mathf.Abs (horizontalScroll (content.gameObject, 10, false));
		if (scrolledDist > 150)
			click = false;

		// Get exit velocity
		//if (currentPosition != null)
			//previousPosition = currentPosition;
		//currentPosition = content.transform.position;
		//exitVelocity = currentPosition.x - previousPosition.x;
	}

	public override void enterHover(Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerEnterHandler);
	}
	
	public override void exitHover (Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerExitHandler);
	}
}
