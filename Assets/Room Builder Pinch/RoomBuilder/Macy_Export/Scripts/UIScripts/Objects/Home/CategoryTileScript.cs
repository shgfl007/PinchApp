using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CategoryTileScript : VRObject {

	//bool click = true;

	void Start() {
		audioEffects = GameObject.Find ("AudioEffects");
	}

	public override void onClick ()
	{
		base.onClick ();
		audioEffects.transform.FindChild("Click").GetComponent<AudioSource>().Play();

		//GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (transform.parent.parent.FindChild("Category Interface").gameObject);
		//transform.parent.FindChild ("ClosePopupButton").GetComponent<ClosePopupScript> ().onClick ();

		// New animation
		StartCoroutine (goToCategory ());
	}

	IEnumerator goToCategory()
	{
		Transform closePopupButton = transform.parent.FindChild ("ClosePopupButton");
		closePopupButton.GetComponent<ClosePopupScript> ().onClickWrapper (true);

		//GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (transform.parent.parent.FindChild("Category Interface").gameObject);
		yield return new WaitForSeconds (0.1f);
	}

	public override void enterHover(Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerEnterHandler);
		//audioEffects.transform.FindChild("Rollover").GetComponent<AudioSource>().Play(); // Awkward sound
	}
	
	public override void exitHover (Cursor c)
	{
		PointerEventData pointer = new PointerEventData (EventSystem.current); // pointer event for Execute
		ExecuteEvents.Execute (gameObject, pointer, ExecuteEvents.pointerExitHandler);
	}

}