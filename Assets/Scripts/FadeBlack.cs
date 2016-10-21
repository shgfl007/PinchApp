using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour {
	private static FadeBlack mInstance;
	public static FadeBlack instance { get { return mInstance; } }

	public GameObject blackPanel;
	public Color panelColor;
	public Image panelImage;

	public bool fadeToBlack = false;
	public bool fadeFromBlack = false;
    public bool fadeInBetween = false;

    public bool goBack = false;



	// Use this for initialization
	void Start () {
		if (mInstance == null)
		{
			mInstance = this;
		}

		panelImage = blackPanel.GetComponent<Image> ();
		panelColor = panelImage.color;
	
	}

    public void resetColor() {
        panelColor.a =1f;
        panelImage.color = panelColor;

    }




	// Update is called once per frame
	void Update () {

		if(fadeToBlack){
			if (panelColor.a < 1.0f)
			{
				StartCoroutine (IncreaseAlpha());
			}
			fadeFromBlack = false;
		}


		if(fadeFromBlack){
            if (panelColor.a > 0.0f)
            {
				StartCoroutine (DecreaseAlpha());
            }
           fadeFromBlack = false;


		}

        if (fadeInBetween) {

			StartCoroutine (IncreaseAlpha ());
			goBack = true;
			fadeInBetween = false;

        } //end of fade in between

	
	}
	public float time = 4f;

	private IEnumerator IncreaseAlpha () {
		float elapsedTime = 0;
		while (elapsedTime < time) {
			panelColor.a = Mathf.Lerp (panelColor.a, 1, (elapsedTime / time));
			panelImage.color = panelColor;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		if (goBack) {
			StartCoroutine (DecreaseAlpha ());
		}
		goBack = false;

	}


	private IEnumerator DecreaseAlpha () {

		float elapsedTime = 0;

		while (elapsedTime < time) {
			panelColor.a = Mathf.Lerp (panelColor.a, 0, (elapsedTime / time));
			panelImage.color = panelColor;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();

		}
		goBack = false;



	}

}
