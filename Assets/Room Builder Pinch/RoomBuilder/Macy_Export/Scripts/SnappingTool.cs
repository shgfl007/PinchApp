using UnityEngine;
using System.Collections.Generic;

public class SnappingTool : MonoBehaviour
{
    public List<SnappingPoint> snappingPoints = new List<SnappingPoint>();
    public bool listentToKey;
    bool mChecking;
    Rigidbody mRigidbody;
    SnappingPoint mParentPoint;
    bool mReposition;
    Vector3 mPreviousPos;
    bool mCollidied;

    void Update()
    {
        if (listentToKey)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                mReposition = true;
                SeekingSnapPoint();
            }
            if(mReposition)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Vector3 newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z);
                    gameObject.transform.position = newPos;
                }
            }
        }
    }
    
    public void SeekingSnapPoint()
    {
        enableSnappingPoints(false);
    }

    public void StopSeekingSnapPoint()
    {
        enableSnappingPoints(true);
    }

    public void FoundSnapPoint(SnappingPoint otherPoint, SnappingPoint myPoint)
    {
        mReposition = false;
        Vector3 distance = myPoint.transform.position - otherPoint.transform.position;
        transform.position -= distance;
        FreeDrag f = gameObject.GetComponent<FreeDrag>();
        if(f)
        {
            f.Freeze();
        }
    }

    void enableSnappingPoints(bool enableIt)
    {
        mChecking = !enableIt;
        if(!enableIt)
        {
            if (mRigidbody == null)
            {
                mRigidbody = gameObject.AddComponent<Rigidbody>();
            }
            if (mRigidbody != null)
            {
                mRigidbody.useGravity = false;
                mRigidbody.velocity = Vector3.zero;
                mRigidbody.isKinematic = true;
            }
        }
        else
        {
            if (mRigidbody)
            {
                Destroy(mRigidbody);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (mChecking)
        {
            SnappingPoint point = other.GetComponent<SnappingPoint>();
            if (point)
            {
                point.SeekForAMatch(this);
                foreach (SnappingPoint s in snappingPoints)
                {
                    s.ParentIsSeeking();
                }
            }
            Block b = other.GetComponent<Block>();
            FreeDrag f = gameObject.GetComponent<FreeDrag>();
            if (b)
            {
                if(f)
                {
                    //f.Collided(true);
                }
            }
            else
            {
                if (f)
                {
                    //f.Collided(false);
                }
            }
        }
    }
}
