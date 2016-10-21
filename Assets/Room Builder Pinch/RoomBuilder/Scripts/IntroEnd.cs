using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class IntroEnd : VRObject {


	bool mUpdate = false;

	public CanvasGroup cg;
	//Color titleColor, imageColor, bgColor;
	float cgAlpha;
	public int stateToChange;

	public bool isButton = false;

	public Color normalColor    = Color.white;
	public Color hoverColor     = new Color32(156, 244, 167, 255);
	public Image targetImage;



	// Use this for initialization
	void Start () {


		cgAlpha = cg.alpha;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.A)) {

			mUpdate = true;
		}

		if(mUpdate){

			updateAlpha ();
		}

	}

	public override void enterHover(Cursor c)
	{
		base.enterHover(c);

		if (isButton) {

			if (targetImage) {
				targetImage.color = hoverColor;
			}

		}
	}

	public override void exitHover(Cursor c)
	{
		if (isButton) {
			
			if (targetImage) {
				targetImage.color = normalColor;
			}

		}
		base.exitHover(c);
	}

	void updateAlpha () {

		if (cgAlpha > 0.0f) {

			cgAlpha -= 0.01f;
			cg.alpha = cgAlpha;

		} else {

			SpaceManager.instance.sceneState = stateToChange;
			SpaceManager.instance.changeSceneState ();
		}


	}



	public override void end (){
		cgAlpha = cg.alpha;

		mUpdate = true;

	}

}
