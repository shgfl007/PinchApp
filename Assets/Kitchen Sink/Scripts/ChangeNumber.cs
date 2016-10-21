//Kitchen Sink
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeNumber : VRObject {

	public Slider sl;// Slider
	private int slValue; // Slider Value
	public Text numberToChange; // Number to change on slide

    bool mUpdate;
    Cursor mCursor; // Current Cursor
    float lastCursorx; // Last Cursor Value
    float reMap; //Remapping Slider Values
    float maxRange = 0.8f; //max range value
    float minRange = 0; //min range value
    Vector3 lastPosition;

    // Use this for initialization
    void Start () {
        slValue = (int)sl.value * 100; //Multiply the Slider value by 100 to get numbers from 0-100
		numberToChange.text = slValue.ToString (); //Change Number to string
	}

    public override void start(ref Cursor cursorInput)
    {
        if (!mUpdate)
        {
            lastCursorx = cursorInput.transform.position.x; //On Click get the cursor position
            mUpdate = true; //Allow for values to change
            mCursor = cursorInput; //get the current cursor (left/right)
        }
        base.start(ref cursorInput);
    }

    // Update is called once per frame
    void Update () {

        if (mUpdate)
        {
            float dir = (mCursor.transform.position.x - lastCursorx) * -1; //subtract start from last pos
            reMap = Remap(dir, minRange, maxRange); //remapping how much mouse has moved to slider values
            sl.value += reMap; // add the moved amount
            lastCursorx = mCursor.transform.position.x; // take the last cursor
            slValue = (int)(sl.value * 100); //convert the slider value between 0-100 for text
            numberToChange.text = slValue.ToString(); // assign slider value to text
        }
    }

    //Remap function
    public float Remap(this float value, float from, float to)
    {
        return (value - from) / (to - from);
    }


    public override void end()
    {
        mUpdate = false; // user released mouse
        base.end();
    }
}
