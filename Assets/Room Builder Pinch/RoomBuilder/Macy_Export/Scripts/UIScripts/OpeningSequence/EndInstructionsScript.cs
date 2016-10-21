using UnityEngine;
using System.Collections;

public class EndInstructionsScript : VRObject {
    private static EndInstructionsScript mInstance;
    public static EndInstructionsScript instance { get { return mInstance; } }
    //GameObject instructionsScript;

    //GameObject everythingContainer;
    //GameObject stationaryObjects;
    GameObject head;
    //GameObject instructionSlide;
	//bool init = true;
	//float timer = 0;
	//Object[] instructionSlides;
	//int currentSlideIndex = 0;
	
	// Use this for initialization
	void Start () {
        if (mInstance == null)
        {
            mInstance = this;
        }
        //everythingContainer = GameObject.Find ("EverythingContainer");
		//stationaryObjects = GameObject.Find ("StationaryObjects");
		//instructionSlides = Resources.LoadAll("Instructions", typeof(Texture2D));
		//instructionSlide = GameObject.Find ("InstructionSlide");
	}

	public override void onClick ()
	{
		base.onClick ();
		//instructionsScript = transform.parent.gameObject;
        if (HeadRotation.instance)
        {
            head = HeadRotation.instance.gameObject;


            head.GetComponent<CamRotTracker>().camRotTracking = true;
            TrackerScript.instance.outputCam = false;
            head.GetComponent<CamRotTracker>().cameraPreview.gameObject.SetActive(false);
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            if (gameObject.activeSelf)
            {
                StartCoroutine(head.GetComponent<CamRotTracker>().toggleCamPreview());
            }
            //Debug.Log("End Instruction");
            if (transform.parent)
            {
                if (transform.parent.gameObject.activeInHierarchy)
                {
                    StartCoroutine(fadeOut(transform.parent, 20, true));
                }
            }
            if(UIManager.instance)
            {
                UIManager.instance.CanRotateScene(true);
            }
        }
	}

}
