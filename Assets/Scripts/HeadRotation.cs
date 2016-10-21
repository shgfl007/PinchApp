using UnityEngine;
using System.Collections;

public class HeadRotation : MonoBehaviour {
    private static HeadRotation mInstance;
    public  static HeadRotation instance { get { return mInstance; } }
    // Use this for initialization
    void Start () {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }
}
