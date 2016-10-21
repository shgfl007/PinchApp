using UnityEngine;
using System.Collections;

public class ColorPicker : ButtonBehaviour {

	private static ColorPicker mInstance;
	public static ColorPicker instance { get { return mInstance; } }

	public GameObject colorPicker;
	public bool isOn;
	// Use this for initialization
	void Start () {
		if(mInstance == null){
			mInstance = this;
		}
		base.Start ();
	}


	public override void end() {

		isOn = !isOn;
		toggleColors (isOn);

	}

	public void toggleColors(bool b) {
		colorPicker.SetActive (b);

	}


}
