using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileScript : VRObject {

	bool newScrollPos = false;

	GameObject buttonHit;
	//bool hoveringButton = false;
	Vector3 moveOffset;
	public bool onBoard = false;

	void Start() {
		audioEffects = GameObject.Find ("AudioEffects");
	}

	public override void start (ref Cursor cursorInput)
	{
		single = cursorInput;
		mode = "single";

		newScrollPos = true;
	}

	public override void end ()
	{
	}

	public override void AfterUpdate()
	{
		if (onBoard) {
			if (newScrollPos) {
				newScrollPos = false;
				return;
			}

			// Do movement on board
			float yDifference = single.transform.localPosition.y - single.previousCursorPos.y;
			float xDifference = single.transform.localPosition.x - single.previousCursorPos.x;
			Vector3 localPositionVector = transform.parent.localPosition;
			localPositionVector.y = localPositionVector.y + yDifference * 8;
			localPositionVector.x = localPositionVector.x + xDifference * 8;
			transform.parent.localPosition = localPositionVector;
		}
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

	public override void dualCursorUpdate (ref Cursor left, ref Cursor right)
	{
		resizeAndRotateWithDualCursor(ref left, ref right, transform.parent, 1);
	}

}
