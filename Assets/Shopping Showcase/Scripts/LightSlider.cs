using UnityEngine;
using System.Collections;

public class LightSlider : MonoBehaviour {

	private Light l;
	private float initIntensity, targetIntensity, curIntensity;
	public bool canChange;


	// Use this for initialization
	void Start () {
		l = GetComponent<Light> ();
		initIntensity = l.intensity;
	}
	
	// Update is called once per frame
	void Update () {

		if (canChange) {
			changeLighting (targetIntensity);

		} else {
		
			return;
		}



	}

	void changeLighting (float targetLight) {
	
		l.intensity = Mathf.MoveTowards(l.intensity, targetLight, Time.deltaTime * 0.5f);
	//	float difference = Mathf.Abs(curIntensity - targetLight);

		if (l.intensity == targetLight ) {

			canChange = false;
		}
	}

	public void lightDirection (bool s) {


		//curIntensity = l.intensity;

		if (s) {
			targetIntensity = 0;
		//	RenderSettings.ambientIntensity = 1.3f;


		} else {
			targetIntensity = initIntensity;
			//RenderSettings.ambientIntensity = 0;

		}

	//	print (l.intensity + "  " + targetIntensity);

		canChange = true;
	}



}
