using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class IntroManager : MonoBehaviour {
	private static IntroManager mInstance;
	public static IntroManager instance { get { return mInstance; } }
	public IntroCube ic;
	public bool featureComplete;
	public string[] str;
	public Text info;
	public IntroFeatureSwitcher nextArrow;//previousArrow, nextArrow;
	public Image[] sliderDots;
	Color tempColor;
	private Color sdColor;
	public Text tx;

	int index = 0;
	// Use this for initialization
	void Start () {

		if (mInstance == null)
		{
			mInstance = this;
		}

		sdColor = Color.white;
		sdColor.a = 0.3f;
		info.text = str[index];

	}
	
	// Update is called once per frames
	void Update () {
	
	}



	public void updateFeatures() {
	

		/*
		//disable the next button if the slide reached its right end
		if (index == 6) {
			nextArrow.disableButton();
		} else {
			nextArrow.enableButton();
		}
		*/
		//Disable the previous button if the slide reached its left end
/*		if (index == 0) {
			previousArrow.disableButton();
		} else {
			previousArrow.enableButton();
		}
		*/

		for (int i=0; i < sliderDots.Length; i++) {
			if (i == index) {
				sliderDots [i].color = Color.white; //if its the selected dot then make it white
			}
			else
			{	sliderDots [i].color = sdColor; // other dots gets greyed out
			}
		}

		info.text = str[index];




	
	}




	public void increaseIndex() {

		if (index == 3) {
			ic.deleteDrawLines ();
		}

		index++;


		if (index == 6) {

			tx.text = "Finish";
		}
		else {

			tx.text = "Next";

		}

		if (index == 7) {

			GoToxScene.instance.sceneIndex = 5;
			GoToxScene.instance.GoToScene ();
			//go to home scene
			return;
		}



		ic.featureComplete = false;
		ic.index = index;
		ic.resetCube ();
		updateFeatures ();
	}

	public void decreaseIndex() {
		if (index == 3) {
			ic.deleteDrawLines ();
		}
		index--;		
		ic.index = index;
		ic.resetCube ();
		updateFeatures ();


	}

}
