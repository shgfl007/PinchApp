using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ButtonHover : VRObject {
	public Color normalColor    = Color.white;
	public Color hoverColor     = new Color32(156, 244, 167, 255);
	public Image targetImage;

	// Use this for initialization
	void Start () {
	
		targetImage = GetComponent<Image> ();
		//targetRender = GetComponent<SpriteRenderer> ();
	}

	public override void enterHover(Cursor c){

		if (targetImage) {
		
			targetImage.color = hoverColor;
		}
	}
	public override void exitHover(Cursor c){

		if (targetImage) {

			targetImage.color = normalColor;
		}
	}

	public override void end(){
	
		//Debug.Log ("Clicked on the Pin");
	}

	// Update is called once per frame
	void Update () {
	
	}


}
