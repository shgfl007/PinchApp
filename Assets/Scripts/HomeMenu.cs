using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class HomeMenu : ButtonBehaviour {


	//public GameObject featureModel;
	public Color highlightedColor, normalColor;
	Image img;
	public int sceneIndex;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	//	featureModel.SetActive (false);
		base.Start();
	}
	


	public override void enterHover (Cursor c)
	{
		img.color = highlightedColor;
		//featureModel.SetActive (true);
	//	AudioManager.instance.PlayAudio (0);

		base.enterHover (c);
	}


	public override void exitHover(Cursor c) {
	
		img.color = normalColor;
	//	featureModel.SetActive (false);

		base.exitHover (c);
	}
	public override void start(ref Cursor c) {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlayAudio (2);
		}
	}
	public override void end() {

		// go to desired Scene
		GoToxScene.instance.sceneIndex = sceneIndex;
		GoToxScene.instance.GoToScene ();

	}
}
