using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ButtonAnimations : MonoBehaviour {


	Image img;

	Vector3 initPosition;
	public Vector3 targetPosition = new Vector3(0,230,0);

	Color initColor;
	Color targetColor;

	Vector3 initScale;
	Vector3 targetScale;

	float time = 0.75f;


	void OnEnable() {
		StartCoroutine (animateIcons());
	}

	void OnDisable() {

		resetParameters ();
	}

	// Use this for initialization
	void Awake () {

		img = GetComponent<Image> ();

		initPosition = Vector3.zero;
		//targetPosition = new Vector3 (0,230,0);


		initScale = Vector3.zero;
		targetScale = new Vector3 (1,1,1);

		initColor = img.color;
		initColor.a = 0;

		targetColor = initColor;
		targetColor.a = 1;

		resetParameters ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void resetParameters () {

		img.gameObject.transform.localScale = initScale;
		img.gameObject.transform.localPosition = initPosition;
		img.color = initColor;

	}

	private IEnumerator animateIcons () {

		float elapsedTime = 0;

		while (elapsedTime < time) {

			float scaleIncrement = Mathf.Lerp (img.gameObject.transform.localScale.x, targetScale.x, (elapsedTime / time));
			Vector3 positionIncrement = Vector3.Lerp(img.gameObject.transform.localPosition, targetPosition, (elapsedTime / time));
			float alphaIncrement = Mathf.Lerp (img.color.a, targetColor.a, (elapsedTime / time));
			Color tempColor = img.color;
			tempColor.a = alphaIncrement;
			img.gameObject.transform.localScale = new Vector3 (scaleIncrement,scaleIncrement,scaleIncrement) ;
			img.gameObject.transform.localPosition = positionIncrement;
			img.color = tempColor;

			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}



	}





}
