using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HomeMenuScript : VRObject {

	GameObject everythingContainer;
	Transform homeSections;
	GameObject trendingController;
	//bool trendingEnded = false;
	GameObject homeInterface;

	void Start() {
		everythingContainer = GameObject.Find ("EverythingContainer");
		homeSections = transform.parent.parent.FindChild ("HomeSectionMarkers");
		trendingController = GameObject.Find ("TrendingControllerButton");
		homeInterface = GameObject.Find ("Home Interface");
		audioEffects = GameObject.Find ("AudioEffects");
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

	public override void onClick ()
	{
		base.onClick ();

		string name = transform.name;

		switch (name) {
		case "Search":
			everythingContainer.GetComponent<EverythingContainerScript>().goToGameObject(homeSections.FindChild("SearchMarker").gameObject);
			break;
		case "Trending":
			everythingContainer.GetComponent<EverythingContainerScript>().goToGameObject(transform.parent.parent.FindChild("Home Interface").gameObject); // Home Interface
			if (!trendingController.GetComponent<TrendingControllerScript> ().trendingOpen)
				trendingController.GetComponent<TrendingControllerScript>().onClick();
			break;
		case "Highlights":
			turnOffTrending();
			everythingContainer.GetComponent<EverythingContainerScript>().goToGameObject(transform.parent.parent.FindChild("Home Interface").gameObject); // Home Interface
			break;
		case "Browse":
			turnOffTrending();
			everythingContainer.GetComponent<EverythingContainerScript>().goToGameObject(homeSections.FindChild("BrowseMarker").gameObject);
			break;
		case "Friends":
			turnOffTrending();
			everythingContainer.GetComponent<EverythingContainerScript>().goToGameObject(homeSections.FindChild("FriendsMarker").gameObject);
			break;
		case "Music":
			turnOffTrending();
			everythingContainer.GetComponent<EverythingContainerScript>().goToGameObject(homeSections.FindChild("MusicMarker").gameObject);
			break;
		default:
			break;
		}

		audioEffects.transform.FindChild("Click").GetComponent<AudioSource>().Play();

		// Force home interface to update
		homeInterface.GetComponent<HomeControllerScript> ().currentHomeMarker = null;
	}

	void turnOffTrending() {
		if (trendingController.GetComponent<TrendingControllerScript> ().trendingOpen) {
			trendingController.GetComponent<TrendingControllerScript>().onClick();
		}
	}

}
