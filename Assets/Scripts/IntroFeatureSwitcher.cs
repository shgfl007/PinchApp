using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class IntroFeatureSwitcher : ButtonBehaviour {

	public Color highlightedColor, clickedColor, normalColor, disabledColor;
	bool mUpdate = false;
	Image img;
	public bool disabled = false;

	public bool increment = true;


	void Start() {

		img = GetComponent<Image> ();
		if (disabled) {
			disableButton ();
		} else {
		
			enableButton ();
		}
		gameObject.SetActive (false);
		base.Start ();
	}


	public void disableButton () {
	
		disabled = true;
		img.color = disabledColor;

	}

	public void enableButton () {
	
		disabled = false;
		img.color = normalColor;
	
	}



	public override void start(ref Cursor c) {

		if(!disabled)
			img.color = clickedColor;

		base.start (ref c);

	}


	public override void enterHover (Cursor c)
	{
		if(!disabled)
			img.color = highlightedColor;



		base.enterHover (c);
	}

	public override void exitHover(Cursor c){
		if(!disabled)
			img.color = normalColor;

		base.exitHover (c);


	}

	public override void end() {

		if (increment && !disabled) {
		
			IntroManager.instance.increaseIndex ();

		} else if (!increment && !disabled){
		
			IntroManager.instance.decreaseIndex ();
		}

		gameObject.SetActive (false);

		//Go to Next Scene
	}





}
