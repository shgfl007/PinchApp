using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenContentScript : VRObject {

	static public bool openContent(GameObject parent, float dist, string mode, string videoURL = "", Texture image = null)
	{
		EverythingContainerScript everythingContainerScript = GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ();

		if (!everythingContainerScript.isAnimating && !everythingContainerScript.contentOpen) {
			// Create new content interface
			GameObject contentInterface = (GameObject) Instantiate (Resources.Load ("Content Interface"));
			contentInterface.transform.SetParent (parent.transform);
			contentInterface.GetComponent<CanvasGroup> ().alpha = 0;

			// Set transform to neutral
			contentInterface.transform.localPosition = Vector3.zero;
			contentInterface.transform.localRotation = Quaternion.Euler(Vector3.zero);
			contentInterface.transform.localScale = new Vector3 (1, 1, 1);

			// Move position to be in front of parent
			Vector3 newPosition = contentInterface.transform.localPosition;
			newPosition.z += dist;
			contentInterface.transform.localPosition = newPosition;

			// Set parent to stationary objects
			contentInterface.transform.SetParent (GameObject.Find("StationaryObjects").transform);

			// Fade everything else
			GameObject everythingContainer = GameObject.Find ("EverythingContainer");
			EverythingContainerScript everythingScript = everythingContainer.GetComponent<EverythingContainerScript> ();
			everythingScript.mTween.tween (everythingContainer, "alpha", 0.4f, 0.4f, 0, Ease.Linear, false);

			// Fade in content
			//everythingScript.mTween.tween (contentInterface, "alpha", 1, 0.4f, 0, Ease.Linear, false);

			// Move to content
			GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (contentInterface);

			// Remove back blocker
			GameObject backBlocker = GameObject.Find("BackBlocker");
			if (backBlocker != null)
				backBlocker.GetComponent<CanvasGroup>().alpha = 0;

			// Move content
			everythingScript.simultaneousMoveTo (contentInterface, Vector3.zero);

			// Set content mode
			switch (mode) {
			case "Video":
				contentInterface.transform.FindChild("CenterUI").FindChild("ScrollRect").FindChild("ArticleContent").gameObject.SetActive(true);
				contentInterface.transform.FindChild("CenterUI").FindChild("Scrollbar").gameObject.SetActive(true);
				break;
			case "Image":
				contentInterface.transform.FindChild("CenterUI").FindChild("ScrollRect").FindChild("PhotoContent").gameObject.SetActive(true);
				contentInterface.transform.FindChild("CenterUI").FindChild("ScrollRect").FindChild("PhotoContent").FindChild("Image").GetComponent<RawImage>().texture = image;
				break;
			case "Article":
				contentInterface.transform.FindChild("CenterUI").FindChild("ScrollRect").FindChild("ArticleContent").gameObject.SetActive(true);
				contentInterface.transform.FindChild("CenterUI").FindChild("Scrollbar").gameObject.SetActive(true);
				break;
			default:
				break;
			}
			contentInterface.transform.FindChild("CenterUI").FindChild("Title").GetComponent<Text>().text = mode;

			GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().contentOpen = true;

			return true;
		}
		return false;
	}

	public override void end ()
	{
		openContent (transform.parent.gameObject, 10, "Video");
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
