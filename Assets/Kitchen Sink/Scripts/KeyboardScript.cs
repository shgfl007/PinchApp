using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class KeyboardScript : VRObject {

	KeyboardMaster keyboardMasterScript;
	GameObject text;
	Color white, dark;
	bool click = true;


	public override void start (ref Cursor cursorInput)
	{
		single = cursorInput;
		mode = "single";
		click = true;

		// Show click
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.submitHandler);
	}

	public override void end ()
	{
		if (click) {

			// Do typing
			string keyboardButtonString = transform.parent.FindChild ("Text").GetComponent<Text> ().text;
			switch (keyboardButtonString) {
			case "del":
                    KeyboardMaster.instance.backspace ();
				break;
			case "123#":
				break;
			case "shft":
				break;
			case "space":
                    KeyboardMaster.instance.addLetter (" ");
				break;
			default:
                    KeyboardMaster.instance.addLetter (keyboardButtonString);
				break;
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
