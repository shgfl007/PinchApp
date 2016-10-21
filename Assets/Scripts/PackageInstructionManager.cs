using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PackageInstructionManager : MonoBehaviour {

	public Image[] sliderDots;
	public Button previousArrow, nextArrow;
	public Text badgeText, instructionText, instructionTitle, phoneID;
	public Image pckImage, badgeImage;
	public Sprite pck4;

	public Sprite[] pckImages;
	public string[] pckInstructions, pckTitles;

	private int index = 0;
	private Color sdColor;

	Color tempColor = Color.white;
	Color transparent = Color.white;

	public float time = 0.3f;


	float pck4Time = 3;
	int counter = 0;
	Color pck4_color = Color.white;
	public Image pck4_2;

	// Use this for initialization
	void Start () {
		sdColor = Color.white;
		sdColor.a = 0.3f;

		transparent.a = 0;
		badgeText.color =transparent;
		instructionTitle.color = transparent;
		pckImage.color = transparent;
		instructionText.color = transparent;
		badgeImage.color = transparent;
		pck4_2.sprite = pck4;
		pck4_color.a = 0;
		pck4_2.color = pck4_color;

		pck4_2.enabled = false;
		tempColor.a = 0;
		updateSlides (index);

	}
	public Text doneTxt;

	private void updateSlides (int ind){

		index = ind;

		StopAllCoroutines ();

		//disable the next button if the slide reached its right end
		if (index == 5) {
			nextArrow.interactable = false;
			doneTxt.text = "Done";
			phoneID.color = transparent;
			phoneID.enabled = true; // little number besides the slider
		} else {
			nextArrow.interactable = true;
			phoneID.enabled = false;

		}

		//Disable the previous button if the slide reached its left end
		if (index == 0) {
			previousArrow.interactable = false;
		} else {
			previousArrow.interactable = true;
		}

		//Change Slider Dot Colors
		for (int i=0; i < sliderDots.Length; i++) {

			if (i == index) {
				sliderDots [i].color = Color.white; //if its the selected dot then make it white
			}
			else
			{	sliderDots [i].color = sdColor; // other dots gets greyed out
			}
		}
		StartCoroutine (animateSlidesFadeOut());




	}


	public void increaseNumber() {
		index++;
		updateSlides (index);
	}

	public void decreaseNumber() {

		index--;
		updateSlides (index);
	}



	private IEnumerator fadeOutPck4 () {
		pck4_2.enabled = true;

		//tempColor.a = 1;
		float elapsedTime = 0;

		while (elapsedTime < pck4Time) {
			tempColor.a = Mathf.Lerp (tempColor.a, 0, (elapsedTime / pck4Time));
			pck4_color.a = Mathf.Lerp (pck4_color.a , 1, (elapsedTime / pck4Time));
			pck4_2.color = pck4_color;
			pckImage.color =tempColor;

			elapsedTime += Time.deltaTime;
			print (elapsedTime);
			yield return new WaitForEndOfFrame ();
		}

		counter++;
		StartCoroutine (fadeInPck4 ());

	}


	private IEnumerator fadeInPck4 () {
		//	tempColor.a = 0;
		float elapsedTime = 0;
		/*
		if (counter == 1) {
			pckImage.sprite = pck4;
		} else if (counter == 3)
		// this is 3 because it needs to come back to this loop
		{
			pckImage.sprite = pckImages[index];
			counter = -1;
		}
		*/


		while (elapsedTime < pck4Time) {
			tempColor.a = Mathf.Lerp (tempColor.a, 1, (elapsedTime / pck4Time));
			pckImage.color =tempColor;
			pck4_color.a = Mathf.Lerp (pck4_color.a , 0, (elapsedTime / pck4Time));
			pck4_2.color = pck4_color;
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

		counter++;
		StartCoroutine (fadeOutPck4 ());

	}

	private IEnumerator animateSlidesFadeOut() {

		if (index != 3) {
			pck4_2.enabled = false;

		} 
		//tempColor.a = 1;
		float elapsedTime = 0;


		while (elapsedTime < time) {

			tempColor.a = Mathf.Lerp (tempColor.a, 0, (elapsedTime / time));
			badgeText.color = tempColor;
			instructionTitle.color = tempColor;
			pckImage.color = tempColor;
			instructionText.color = tempColor;
			badgeImage.color = tempColor;

			// badgeText, instructionText, instructionTitle;
			// pckImage, badgeImage;
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

		StartCoroutine (animateSlidesFadeIn());

	}


	IEnumerator animateSlidesFadeIn() {

		tempColor.a = 0;
		float elapsedTime = 0;
		pckImage.sprite = pckImages [index];
		instructionText.text = pckInstructions [index];
		badgeText.text = (index+1).ToString ();
		instructionTitle.text = pckTitles [index];

		while (elapsedTime < time) {

			tempColor.a = Mathf.Lerp (tempColor.a, 1, (elapsedTime / time));
			badgeText.color = tempColor;
			instructionTitle.color = tempColor;
			pckImage.color = tempColor;
			instructionText.color = tempColor;
			badgeImage.color = tempColor;
			if (index == 5) {
				phoneID.color = tempColor;
			}
			// badgeText, instructionText, instructionTitle;
			// pckImage, badgeImage;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		if (index == 3) {
			Invoke ("startLater", 0.5f);
		}

	}


	void startLater(){
		StartCoroutine (fadeOutPck4 ());

	}


}
