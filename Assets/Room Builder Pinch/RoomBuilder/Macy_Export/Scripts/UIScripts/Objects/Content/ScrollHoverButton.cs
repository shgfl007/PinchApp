using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScrollHoverButton : VRObject {

	//bool click = true;
	bool newScrollPos = false;
	//bool scrolling = false;
	Transform content;
	//float maxSpeedNoScroll = 1.4f;

	public override void start (ref Cursor cursorInput)
	{
		single = cursorInput;
		mode = "single";
		//click = true;
		content = null;
		newScrollPos = true;
		
		//scrolling = true;
	}

	public override void end ()
	{
		//scrolling = false;
	}

	public override void AfterUpdate()
	{
		if (newScrollPos) {
			newScrollPos = false;
			return;
		}
		
		//if (single.verticalSpeed > 1.7f) {
		//	click = false;
		//}

		// Do scroll of parent
		if (content == null) {
			content = single.heldTransform.parent;
		}
		verticalScroll (content.gameObject, 3, false);
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
