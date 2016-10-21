using UnityEngine;
using System.Collections;

public class CloseContentScript : VRObject {

	public override void end ()
	{
		GameObject contentInterface = transform.parent.parent.gameObject;
		GameObject everythingContainer = GameObject.Find ("EverythingContainer");
		EverythingContainerScript everythingScript = everythingContainer.GetComponent<EverythingContainerScript> ();

		if (!everythingScript.isAnimating && everythingScript.contentOpen) {
			audioEffects.transform.FindChild("Close").GetComponent<AudioSource>().Play();

			// Fade everything back in
			everythingScript.mTween.tween (everythingContainer, "alpha", 1, 0.4f, 0, Ease.Linear, false);

			// Fade back out
			everythingScript.mTween.tween (contentInterface, "alpha", 0, 0.4f, 0, Ease.Linear, false);
			everythingScript.mTween.tween (contentInterface, "position",
			                               contentInterface.transform.position + contentInterface.transform.forward * 40,
			                               0.4f, 0, Ease.Linear, false);

			// Move back
			GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goBack ();

			// Destroy object
			StartCoroutine (waitThenDestroy (contentInterface));
			StartCoroutine(unDimCameraBackground());

			// Return back blocker
			GameObject backBlocker = GameObject.Find("BackBlocker");
			if (backBlocker != null)
				backBlocker.GetComponent<CanvasGroup>().alpha = 1;

			everythingContainer.GetComponent<EverythingContainerScript> ().contentOpen = false;
		}
	}

	IEnumerator waitThenDestroy(GameObject contentInterface) {
		yield return new WaitForSeconds (1);
		GameObject.Destroy (contentInterface);
	}
}
