using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BrowseScript : VRObject {

	//bool initDone = false;
	//bool animating = false;
	//bool highlightsMode = false;
	bool click = true;
	float scrolledAmount = 0;
	bool newScrollPos = false;
	Transform content = null;
	Transform closestPane;
	float actualRotation = 180;
	
	float animatedInPosition;
	int animateInIntervals;

	
	public override void AfterUpdate()
	{
		if (newScrollPos) {
			newScrollPos = false;
			return;
		}
		
		// Do rotate of parent
		if (content == null) {
			content = transform.parent.parent.parent.parent.parent.parent;
		}
		float xDifference = single.transform.localPosition.x - single.previousCursorPos.x;
		Vector3 rotationEulerVector = content.localRotation.eulerAngles;
		rotationEulerVector.y = rotationEulerVector.y + xDifference*0.3f;
		content.localRotation = Quaternion.Euler(rotationEulerVector);
		
		scrolledAmount += Mathf.Abs (xDifference * 0.3f);
		if (scrolledAmount > 5)
			click = false;
	}
	
	public override void start (ref Cursor cursorInput)
	{
		base.start (ref cursorInput);
		newScrollPos = true;
		content = null;
		
		// For clicking
		click = true;
		scrolledAmount = 0;
	}
	
	// Will move to nearest pane then open the popup
	IEnumerator moveToNearestPane() {
		// Get object to move
		Transform parent = gameObject.transform.parent.parent.parent.parent.parent.parent;
		
		// Do rotation
		float difference = closestPane.rotation.eulerAngles.y - actualRotation;
		float amount;
		//bool usingExitVelocity = false;
		
		// Determine velocity
		amount = difference / 10;
		
		// While the position is not met, keep moving
		while (amount > 0 ? difference > 0.01f : difference < -0.01f) {
			difference -= amount;
			
			// Alter position
			Vector3 newRotation = parent.rotation.eulerAngles;
			newRotation.y = newRotation.y - amount;
			parent.rotation = Quaternion.Euler(newRotation);
			
			// Wait 0.015 seconds before making the next move
			yield return new WaitForSeconds(0.016f);
		}
		
		if (click) {
			audioEffects.transform.FindChild("Click").GetComponent<AudioSource>().Play();
			GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (transform.parent.parent.parent.parent.FindChild("Category Interface").gameObject);
		}
	}
	
	public override void end ()
	{
		// Loop through all boards to see which is the "current" board
		Transform parent = transform.parent.parent.parent.parent.parent.parent;
		
		// Find closest child to x coordinate:
		float difference = 10000;
		for (int i = 0; i < parent.childCount; i++){
			Transform pane = parent.GetChild(i);
			float newDiff = Mathf.Abs(pane.transform.rotation.eulerAngles.y - actualRotation);
			if (newDiff < difference && i != 0) {
				//Debug.Log(pane.name + " x: " + pane.rotation.eulerAngles.y);
				difference = newDiff;
				closestPane = pane;
			}
		}
		
		StartCoroutine (moveToNearestPane());
	}

	public override void enterHover(Cursor c)
	{
		audioEffects.transform.FindChild("Rollover").GetComponent<AudioSource>().Play();
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerEnterHandler);
	}
	
	public override void exitHover (Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerExitHandler);
	}

}
