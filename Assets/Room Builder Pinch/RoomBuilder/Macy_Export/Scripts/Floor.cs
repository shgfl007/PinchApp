using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{
    private static Floor mInstance;
    public static Floor instance { get { return mInstance; } }
    void Start () {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }
	
}
