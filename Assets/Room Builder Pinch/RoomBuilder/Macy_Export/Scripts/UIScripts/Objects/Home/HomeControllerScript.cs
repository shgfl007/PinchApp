using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HomeControllerScript : VRObject {
	
	public bool isAnimating = false;
	public bool popupOpen = false;
	public float animatedInPosition = -720;
	public int popupAnimationIntervals = 20;
	GameObject homeMenu;
	int homeMenuOriginalSortingOrder;
	int numMarkers;
	GameObject everythingContainer;
	float camFieldOfViewHalf = 70;

	public Transform homeInterface, searchInterface, backBlocker, chatheads, currentHomeMarker, markers, categories, friends, music, trendingController, trendingInterface;

	bool isCurrentlyTrending;
	GameObject cam;

	// Use this for initialization
	void Start () {
		// Add home interfaces
		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().managedInterfaces.Add(GameObject.Find("SearchMarker"));
		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().managedInterfaces.Add(GameObject.Find("BrowseMarker"));
		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().managedInterfaces.Add(GameObject.Find("FriendsMarker"));
		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().managedInterfaces.Add(GameObject.Find("MusicMarker"));

		homeInterface = GameObject.Find ("Home Interface").transform;
		trendingController = GameObject.Find ("TrendingControllerButton").transform;
		trendingInterface = trendingController.parent.parent;
		searchInterface = transform.parent.FindChild ("Search");
		searchInterface.gameObject.SetActive (false);
		markers = transform.parent.FindChild ("HomeSectionMarkers");
		homeMenu = GameObject.Find ("HomeMenu");
		homeMenuOriginalSortingOrder = homeMenu.GetComponent<Canvas> ().sortingOrder;
		numMarkers = transform.parent.FindChild("HomeSectionMarkers").childCount;
		currentHomeMarker = null;
		everythingContainer = GameObject.Find ("EverythingContainer");
		cam = GameObject.Find ("CenterEyeAnchor").gameObject;
		categories = transform.FindChild ("Categories");
		friends = transform.FindChild ("Friends");
		music = transform.FindChild ("Music");
		backBlocker = transform.FindChild ("BackBlocker");
		chatheads = transform.FindChild ("Chathead Container");
	}
	
	// Update is called once per frame
	void Update () {
		//categoryCulling ();

		// Check which of the interfaces is closest to Vector3.zero
		float minDist = 1000000;
		Transform closestMarker = null;
		isCurrentlyTrending = trendingController.GetComponent<TrendingControllerScript> ().trendingOpen;

		for (int i = 0; i < numMarkers; i++)
		{
			Transform marker = markers.GetChild(i);
			marker.gameObject.SetActive(true);
			float dist = marker.position.magnitude;
			if (dist < minDist) {
				minDist = dist;
				closestMarker = marker;
			}
		}

		if (closestMarker != null && currentHomeMarker != closestMarker && !everythingContainer.GetComponent<EverythingContainerScript>().isAnimating) {
			currentHomeMarker = closestMarker;
			StartCoroutine(updateMarker());
			//Debug.Log("Changed Home Marker to " + currentHomeMarker.name);
		}
	}

	IEnumerator updateMarker() {
		float originalYPos = homeMenu.transform.position.y;
		float originalLocalYPos = homeMenu.transform.localPosition.y;
		float difference = currentHomeMarker.position.y - originalYPos;
		int intervals = 20;

		updateOpacities ();

		// Change highlighting of homeMenu
		for (int i = 0; i < homeMenu.transform.childCount; i++) {
			Transform child = homeMenu.transform.GetChild(i);
			
			if (isCurrentlyTrending && !(currentHomeMarker.name == "SearchMarker") ?
			    child.FindChild("Text").GetComponent<Text>().text == "Trending" :
			    child.GetSiblingIndex() == currentHomeMarker.GetSiblingIndex()) {
				//child.GetComponent<Button>().image.color = new Color(0.35f, 0.35f, 0.35f);
				child.FindChild("Text").GetComponent<Text>().color = new Color(1, 1, 1);

				// Change button color
				ColorBlock cb = child.GetComponent<Button>().colors;
				cb.normalColor = new Color(0.35f, 0.35f, 0.35f);
				child.GetComponent<Button>().colors = cb;
			}
			else {
				//child.GetComponent<Button>().image.color = new Color(1, 1, 1, 0);
				child.FindChild("Text").GetComponent<Text>().color = new Color(0.35f, 0.35f, 0.35f);

				// Change button color
				ColorBlock cb = child.GetComponent<Button>().colors;
				cb.normalColor = new Color(1, 1, 1, 0);
				child.GetComponent<Button>().colors = cb;
			}
		}

		// Animate homeMenu into position
		for (int i = 1; i <= intervals; i++) {
			Vector3 newPosition = homeMenu.transform.localPosition;
			newPosition.y = originalLocalYPos + easeInOut(difference, intervals, i);
			homeMenu.transform.localPosition = newPosition;

			//Debug.Log(newPosition.y);

			yield return new WaitForSeconds(0.016f);
		}
	}

	void updateOpacities() {
		//Debug.Log ("Updating Opacities");
		bool fadeInSearch = currentHomeMarker.name == "SearchMarker";
		bool fadeInHighlights = currentHomeMarker.name == "HighlightsMarker";
		bool fadeInBrowse = currentHomeMarker.name == "BrowseMarker";
		bool fadeInFriends = currentHomeMarker.name == "FriendsMarker";
		bool fadeInMusic = currentHomeMarker.name == "MusicMarker";

		// Set opacity of Search
		if (fadeInSearch) {
			StartCoroutine(fadeInForSearch (searchInterface));
			return;
		}
		else {
			StartCoroutine(fadeOutForSearch (searchInterface));
		}

		// For each category
		for (int i = 0; i < categories.childCount; i++) {
			Transform category = categories.GetChild(i);

			// Set opacity of Highlights
			if (fadeInHighlights) {
				StartCoroutine(fadeIn (category.FindChild("Canvas").FindChild("Panel")));
				category.FindChild("Canvas").FindChild("Panel").FindChild("Mask").FindChild("Image").GetComponent<BoxCollider>().enabled = true;
			}
			else {
				StartCoroutine(fadeOut (category.FindChild("Canvas").FindChild("Panel")));
				category.FindChild("Canvas").FindChild("Panel").FindChild("Mask").FindChild("Image").GetComponent<BoxCollider>().enabled = false;
			}

			// Set opacity of Browse
			if (fadeInBrowse) {
				StartCoroutine(fadeIn (category.FindChild("Canvas").FindChild("Channels")));
				category.FindChild("Canvas").FindChild("Channels").FindChild("ScrollRect").FindChild("Content").
					FindChild("UI Image").GetComponent<BoxCollider>().enabled = true;
			}
			else {
				StartCoroutine(fadeOut (category.FindChild("Canvas").FindChild("Channels")));
				category.FindChild("Canvas").FindChild("Channels").FindChild("ScrollRect").FindChild("Content").
					FindChild("UI Image").GetComponent<BoxCollider>().enabled = false;
			}
		}

		// Friends
		if (fadeInFriends) {
			StartCoroutine(fadeIn(friends, 20, true));
		}
		else {
			StartCoroutine(fadeOut(friends, 20, true));
		}

		// Music
		if (fadeInMusic) {
			StartCoroutine(fadeIn(music, 20, true));
		}
		else {
			StartCoroutine(fadeOut(music, 20, true));
		}
	}

	IEnumerator fadeOut(Transform toFade) {
		int intervals = 20;
		if (toFade.GetComponent<CanvasGroup> ().alpha == 1) {
			for (int i = 1; i <= intervals; i++) { 
				toFade.GetComponent<CanvasGroup> ().alpha = 1 - easeInOut (0.5f, intervals, i);
				yield return new WaitForSeconds (0.016f);
			}
		}
		yield return new WaitForSeconds (0.01f);
	}

	IEnumerator fadeIn(Transform toFade) {
		int intervals = 20;
		if (toFade.GetComponent<CanvasGroup> ().alpha == 0.5f) {
			for (int i = 1; i <= intervals; i++) { 
				toFade.GetComponent<CanvasGroup> ().alpha = 0.5f + easeInOut (0.5f, intervals, i);
				yield return new WaitForSeconds (0.016f);
			}
		}
		yield return new WaitForSeconds (0.01f);
	}

	IEnumerator fadeOutForSearch(Transform toFade) {
		int intervals = 20;
		if (toFade.GetComponent<CanvasGroup> ().alpha == 1) {
			StartCoroutine(fadeIn(homeInterface, intervals));
			for (int i = 1; i <= intervals; i++) { 
				toFade.GetComponent<CanvasGroup> ().alpha = 1 - easeInOut (1, intervals, i);

				homeMenu.GetComponent<Canvas>().sortingOrder = homeMenuOriginalSortingOrder;
				yield return new WaitForSeconds (0.016f);
			}
		}
		searchInterface.FindChild ("SearchBar").gameObject.SetActive (false);
		searchInterface.FindChild ("Ad").gameObject.SetActive (false);
		searchInterface.gameObject.SetActive (false);
		yield return new WaitForSeconds (0.01f);
	}
	
	IEnumerator fadeInForSearch(Transform toFade) {
		int intervals = 20;
		searchInterface.gameObject.SetActive (true);
		searchInterface.FindChild ("SearchBar").gameObject.SetActive (true);
		searchInterface.FindChild ("Ad").gameObject.SetActive (true);

		if (toFade.GetComponent<CanvasGroup> ().alpha == 0) {
			StartCoroutine(fadeOut(homeInterface, intervals));
			for (int i = 1; i <= intervals; i++) { 
				toFade.GetComponent<CanvasGroup> ().alpha = easeInOut (1, intervals, i);

				homeMenu.GetComponent<Canvas>().sortingOrder = 0;
				yield return new WaitForSeconds (0.016f);
			}
		}
		yield return new WaitForSeconds (0.01f);
	}

	void categoryCulling() {
		string current = everythingContainer.GetComponent<EverythingContainerScript> ().current.name;
		if (current == "Home Interface" || current == "SearchMarker" || current == "HighlightsMarker" || current == "BrowseMarker") {
			// Get camera forward vector
			Vector3 camForward = cam.transform.forward;
			camForward.y = 0;
			
			// For each category
			for (int i = 1; i < categories.childCount; i++) {
				Transform category = categories.GetChild (i);
				Transform categoryPanel = category.FindChild ("Canvas").FindChild ("Panel");
				// Get category position vector relative to cam
				Vector3 categoryPosRelCam = categoryPanel.position - cam.transform.position;
				
				if (Vector3.Angle (camForward, categoryPosRelCam) <= camFieldOfViewHalf || camForward.magnitude == 0) {
					category.gameObject.SetActive (true);
				} else {
					category.gameObject.SetActive (false);
				}
			}
		}
	}
}
