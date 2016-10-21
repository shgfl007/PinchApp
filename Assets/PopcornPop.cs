using UnityEngine;
using System.Collections;

public class PopcornPop : MonoBehaviour {
	float time = 0.5f;
	Vector3 origPosition;
	Vector3 origScale;
	Vector3 targetPosition;
	Vector3 targetScale;
	Vector3 secondaryTargetScale;
	float popTime;

	void OnEnable() {

		StartCoroutine (pop());

	}


	void OnDisable() {

		resetParameters ();
	}


	// Use this for initialization
	void Start () {
	
		origPosition = new Vector3 (0,72,0);
		targetPosition = new Vector3 (0,72,25);
		origScale = new Vector3 (0,0,0);
		targetScale = new Vector3 (1,1,1);
		secondaryTargetScale = new Vector3 (1.5f,1.5f,1.5f);
		resetParameters ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator pop() {
		float elapsedTime = 0;
		popTime = (time / 2);
		float remainingTime = time - popTime;
		while (elapsedTime < time) {

			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPosition, (elapsedTime / time));

			if (elapsedTime < popTime) {
				transform.localScale = Vector3.Lerp (transform.localScale, secondaryTargetScale, (elapsedTime / popTime));
			} else {
				transform.localScale = Vector3.Lerp (transform.localScale,targetScale, (elapsedTime / time));

			}
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

	}

	void resetParameters() {

		transform.localPosition = origPosition;
		transform.localScale = origScale;

	}

}
