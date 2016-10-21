using UnityEngine;
using System.Collections;

public class RightCursor : Cursor
{
    private static RightCursor mInstance;
    public static RightCursor instance { get { return mInstance; } }

    // Use this for initialization
    void Start ()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }
}
