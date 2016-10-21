using UnityEngine;
using System.Collections;

public class ToggleItems : VRObject {

	public GameObject specialPanel;

	public GameObject shoes;
	public bool isShowing = false;
	public GameObject toToggle;

	public GameObject goTo;



	// Use this for initialization
	void Start () {
	
	}

	public override void end ()
	{
		isShowing = !isShowing;
		toggleViews ();
	}

	// Update is called once per frame
	void Update () {
	
	}


	void toggleViews () {
		//shoes.SetActive (isShowing);
		specialPanel.SetActive(true);
		StartCoroutine (offOthers());

		//if(goTo != null){
			ShopChangeCamPos.instance.goToSelectItem (goTo.transform.position, goTo.transform.rotation);

		//}
	}
	IEnumerator offOthers () {
	
		yield return new WaitForSeconds (0.5f);
		toToggle.gameObject.SetActive (false);

	}

}
