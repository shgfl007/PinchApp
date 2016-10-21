using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ObjectDropper : VRObject {

	GameObject newObject;

	// Parameters
	float maxSpeedNoScroll = 1.2f;
	float toNewObjectCounter = 0;
	bool keepCounting = true;
	bool createObjectPhase = false;
	bool newObjectMade = false;

	void Start() {
		audioEffects = GameObject.Find ("AudioEffects");
	}
	
	public override void start (ref Cursor cursorInput)
	{
		single = cursorInput;
		mode = "single";

		// Reset parameters
		toNewObjectCounter = 0;
		keepCounting = true;
		newObjectMade = false;
		createObjectPhase = false;
	}

	public override void end ()
	{
		if (newObjectMade) {
			newObject.GetComponent<BoxCollider> ().enabled = true;
		}
	}

	public override void AfterUpdate()
	{
		// State machine controller for creating a new object
		if (single.verticalSpeed < maxSpeedNoScroll && single.horizontalSpeed < maxSpeedNoScroll && keepCounting) {
			toNewObjectCounter += Time.deltaTime;
			if (toNewObjectCounter > 0.5f) { // If held still for over 0.5 seconds, initiate object creation
				createObjectPhase = true;
				toNewObjectCounter = 0;
			}
			else {
				return;
			}
		}
		else {
			keepCounting = false;
		}

		if (!createObjectPhase) {
			// Do scrolling
			verticalScroll(transform.parent.parent.gameObject, 5f, false);
		}
		else {
			// Create the new tile only once
			if (!newObjectMade) {
				// Instantiate new car fab
				Vector3 hitPoint = single.hit.point;
				newObject = (GameObject) Instantiate (Resources.Load ("car"), hitPoint, Quaternion.Euler(Vector3.zero));
				newObject.GetComponent<BoxCollider> ().enabled = false;
				newObjectMade = true;
			}

			// Move car
			if (single.hit.transform.tag == "Floor" || single.hit.transform.tag == "DualCursor") {
				Vector3 hitPoint = single.hit.point;
				newObject.transform.position = hitPoint;
			}
			else {
				newObject.transform.position = single.transform.position * 50;
			}

		}
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
