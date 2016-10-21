using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollHandle : VRObject {

    /*This script includes the slider purely for aesthetic purpose. The default scrollbar with unity always
     *changes the size based on content. So to keep it consistent I hid the scrollbar and replaced it with
     * a slider. The slider values are fed into scrollbar values. 
     */

    bool mUpdate;
    Cursor mCursor;
    public Scrollbar sb;
    public Slider sl;
    float lastCursory;
    float reMap;
    float maxRange = 0.8f;
    float minRange = 0;
    Vector3 lastPosition;


    public override void start(ref Cursor cursorInput)
    {

        if (!mUpdate)
        {
            lastCursory = cursorInput.transform.position.y;
            mUpdate = true;
            mCursor = cursorInput;
        }
        base.start(ref cursorInput);
    }

    void Update()
    {
        if (mUpdate)
        {
            float dir = (mCursor.transform.position.y - lastCursory);
            reMap = Remap(dir, minRange, maxRange);
          //  Debug.Log(reMap);
            sl.value += reMap ;
            sb.value = sl.value;
            lastCursory = mCursor.transform.position.y;
        }
        sl.value = sb.value;
    }

    public float Remap(this float value, float from, float to)
    {
        return (value - from) / (to - from);
    }


    public override void end()
    {
        mUpdate = false;
        base.end();
    }
}