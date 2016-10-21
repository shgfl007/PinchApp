using UnityEngine;
using System.Collections;

public class OpenShopingCart : ButtonBehaviour {

	public GameObject store;
	public GameObject shopPage;
	public bool isOpen = true;
	public GameObject whiteDrop;
	public GameObject specialPanel;
	public GameObject title;

	public GameObject whiteDropOUT;


	private Transform changeCamPos;
	public GameObject cm;


	public override void end() {
	//	if (UpdateCart.instance.no > 0) {
			isOpen = !isOpen;

			if (!isOpen) {
				toggleStore ();
			} else {
				offStore ();
			}

		ShopChangeCamPos.instance.canMove = false;
		//}
		ShowcaseUIManager.instance.turnOffUI ();

	}

	public void offStore() {
		//	shopPage.SetActive (!isOpen);
		whiteDrop.SetActive (false);
		shopPage.SetActive (false);
		specialPanel.SetActive (false);
		title.SetActive (false);

		StartCoroutine (fadeOutThings());


	}

	public void toggleStore () {

		Follow.instance.ChangePos ();
		//changeCamPos.transform.position = cm.transform.position;
		ToggleLighting.instance.lightBack();
		StartCoroutine (slidePanel());
	//	shopPage.SetActive (!isOpen);
	//	whiteDrop.SetActive (!isOpen);

	}

	IEnumerator fadeOutThings() {
		whiteDropOUT.SetActive (true);
		store.SetActive (isOpen);
	//	cm.transform.position = changeCamPos.transform.position;

		yield return new WaitForSeconds (0.5f);

		whiteDropOUT.SetActive (false);


	}

	IEnumerator slidePanel() {
	//	yield return new WaitForSeconds (0.5f);
		store.SetActive (isOpen);
		//yield return new WaitForSeconds (0.1f);
	//	whiteDrop.SetActive (isOpen);
	//	yield return new WaitForSeconds (0.1f);
		title.SetActive (true);

		shopPage.SetActive (true);
		yield return null;
	}



}
