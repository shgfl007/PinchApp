using UnityEngine;
using System.Collections;

public class VRCamera : MonoBehaviour {
    private static VRCamera mInstance;
    public static VRCamera instance { get { return mInstance; } }
    // Use this for initialization
    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }
}
