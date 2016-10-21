using UnityEngine;
using System.Collections;

public class LeftCursor : Cursor
{
    private static LeftCursor mInstance;
    public static LeftCursor instance { get { return mInstance; } }

    // Use this for initialization
    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }
}
