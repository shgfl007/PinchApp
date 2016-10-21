using UnityEngine;
using System.Collections;

public enum RotateDir
{
    X,
    Y
}

public class RotateTool : VRObject
{
    public Renderer circle;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public delegate void Rotating(float angle, RotateTool tool);
    public event Rotating onRotating;
    public RotateDir rotationDir = RotateDir.X;
    bool mUpdate;
    Vector3 mCursorStartPos = Vector3.zero;
    Cursor mCursor;

    public override void enterHover(Cursor c)
    {
        circle.sharedMaterial = selectedMaterial;
        base.enterHover(c);
    }

    public override void exitHover(Cursor c)
    {
        circle.sharedMaterial = defaultMaterial;
        base.exitHover(c);
    }
    public override void start(ref Cursor cursorInput)
    {
        if (!mUpdate)
        {
            mCursorStartPos = cursorInput.gameObject.transform.position;
            mUpdate = true;
            mCursor = cursorInput;
        }
        base.start(ref cursorInput);
    }

    void Update()
    {
        if(mUpdate)
        {
            Vector3 dir = mCursor.transform.position - mCursorStartPos;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float roundAngle = Mathf.Round(angle / 45) * 45;

            angle = roundAngle;

            //Debug.Log("angle: "+ angle);
            if(onRotating!=null)
            {
                onRotating(angle, this);
            }
        }
    }

    public override void end()
    {
        mUpdate = false;
        base.end();
    }
}
