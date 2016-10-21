using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class FadeInCanvases : MonoBehaviour {


	public CanvasGroup cg;
	//Color titleColor, imageColor, bgColor;
	float cgAlpha;
	public bool fadeInCanvas = false;

	// Use this for initialization
	void Start () {

		cgAlpha = cg.alpha;
	}

	// Update is called once per frame
	void Update () {

		if(fadeInCanvas){

			updateAlpha ();

		}

	}

	void updateAlpha () {

		if (cgAlpha < 1.0f) {

			cgAlpha += 0.01f;
			cg.alpha = cgAlpha;


		} 


	}



}
