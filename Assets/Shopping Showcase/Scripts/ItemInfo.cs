using UnityEngine;
using System.Collections;

public class ItemInfo : VRObject {

	public bool canRotate = true;
	bool canControlRotate = false;
	Vector3 initMousePos;
	private float  startAngle = 0;
	private float rotX, rotY;
	private Vector3 origRot;
	private float rotSpeed = 0.5f;
	Vector3 lastPos;
	public bool inCar = false;
	public GameObject cTag;
	bool isShowing = false;

	public Animator a1, a2;
	bool switchLevels = true;

	public GameObject camPos;

	Cursor curCursor;
	public bool mUpdate = false;
	// Use this for initialization
	void Start () {
		//a1.Play("Sliding",0, 0f);
	//	a2.Play("Pop", 0, 0f);


		origRot = this.transform.eulerAngles;
		rotX = origRot.x;
		rotY = origRot.y;

	}

	public override void start (ref Cursor c) {

		initMousePos = Camera.main.WorldToScreenPoint(c.transform.position);
		mUpdate = true;
		//canControlRotate = true;
		curCursor = c;



		//Invoke ("stopRot", 3);
	}

	void stopRot() {

		canControlRotate = false;
	}

	public override void end() {




		if (!bothCursorClicking) {


			if (switchLevels) {
				ToggleLighting.instance.SetLayerRecursively (this.gameObject, 14, switchLevels);

			} else {
				ToggleLighting.instance.SetLayerRecursively (this.gameObject, 0, switchLevels);

			}
			switchLevels = !switchLevels;

			canControlRotate = false;
			mUpdate = false;
			isShowing = !isShowing;


			if (isShowing && cTag != null ) {

				cTag.SetActive (true);


			} else if(!isShowing && cTag !=null) {
				cTag.SetActive (false);
			}



			ShopChangeCamPos.instance.goToTee (camPos.transform.position, camPos.transform.rotation, this.gameObject);
		
		
		
		}
	}

	public void stopRotation() {

		//	inCar = !inCar;
		canRotate = false;
		//transform.eulerAngles = new Vector3 (0,0,0);
	}


	public void startRotation () {

		canRotate = true;

	}

	bool bothCursorClicking = false;
	// Update is called once per frame
	void Update () {

		if (LeftCursor.instance.clicked && RightCursor.instance.clicked) {
			bothCursorClicking = true;
		} else {
			bothCursorClicking = false;
		}
			

	}


	public void playAnimations () {
	
		a1.Play("Sliding", -1, 0f);
		a2.Play("Pop", -1, 0f);

	}

	public void shirtAnimation() {

		StartCoroutine (addThisToCart());

	}
	float time =1f;

	private IEnumerator addThisToCart () {

		float elapsedTime = 0;
		this.gameObject.transform.parent = Camera.main.transform;

		while (elapsedTime < time) {

			float scaleIncrement = Mathf.Lerp (gameObject.transform.localScale.x, 0, (elapsedTime / time));
			Vector3 positionIncrement = gameObject.transform.localPosition;

			float speed = Time.deltaTime * 7;

			positionIncrement.y += speed;
			positionIncrement.x += speed;


			gameObject.transform.localScale = new Vector3 (scaleIncrement,scaleIncrement,scaleIncrement) ;
			gameObject.transform.localPosition = positionIncrement;
			elapsedTime += Time.deltaTime;

			yield return new WaitForEndOfFrame ();
		}

			this.gameObject.SetActive (false);




	}

}
