using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CategoryHomeScript : VRObject {

	public CategoryTileScript categoryTileScript;
	Transform content;
	//bool newScrollPos = false;

	public override void end ()
	{
		audioEffects.transform.FindChild("Click").GetComponent<AudioSource>().Play();
		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (GameObject.Find ("Home Interface"));
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
