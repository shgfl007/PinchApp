using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PinchToStartButton : ButtonBehaviour {

	public Color highlightedColor, clickedColor, normalColor;
	bool mUpdate = false;
	public Image img;

	public override void start(ref Cursor c) {
	
		img.color = clickedColor;
		base.start (ref c);
	}


	public override void enterHover (Cursor c)
	{
		img.color = highlightedColor;

		base.enterHover (c);
	}

	public override void exitHover(Cursor c){
		img.color = normalColor;

		base.exitHover (c);


	}

	public override void end() {

		GoToxScene.instance.GoToScene ();
		//Go to Next Scene
	}


}
