//Kitchen Sink
using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class IntroScript : VRObject
{

	GameObject everythingContainer;
	GameObject intro;
	bool introEnd = false;
	//check if intro ended
	bool canInvoke = true;
	//for deactivating intro canvas
	bool fadeOutCanvas = false;
	CanvasGroup cs, introGroup;
	public GameObject hm;

	// Use this for initialization
	void Start ()
	{
		everythingContainer = GameObject.Find ("Parth Canvas"); // find parth canvas
		cs = GameObject.Find ("Parth Canvas").GetComponent<CanvasGroup> (); // find parth canvas
		intro = GameObject.Find ("Intro"); // Get intro
		cs.alpha = 0;
		introGroup = intro.gameObject.GetComponent<CanvasGroup> ();
		hm.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (introEnd) {
			intro.transform.Translate (Vector3.forward * Time.deltaTime * 10);//animate it going forward
			//StartCoroutine(fadeOutCanvas());
			everythingContainer.SetActive (true);//activate everything container
			fadeOutCanvas = true;
		}
		if (fadeOutCanvas) {
			introGroup.alpha -= Time.deltaTime / 0.8f;

		}
	}

	public void disable ()
	{
		gameObject.transform.parent.parent.gameObject.SetActive (false);
	}

	IEnumerator fadeInCanvas ()
	{
		while (cs.alpha < 1) {
			cs.alpha += Time.deltaTime / 2f;
			yield return null;

		}
		FeatureManager.instance.IntroComplete ();
		//	print ("collider Ended");

		yield return null;
	}

	public override void start (ref Cursor cursor)
	{
		introEnd = true;
		if (canInvoke) {
			Invoke ("disable", 4f);
			StartCoroutine (fadeInCanvas ());
			hm.SetActive (true);
			canInvoke = false;
		}
	}



}
