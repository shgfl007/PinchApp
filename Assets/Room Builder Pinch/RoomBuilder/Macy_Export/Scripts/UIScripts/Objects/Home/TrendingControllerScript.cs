using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrendingControllerScript : VRObject {

	Transform homeController;
	Transform trending;
	Transform lowerAestheticRing;
	List<Transform> transformsToToggle = new List<Transform>();
	List<Transform> trendingInterfaces = new List<Transform>();
	Color openColor, closeColor;
	public bool trendingOpen = false;

	void Start() {
		trending = transform.parent.parent;
		homeController = GameObject.Find ("Home Interface").transform;
		lowerAestheticRing = GameObject.Find ("Aesthetic Rings").transform.FindChild ("Lower");

		transformsToToggle.Add (homeController.FindChild ("Categories"));
		transformsToToggle.Add (homeController.FindChild ("BackBlocker"));
		//transformsToToggle.Add (homeController.FindChild ("HomeMenu").FindChild("Browse"));
		transformsToToggle.Add (homeController.parent.FindChild ("HomeSectionMarkers").FindChild("BrowseMarker"));

		// Get all trending interfaces
		for (int i = 0; i < trending.childCount; i++) {
			if (trending.GetChild(i).name != "TrendingController") {
				trendingInterfaces.Add(trending.GetChild(i));
			}
		}

		// Close all trending interfaces
		for (int i = 0; i < trendingInterfaces.Count; i++) {
			trendingInterfaces[i].gameObject.SetActive(false);
		}

		openColor = new Color (0.55f, 0.55f, 0.55f);
		closeColor = new Color (0.9f, 0.9f, 0.9f);

		transform.GetComponent<RawImage> ().color = closeColor;
	}

	public override void onClick ()
	{
		base.onClick ();

		if (!trendingOpen) {
			// Ensures that all trending interfaces are closed
			bool shouldClick = true;
			for (int i = 0; i < trendingInterfaces.Count; i++) {
				if (trendingInterfaces[i].GetComponent<CanvasGroup>().alpha != 0)
					shouldClick = false;
			}

			if (shouldClick) {
				// Open all trending interfaces
				for (int i = 0; i < trendingInterfaces.Count; i++) {
					StartCoroutine(fadeIn (trendingInterfaces[i], 30, true));
				}
				// Remove categories and backblocker and etc...
				for (int i = 0; i < transformsToToggle.Count; i++) {
					StartCoroutine(fadeOut (transformsToToggle[i], 30, true, 1));
				}

				trendingOpen = true;
				transform.GetComponent<RawImage> ().color = openColor;

				StartCoroutine(fadeOut(lowerAestheticRing));
			}
		}
		else {
			// Ensures that all trending interfaces are opened
			bool shouldClick = true;
			for (int i = 0; i < trendingInterfaces.Count; i++) {
				if (trendingInterfaces[i].GetComponent<CanvasGroup>().alpha != 1)
					shouldClick = false;
			}

			if (shouldClick) {
				// Close all trending interfaces
				for (int i = 0; i < trendingInterfaces.Count; i++) {
					StartCoroutine(fadeOut (trendingInterfaces[i], 30, true));
				}
				// Returns categories and backblocker and etc...
				for (int i = 0; i < transformsToToggle.Count; i++) {
					StartCoroutine(fadeIn (transformsToToggle[i], 30, true, 0));
				}

				trendingOpen = false;
				transform.GetComponent<RawImage> ().color = closeColor;

				StartCoroutine(fadeIn(lowerAestheticRing));
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
