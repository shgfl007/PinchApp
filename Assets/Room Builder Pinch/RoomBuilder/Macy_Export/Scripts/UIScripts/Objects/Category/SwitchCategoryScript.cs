using UnityEngine;
using System.Collections;

public class SwitchCategoryScript : VRObject {

	Transform category;

	// Use this for initialization
	void Start () {
		category = transform.parent.parent.FindChild("Category Interface");
		audioEffects = GameObject.Find ("AudioEffects");
	}
	
	public override void onClick ()
	{
		base.onClick ();
		audioEffects.transform.FindChild("Click").GetComponent<AudioSource>().Play();

		GameObject.Find ("EverythingContainer").GetComponent<EverythingContainerScript> ().goToGameObject (category.gameObject);
	}
}
