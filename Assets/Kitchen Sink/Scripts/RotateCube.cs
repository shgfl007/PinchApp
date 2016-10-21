using UnityEngine;
using System.Collections;


public class RotateCube : VRObject
{
    bool mUpdate;
    Cursor mCursor;
    Vector3 lastPos;

	Vector3 initMousePos;
	private float  startAngle = 0;
	private float rotX, rotY;
	private Vector3 origRot;
	private float rotSpeed = 0.5f;


    public override void start(ref Cursor curCursor)
    {
        if (!mUpdate)
        {
            mUpdate = true;
            mCursor = curCursor;
			origRot = this.transform.eulerAngles;
			rotX = origRot.x;
			rotY = origRot.y;
			initMousePos = Camera.main.WorldToScreenPoint(mCursor.transform.position);

        }
		base.start(ref curCursor);
    }
    void Update()
    {
        
            if (mUpdate)
            {


			Vector3 curCursorPos = Camera.main.WorldToScreenPoint (mCursor.transform.position);

			float deltaX = initMousePos.x - curCursorPos.x;
			float deltaY = initMousePos.y - curCursorPos.y;
			rotX -= deltaY * Time.deltaTime  * 10 * -1 ;
			rotY += deltaX * Time.deltaTime  * 10;
			transform.eulerAngles = new Vector3 (rotX, rotY, 0);
			initMousePos =  Camera.main.WorldToScreenPoint (mCursor.transform.position);

            }

    }

    public override void end()
    {
        mUpdate = false;
        base.end();
    }
}




