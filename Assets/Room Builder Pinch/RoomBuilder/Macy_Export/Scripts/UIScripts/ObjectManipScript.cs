using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectManipScript : VRObject {
	
	GameObject buttonHit;


	void Start() {
		audioEffects = GameObject.Find ("AudioEffects");
	}

	public override void start (ref Cursor cursorInput)
	{
		single = cursorInput;
		mode = "single";

		// Disable hitbox
		gameObject.GetComponent<BoxCollider> ().enabled = false;
	}

	public override void end ()
	{
		gameObject.GetComponent<BoxCollider> ().enabled = true;
		if (single.hit.transform.name == "Trash") {
			Destroy(gameObject);

		}
	}

	public override void AfterUpdate()
	{
        if (single.hit.transform != null)
        {
            if (single.hit.transform.tag == "Floor" || single.hit.transform.tag == "DualCursor")
            {
                Vector3 hitPoint = single.hit.point;
                transform.position = hitPoint;
            }
            else {
                transform.position = single.transform.position * 50;
            }
        }
	}

	public override void enterHover(Cursor c)
	{
	}

	public override void exitHover (Cursor c)
	{
	}

	public override void dualCursorUpdate (ref Cursor left, ref Cursor right)
	{

		//Debug.Log ("Random Check");
		resizeAndRotateWithDualCursor(ref left, ref right, transform, 1);
	}

}
