using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR;

public class OrbitBackground : VRObject
{

    bool mOrbiting = false;

    GameObject cMain;

    Vector3 initCursorPos = new Vector3();
    bool initCursorPosSet = false;

    Cursor c;

    float orbitSpeed = 0.0025f;


    // Use this for initialization
    void Start()
    {


    }
  
    public override void end()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        

            if (LeftCursor.instance.hold )
            {
                if (!initCursorPosSet)
                {
				initCursorPos = LeftCursor.instance.transform.localPosition;
                    initCursorPosSet = true;
                }
			Vector3 relativePos = LeftCursor.instance.transform.localPosition - initCursorPos;

                if (cMain == null)
                    cMain = GameObject.Find("CardboardMain");

                cMain.transform.RotateAround(transform.position, cMain.transform.up, relativePos.x * orbitSpeed);
            }
            else
                initCursorPosSet = false;
 
    }
}
