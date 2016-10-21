using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VRObject : MonoBehaviour {
    public delegate void Action(VRObject vrObject, Cursor cursorInput);
    public delegate void Released(VRObject vrObject);
    public event Action onClicked;
    public event Released onReleased;
	public Cursor single;
	public string mode;
	public GameObject cameraObject;
	public bool hovering = false;
	public GameObject audioEffects;

    void Start()
    {
		audioEffects = GameObject.Find ("AudioEffects");
    }

    public virtual void Init() { }

    /// <summary>
    /// This is click
    /// </summary>
    /// <param name="cursorInput"></param>
	public virtual void start(ref Cursor cursorInput)
	{
        single = cursorInput;
		mode = "single";
        if (onClicked != null)
        {
            onClicked(this, cursorInput);
        }
    }

	public virtual void end()
	{
        if (onReleased != null)
        {
            onReleased(this);
        }
    }

    /// <summary>
    /// This is not onClick
    /// </summary>
	public virtual void onClick(){}

	public virtual void AfterUpdate()
	{
        if (cameraObject == null)
        {
            cameraObject = VRCamera.instance.gameObject;
        }
    }

	public virtual void enterHover(Cursor c){}

	public virtual void exitHover(Cursor c) {}

	public virtual void dualCursorUpdate(ref Cursor left, ref Cursor right)
	{
	}

	// Inverse is + yDiff
	public float verticalScroll(GameObject content, float scale, bool inverse) {
		float yDifference = single.transform.localPosition.y - single.previousCursorPos.y;
		Vector3 localPositionVector = content.transform.localPosition;
		if (inverse)
			localPositionVector.y = localPositionVector.y - yDifference * scale;
		else
			localPositionVector.y = localPositionVector.y + yDifference * scale;
		content.transform.localPosition = localPositionVector;

		return yDifference * scale;
	}

	// Inverse is + xDiff
	public float horizontalScroll(GameObject content, float scale, bool inverse) {
		float xDifference = single.transform.localPosition.x - single.previousCursorPos.x;
		Vector3 localPositionVector = content.transform.localPosition;
		if (inverse)
			localPositionVector.x = localPositionVector.x - xDifference * scale;
		else
			localPositionVector.x = localPositionVector.x + xDifference * scale;
		content.transform.localPosition = localPositionVector;

		return xDifference * scale;
	}

	public void resizeAndRotateWithDualCursor(ref Cursor left, ref Cursor right, Transform content, float scaleSpeed) {
		// Do resize
		Vector3 currentDist = left.transform.localPosition - right.transform.localPosition;
		Vector3 prevDist = left.previousCursorPos - right.previousCursorPos;
		float magnitudeDiff = (currentDist.magnitude - prevDist.magnitude) * scaleSpeed;
		
		Vector3 newLocalScale = content.localScale;
		newLocalScale += new Vector3(magnitudeDiff, magnitudeDiff, magnitudeDiff);
		content.localScale = newLocalScale;
		
		// Do rotation
		float dAngle = angleFromXAxis (currentDist) - angleFromXAxis(prevDist);
		Vector3 newLocalRotation = content.localRotation.eulerAngles;
		newLocalRotation.z += dAngle;
		content.localRotation = Quaternion.Euler (newLocalRotation);
	}

	// Utility calculation function. Pretty useful thing
	public float angleFromXAxis(Vector3 vector)
	{
		float refAngle = Mathf.Atan(Mathf.Abs(vector.y) / Mathf.Abs(vector.x));
		refAngle = refAngle / Mathf.PI * 180;
		
		if (vector.x > 0 && vector.y > 0)
			return refAngle;
		else if (vector.x < 0 && vector.y > 0)
			return 180 - refAngle;
		else if (vector.x < 0 && vector.y < 0)
			return 180 + refAngle;
		else if (vector.x > 0 && vector.y < 0)
			return 360 - refAngle;
		else
			return 0;
	}

	public virtual float easeInOut(float totalLength, int increments, float incrementNum) {
		return totalLength * (-0.5f * Mathf.Cos(Mathf.PI * incrementNum / (float) increments) + 0.5f);
	}



	public IEnumerator unDimCameraBackground() {
		GameObject mainCamera = GameObject.Find ("Main Camera");
		int intervals = 20;
		float original = (float)40 / (float)255;
		
		for (int i = 1; i <= intervals; i++) {
			float colour = original + easeInOut((float)200/255, intervals, i);
			mainCamera.GetComponent<Camera>().backgroundColor = new Color(colour, colour, colour);
			
			yield return new WaitForSeconds(0.016f);
		}
	}



	public IEnumerator fadeIn(Transform toFade, int intervals = 20, bool setActiveFirst = false, float initialAlpha = -1) {
		if (setActiveFirst)
			toFade.gameObject.SetActive (true);
		if (toFade.GetComponent<CanvasGroup> () != null) {
			if (initialAlpha == -1)
				initialAlpha = toFade.GetComponent<CanvasGroup> ().alpha;
			for (int i = 1; i <= intervals; i++) {
				toFade.GetComponent<CanvasGroup> ().alpha = initialAlpha + easeInOut (1 - initialAlpha, intervals, i);
			
				yield return new WaitForSeconds (0.016f);
			}
		}
	}

	public IEnumerator fadeOut(Transform toFade, int intervals = 20, bool setInactiveAfter = false, float initialAlpha = -1) {
		if (toFade.GetComponent<CanvasGroup> () != null) {
			if (initialAlpha == -1)
				initialAlpha = toFade.GetComponent<CanvasGroup> ().alpha;
			for (int i = 1; i <= intervals; i++) {
				toFade.GetComponent<CanvasGroup> ().alpha = initialAlpha - easeInOut (initialAlpha, intervals, i);
			
				yield return new WaitForSeconds (0.016f);
			}
		}
		if (setInactiveAfter)
			toFade.gameObject.SetActive (false);
	}

}
