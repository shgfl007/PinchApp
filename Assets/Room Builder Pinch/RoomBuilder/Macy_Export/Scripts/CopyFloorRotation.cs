using UnityEngine;
using System.Collections;

public class CopyFloorRotation : MonoBehaviour
{
    bool mRotate;
    public void UpdateRotation()
    {
        //Debug.Log("UpdateRotation");
        transform.rotation = Floor.instance.gameObject.transform.rotation;
    }
}
