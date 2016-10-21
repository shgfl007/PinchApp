using UnityEngine;
using System.Collections;

public class RotateWheel : VRObject {

	private Quaternion originalRotation; // Original Rotation
	private float  startAngle = 0; // Start Angle
    bool mUpdate; // Can Update
    Cursor mCursor; //current cursor

    public void Start()
	{
		originalRotation = this.transform.rotation;
	}


    public override void start(ref Cursor cursorInput)
    {
        if (!mUpdate)
        {
            mUpdate = true;
            mCursor = cursorInput;
            InputIsDown();
        }
        base.start(ref cursorInput);
    }

    public override void end()
    {
        mUpdate = false;
        base.end();
    }
    public void Update()
    {

        if (mUpdate)
        {
            InputIsHeld();
        }

    }

    public void InputIsDown()
	{

		originalRotation = this.transform.rotation;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorPos = Camera.main.WorldToScreenPoint(mCursor.transform.position);
        Vector3 vector = cursorPos - screenPos;
        startAngle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

	}

	public void InputIsHeld()
	{
        //rotate the wheel in a radius
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorPos = Camera.main.WorldToScreenPoint(mCursor.transform.position);
		Vector3 vector = cursorPos - screenPos;
		float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		Quaternion newRotation = Quaternion.AngleAxis((angle - startAngle)/1.5f , this.transform.forward);
		newRotation.y = 0; 
		newRotation.eulerAngles = new Vector3(0,0,-newRotation.eulerAngles.z );
		this.transform.rotation = originalRotation *  newRotation ;


	}
}
