using UnityEngine;

using UnityEngine.UI;
using System.Collections;

public class CreateMarker : VRObject {


	public GameObject marker;
	public GameObject mapCanvas;

	public Color normalColor    = Color.white;
	public Color hoverColor     = new Color32(156, 244, 167, 255);
	public Image targetImage;

	public int pinCount;


	bool mUpdate; // Can Update
	Cursor mCursor; //current cursor

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {


	}

	public override void start(ref Cursor cursorInput) {

		if (RightCursor.instance.clicked) {
			mCursor = cursorInput;
		}
		base.start(ref cursorInput);

	}

	public override void end() {

			addMarker ();
			RoomBuilderMenu.instance.disableMenu ();
				


	}


	public override void enterHover(Cursor c)
	{
		base.enterHover(c);

		if (targetImage)
		{
			targetImage.color = hoverColor;
		}
	
	}

	public override void exitHover(Cursor c)
	{

		if (targetImage)
		{
			targetImage.color = normalColor;
		}

		base.exitHover(c);
	}
		
	public void addMarker() {
		pinCount++;
		Vector3 cursorPos = Camera.main.WorldToScreenPoint(mCursor.transform.position);
		Vector3 CanvasPos = Camera.main.ScreenToWorldPoint (cursorPos);



		GameObject clone = (GameObject) Instantiate (marker, mCursor.transform.position, Quaternion.identity);
		clone.transform.position = cursorPos;
		clone.transform.parent = mapCanvas.gameObject.transform;
		clone.transform.localEulerAngles = new Vector3 (-90, 180, 0);

		//clone.gameObject.AddComponent<FreeDrag> ();
		clone.gameObject.GetComponent<FreeDragLib> ().mCursor = mCursor;
		clone.gameObject.GetComponent<FreeDragLib> ().mFollow = true;
		clone.gameObject.GetComponent<FreeDragLib> ().hasBeenPlaced = false;

		clone.gameObject.GetComponentInChildren<Text> ().text = pinCount.ToString ();


	}
}
