using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PinchToStartShuffle : MonoBehaviour {


	public Sprite[] shuffleImages;

	int index = 0;

	public Image currentImage;
	Color tempColor;

	// Use this for initialization
	void Start () {
	
		tempColor = Color.white;
		currentImage.sprite = shuffleImages [index];
		StartCoroutine (shuffleImgFadeOut());
	}


	float pck4Time = 1;
	int counter = 0;


	private IEnumerator shuffleImgFadeOut () {


		//tempColor.a = 1;
		float elapsedTime = 0;

		while (elapsedTime < pck4Time) {
			tempColor.a = Mathf.Lerp (tempColor.a, 0, (elapsedTime / pck4Time));
			currentImage.color =tempColor;
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

		counter++;
		StartCoroutine (shuffleImgFadeIn ());

	}
	private IEnumerator shuffleImgFadeIn () {
		tempColor.a = 0;
		float elapsedTime = 0;

		if (counter == 1) {
			index++;
			if (index > 2) {
				index = 0;
			
			}
			currentImage.sprite = shuffleImages[index];
			
			counter = -1;
		}


		while (elapsedTime < pck4Time) {
			tempColor.a = Mathf.Lerp (tempColor.a, 1, (elapsedTime / pck4Time));
			currentImage.color =tempColor;
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

		counter++;
		StartCoroutine ( shuffleImgFadeOut());

	}
		
}
