using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class UpdateCart : MonoBehaviour {

	private static UpdateCart mInstance;
	public static UpdateCart instance {get{return mInstance;}}
	Text shop;
	public int no= 0;

	private GameObject g;
	private Animator a1;


	// Use this for initialization
	void Start () {
		if (mInstance == null) {

			mInstance = this;
		}
	//	g = GameObject.Find ("ShoppingCart");
	//	a1 = g.GetComponent<Animator> ();
	//	//
	//	shop = GetComponent<Text> ();
	}
	

	public void updateText () {

		no++;
		//shop.text = no.ToString ();
	//	ToggleLighting.instance.lightBack();


	//	a1.Play ("ShopGrow", -1, 0f);

	}

}
