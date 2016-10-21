using UnityEngine;
using UnityEngine.UI;

public class FreeDragLib : VRObject
{
	public Cursor mCursor;
	public bool mFollow = false;
	public bool hasBeenPlaced = false;

	public Color normalColor    = Color.white;
	public Color hoverColor     = new Color32(156, 244, 167, 255);
	public Image targetImage;
	public bool zoomIn = false;
	Vector3 centerX ;
	private GameObject cardboardM;
	private GameObject cameraPlacer;
	float step;
	Vector3 modifiedVal;
	private GameObject blackImage;
	public GameObject Panel;

	public bool firstOne = false;

	void Start() {
		targetImage = GetComponent<Image> ();
		cardboardM = GameObject.Find ("CardboardMain");
		cameraPlacer = this.gameObject.transform.GetChild (0).gameObject;
		if (hasBeenPlaced) {
			Invoke ("fetchScreen", 0.2f);
		} else {
			fetchScreen ();
		}
		//Camera.main.transform.LookAt (Vector3.left);

		//cardboardM.transform.LookAt ();
	}

	void fetchScreen () {
		blackImage = TurnOffPanels.instance.transport.gameObject;

	}

	public override void start(ref Cursor cursorInput)
	{

		if (!mFollow && !hasBeenPlaced)
		{
			mFollow = true;
			mCursor = cursorInput;
			//InputIsDown();
		}

		if (hasBeenPlaced){
			zoomIn = true;
			//Cardboard.SDK.Recenter ();


		}
		base.start(ref cursorInput);
	}

	public override void end(){

		mFollow = false;
		hasBeenPlaced = true;


	}

	public override void enterHover(Cursor c){

		if (targetImage) {

			targetImage.color = hoverColor;
		}
	}



	public override void exitHover(Cursor c){

		if (targetImage) {

			targetImage.color = normalColor;
		}
	}
	//	bool

	void MoveCamera() {

		ManualHeadRot.instance.zeroCardboard();
		cardboardM.transform.position = cameraPlacer.transform.position;


	}

	void turnOffImage () {
			blackImage.SetActive (false);

		if (Panel.gameObject != null) {
			Panel.gameObject.SetActive (true);

		}
		TurnOffPanels.instance.resetPanel (Panel.gameObject);

	}

	// Update is called once per frame
	void Update ()
	{
		if (zoomIn && hasBeenPlaced)
		{ 

			//Debug.Log (centerX);
		if (!firstOne) {
			
			FadeBlack.instance.fadeInBetween = true;
			blackImage.SetActive(true);}
			Invoke ("MoveCamera", 0.375f);
			Invoke ("turnOffImage", 0.75f);
			zoomIn = false;
		}


		if (mFollow && !hasBeenPlaced) {
			Vector3 temp = mCursor.transform.position;
			Vector3 worldToScreen = Camera.main.WorldToScreenPoint (temp);
			Vector3 cubeScreen = Camera.main.WorldToScreenPoint (transform.position);

			Vector3 cursorToCube = new Vector3 (worldToScreen.x, worldToScreen.y, cubeScreen.z);
			Vector3 screenToWorld = Camera.main.ScreenToWorldPoint (cursorToCube);

			screenToWorld.y = gameObject.transform.parent.position.y + 13; 

			transform.position = screenToWorld;



		} else {
			//this.enabled = false;
		}

	}

}