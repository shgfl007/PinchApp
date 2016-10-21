using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonBehaviour : VRObject {




	public GameObject targetImage; // Assign this if another Image is being affected
	public float scaleFactor;
	Vector3 initScale;
	public bool selfImage;
	bool allowRollOver = false;
	private GameObject temp;
	private AudioManager am;
	void OnEnable() {

		StartCoroutine (toggleRollOver ());
	}


	void OnDisable() {

		allowRollOver = false;

	}

	IEnumerator toggleRollOver () {

		yield return new WaitForSeconds (0.75f);
		allowRollOver = true;

	}

	public void Start() {
		if (selfImage) {

			targetImage = transform.gameObject;
		}
		//temp = GameObject.Find ("AudioManager");
		//am = temp.getCom
		initScale = new Vector3 (1,1,1);

	}
	 

	public override void enterHover(Cursor currentCursor)	{

		if (allowRollOver && targetImage != null) {
			Vector3 tempScale = targetImage.transform.localScale;
			tempScale.x *= scaleFactor;
			tempScale.y *= scaleFactor;
			tempScale.z *= scaleFactor;
			targetImage.transform.localScale = tempScale;


		}

		if (AudioManager.instance != null) {
			AudioManager.instance.PlayAudio (0);
		}
		//Debug.Log ("HIIIIII");

	}

	public override void start(ref Cursor c) {
		if (AudioManager.instance != null) {
			AudioManager.instance.PlayAudio (1);
		}
	}

	public override void exitHover(Cursor currentCursor)	{
		if (allowRollOver && targetImage != null) {
			targetImage.transform.localScale = initScale;
		}
	}






}
