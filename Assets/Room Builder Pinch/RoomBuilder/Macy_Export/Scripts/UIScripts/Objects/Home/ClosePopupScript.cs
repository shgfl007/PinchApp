using UnityEngine;
using System.Collections;

public class ClosePopupScript : VRObject {

	//float animateOutPosition = 0;
	//float animatedInPosition;
	float animateInIntervals;
	public bool animatingOut = false;


	// Use this for initialization
	void Start () {
		HomeControllerScript homeController = GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ();
		//animatedInPosition = homeController.animatedInPosition;
		animateInIntervals = homeController.popupAnimationIntervals;
		audioEffects = GameObject.Find ("AudioEffects");
	}

	public override void onClick ()
	{
		base.onClick ();
		audioEffects.transform.FindChild("Close").GetComponent<AudioSource>().Play();

		onClickWrapper ();
	}

	public void onClickWrapper(bool goToCategory = false) {
		if (!animatingOut) {
			StartCoroutine (animateOut (goToCategory));
			GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ().popupOpen = false;
			
			transform.parent.FindChild ("Hover Button").GetComponent<BoxCollider> ().enabled = true;
			transform.parent.FindChild ("ClosePopupButton").GetComponent<BoxCollider> ().enabled = true;
		}
	}

	IEnumerator animateOut(bool goToCategory)
	{
		GameObject popUp = transform.parent.parent.FindChild("PopupPanel").gameObject;
		popUp.transform.FindChild ("Hover Button").GetComponent<BoxCollider> ().enabled = true;
		
		for (int i = 1; i <= animateInIntervals; i++) {
			// Animate alpha
			popUp.GetComponent<CanvasGroup>().alpha = 1 - easeInOut(1, (int) animateInIntervals, i);
			
			// Animate position
			/*
			Vector3 newLocalPosition = popUp.transform.localPosition;
			newLocalPosition.z = animatedInPosition - easeInOut (animatedInPosition, (int) animateInIntervals, i);
			popUp.transform.localPosition = newLocalPosition;
			*/
			
			yield return new WaitForSeconds (0.016f);
		}

		// Snap position
		Vector3 newLocalPosition = popUp.transform.localPosition;
		newLocalPosition.z = 0;
		popUp.transform.localPosition = newLocalPosition;

		GameObject.Find ("Home Interface").GetComponent<HomeControllerScript> ().isAnimating = false;
		popUp.SetActive (false);

		if (goToCategory) {
			GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (transform.parent.parent.FindChild("Category Interface").gameObject);
		}
		yield return new WaitForSeconds (0.01f);
	}
}
