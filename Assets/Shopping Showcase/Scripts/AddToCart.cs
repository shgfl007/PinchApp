using UnityEngine;
using System.Collections;

public class AddToCart : VRObject {



	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void end() {
		UpdateCart.instance.updateText ();
	//	this.transform.parent.parent.gameObject.SetActive (false);
		this.transform.parent.parent.gameObject.GetComponent<ItemInfo>().shirtAnimation();
		this.transform.parent.gameObject.SetActive (false);
	}

}
