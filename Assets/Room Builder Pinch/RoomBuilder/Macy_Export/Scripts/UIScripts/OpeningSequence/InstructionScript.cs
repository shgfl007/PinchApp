using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionScript : VRObject {
    private static InstructionScript mInstance;
    public  static InstructionScript instance { get { return mInstance; } }

    GameObject everythingContainer , instructionSlide;
    GameObject stationaryObjects;
    public GameObject head;
	bool init = true;
	float timer = 0;
	Object[] instructionSlides;
	int currentSlideIndex = 0;

	// Use this for initialization
	void Start () {
        if (mInstance == null)
        {
            mInstance = this;
        }
        everythingContainer = GameObject.Find ("EverythingContainer");
		//stationaryObjects = GameObject.Find ("StationaryObjects");
		instructionSlides = Resources.LoadAll("Instructions", typeof(Texture2D));
		instructionSlide = GameObject.Find ("InstructionSlide");
	}
	
	// Update is called once per frame
	void Update () {
		// Wait until camera camrottracker is initialized
		if (init) {
			if (head.GetComponent<CamRotTracker> ().cameraPreview != null) {
				// Turn on cam preview
				head.GetComponent<CamRotTracker> ().cameraPreview.gameObject.SetActive (true);
                Camera.main.clearFlags = CameraClearFlags.Skybox;
                everythingContainer.GetComponent<CanvasGroup> ().alpha = 0;
				//stationaryObjects.GetComponent<CanvasGroup> ().alpha = 0;
				head.GetComponent<CamRotTracker> ().camRotTracking = false;
				head.GetComponent<CamRotTracker> ().inCamPreview = true;
                TrackerScript.instance.outputCam = true;

				init = false;
			}
			return;
		}

		timer += Time.deltaTime;

		if (timer > 3.5f) {
			// Next slide
			currentSlideIndex++;
			if (currentSlideIndex >= instructionSlides.Length)
				currentSlideIndex = 0;
			StartCoroutine(switchSlides((Texture) instructionSlides[currentSlideIndex]));
			timer = 0;
		}
	}

	IEnumerator switchSlides(Texture image) {
		// Fade out
		for (int i = 1; i <= 15; i++) {
			instructionSlide.GetComponent<CanvasGroup> ().alpha = 1 - easeInOut (1, 20, i);
			
			yield return new WaitForSeconds (0.016f);
		}

        RawImage rawImage = instructionSlide.GetComponent<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = image;
        }
		// Fade back in
		for (int i = 1; i <= 15; i++) {
			instructionSlide.GetComponent<CanvasGroup> ().alpha = 0 + easeInOut (1, 20, i);
			
			yield return new WaitForSeconds (0.016f);
		}
	}
}
