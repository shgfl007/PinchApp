using UnityEngine;
using System.Collections;

public class SnappingPoint : MonoBehaviour
{
    public Vector3 forwardDir;
    BoxCollider mBoxCollider;
    bool mSeeking;
    SnappingTool mOtherSnappingTool;

    void Start()
    {
        mBoxCollider = gameObject.GetComponent<BoxCollider>();
    }

    public void SeekForAMatch(SnappingTool snappingTool)
    {
        mOtherSnappingTool = snappingTool;
        mBoxCollider.isTrigger = false;
        mSeeking = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (mSeeking && mOtherSnappingTool)
        {
            SnappingPoint result = other.gameObject.GetComponent<SnappingPoint>();
            if (result)
            {
                if (CanSnap(result))
                {
                    mOtherSnappingTool.FoundSnapPoint(this, result);
                    mSeeking = false;
                }
            }
        }
    }
    
    bool CanSnap(SnappingPoint result)
    {
        if (forwardDir != -result.forwardDir)
        {
            return false;
        }
        Vector3 heading = result.gameObject.transform.position - transform.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance; // This is now the normalized direction.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Block b = hit.collider.gameObject.GetComponent<Block>();
            if(b)
            {
                return false;
            }
        }
        return true;
    }

    public void ParentIsSeeking()
    {
        if (mBoxCollider)
        {
           // mBoxCollider.isTrigger = false;
        }
    }
}
