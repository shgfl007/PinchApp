using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowHomepagePopupScript : VRObject {

	//bool initDone = false;
	//bool animating = false;
	//bool highlightsMode = false;
	bool click = true;
	float scrolledAmount = 0;
	bool newScrollPos = false;
	Transform content = null;
	Transform closestPane;
	Transform categories;
	//EverythingContainerScript everythingScript;
	GameObject popUp;
	float actualRotation = 180;

	float animatedInPosition;
	int animateInIntervals;

	void Start() {
		//transform.parent.parent.FindChild ("Category Interface").rotation = Quaternion.Euler(new Vector3 (0, 0, 0));
		transform.parent.parent.parent.FindChild ("Category Interface").GetComponent<CanvasGroup> ().alpha = 0f;
		
		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().addManagedInterface (
			transform.parent.parent.parent.FindChild ("Category Interface").gameObject);
		
		//everythingScript = GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ();

		categories = transform.parent.parent.parent.parent.parent;

		popUp = transform.parent.parent.parent.FindChild("PopupPanel").gameObject;
		popUp.SetActive (false);

		HomeControllerScript homeController = GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ();
		animatedInPosition = homeController.animatedInPosition;
		animateInIntervals = homeController.popupAnimationIntervals;
		audioEffects = GameObject.Find ("AudioEffects");
	}

	public override void AfterUpdate()
	{
		if (newScrollPos) {
			newScrollPos = false;
			return;
		}
		
		// Do rotate of parent
		if (content == null) {
			content = categories;
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
		Transform parent = categories;
		
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
			yield return new WaitForSeconds(0.015f);
		}

		if (click) {
			HomeControllerScript homeController = GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ();
			if (!homeController.isAnimating && !homeController.popupOpen) {
				// Animate popup panel forward
				GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ().isAnimating = true;
				GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ().popupOpen = true;
				StartCoroutine(animateIn ());
			}
			audioEffects.transform.FindChild("Click").GetComponent<AudioSource>().Play();
		}
	}

	public override void end ()
	{
		// Loop through all boards to see which is the "current" board
		Transform parent = categories;
		
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

	IEnumerator animateIn()
	{
		popUp.SetActive (true);
		popUp.transform.FindChild ("Hover Button").GetComponent<BoxCollider> ().enabled = true;
		popUp.transform.FindChild ("ClosePopupButton").GetComponent<BoxCollider> ().enabled = true;

		// Set iamge of popUp to be the same as the panel image
		popUp.transform.FindChild("Image").GetComponent<RawImage> ().texture = transform.GetComponent<RawImage> ().texture;

		for (int i = 1; i <= animateInIntervals; i++) {
			// Animate alpha
			popUp.GetComponent<CanvasGroup>().alpha = easeInOut(1, animateInIntervals, i);
			
			// Animate position
			Vector3 newLocalPosition = popUp.transform.localPosition;
			newLocalPosition.z = easeInOut (animatedInPosition, animateInIntervals, i);
			popUp.transform.localPosition = newLocalPosition;
			
			yield return new WaitForSeconds (0.016f);
		}
		
		GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ().isAnimating = false;
		yield return new WaitForSeconds (0.01f);
	}

	public override void enterHover(Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerEnterHandler);
		audioEffects.transform.FindChild("Rollover").GetComponent<AudioSource>().Play();

	}
	
	public override void exitHover (Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerExitHandler);
	}
}
