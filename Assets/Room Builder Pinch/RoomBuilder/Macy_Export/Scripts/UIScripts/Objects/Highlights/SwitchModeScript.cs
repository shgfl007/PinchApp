using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SwitchModeScript : VRObject {

	bool atBubble = false;

	public override void end ()
	{
		atBubble = atBubble ? false : true;

		Vector3 bubbleLocation = transform.parent.parent.parent.FindChild("FlyThrough").position;
		Vector3 carouselLocation = transform.parent.parent.parent.FindChild("Carousel").position;
		
		// Swap positions
		GameObject.Find("EverythingContainer").GetComponent<EverythingContainerScript>().simultaneousMoveTo (
			transform.parent.parent.parent.FindChild("FlyThrough").gameObject, carouselLocation);
		GameObject.Find("EverythingContainer").GetComponent<EverythingContainerScript>().simultaneousMoveTo (
			transform.parent.parent.parent.FindChild("Carousel").gameObject, bubbleLocation);

		// Swap alphas
		GameObject.Find("EverythingContainer").GetComponent<EverythingContainerScript>().mTween.tween (
			transform.parent.parent.parent.FindChild("FlyThrough").gameObject, "alpha", (atBubble ? 1 : 0.5f), 0.5f, 0, Ease.Linear, false);
		GameObject.Find("EverythingContainer").GetComponent<EverythingContainerScript>().mTween.tween (
			transform.parent.parent.parent.FindChild("Carousel").gameObject, "alpha", (atBubble ? 0.5f : 1f), 0.5f, 0, Ease.Linear,false);

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
