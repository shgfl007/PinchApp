using UnityEngine;

public class FreeDrag : MonoBehaviour
{
    Cursor mCursorInput;
    static float floorY = -20;
    bool mFollow;
    bool mSnapped;
    Vector3 mSnappedCursorPos;
    float mUnfreezeDistance = 0.15f;
    float mDistance;
    Block mBlock;
    bool mHitOther;
    Vector3 mWorkingPos;
    float mYDif;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cursorInput"></param>
    /// <param name="y">Y Different</param>
    public void FollowGameObject(Cursor cursorInput, float y)
    {
        mFollow = true;
        mHitOther = false;
        mCursorInput = cursorInput;
        mBlock = gameObject.GetComponent<Block>();
        
        if (mBlock)
        {
            //mBlock.OnExitHover += HandleExitHover;
            mBlock.OnHitCollider += HandleOnHitCollider;
            //mBlock.OnEnterHover += HandleOnEnterHover;
            mBlock.StartToChange(true);
        }
        mYDif = y;
        UIManager.instance.Dragging = true;
    }

    void HandleExitHover(Cursor c)
    {
        if (mCursorInput == c)
        {
            mFollow = false;
        }
    }

    void HandleOnEnterHover(Cursor c)
    {
        if(mCursorInput == c)
        {
            mFollow = false;
        }
    }

    private void HandleOnHitCollider(Collision other)
    {
        mHitOther = true;
    }

    // Update is called once per frame
    void Update ()
    {
	    if(mFollow)
        {
            if (mHitOther)
            {
                transform.position = mWorkingPos;
                transform.position = new Vector3(transform.position.x, -15.0f, transform.position.z);
                mHitOther = false;
                return;
            }
            Vector3 temp = mCursorInput.transform.position;
            Vector3 worldToScreen = Camera.main.WorldToScreenPoint(temp);
            Vector3 cubeScreen = Camera.main.WorldToScreenPoint(transform.position);
            
            Vector3 cursorToCube = new Vector3(worldToScreen.x, worldToScreen.y, cubeScreen.z);
            Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(cursorToCube);
            screenToWorld.y = -15.0f;

            BoxCollider box = gameObject.GetComponent<BoxCollider>();
            float objectHeight = 100;
            if(box)
            {
                objectHeight = box.bounds.size.y;
            }
            if (mCursorInput.hit.collider)
            {
                Block b = mCursorInput.hit.collider.gameObject.GetComponent<Block>();
                if (b)
                {
                    return;
                }
                Vector3 hitPoint = mCursorInput.hit.point;
                float z = hitPoint.z;
                hitPoint = Camera.main.transform.position + (hitPoint - Camera.main.transform.position) * .7f;
                
                //float y = hitPoint.y + objectHeight + mYDif;
                float y = hitPoint.y;
                if(y<floorY)
                {
                    y = floorY;
                }

                // Vector3 newPos = new Vector3(screenToWorld.x, y, hitPoint.z);
                 Vector3 newPos = new Vector3(screenToWorld.x, screenToWorld.y, hitPoint.z);

                if (Mathf.Abs(hitPoint.z - Camera.main.transform.position.z) <= 8)
                {
                    //transform.position = mWorkingPos;
                   // Debug.Log("newPos: " + mWorkingPos);
                }
                else
                {
                    transform.position = newPos;
                }
                mWorkingPos = transform.position;
            }
        }
        if(mSnapped)
        {
            float distance = Mathf.Abs(Vector3.Distance(mSnappedCursorPos, mCursorInput.gameObject.transform.position));
            if(distance >= mUnfreezeDistance)
            {
                mSnapped = false;
                mFollow = true;
            }
        }
	}

    public void Freeze()
    {
        mSnappedCursorPos = mCursorInput.gameObject.transform.position;
        mFollow = false;
        mSnapped = true;
    }

    public void Collided(bool move)
    {
        mFollow = !move;
    }

    void OnDestory()
    {
        if (mBlock != null)
        {
            //put back to EditingLayer
            //ignore raycast
            mBlock.StopToChange();
            //mBlock.OnExitHover -= HandleExitHover;
            mBlock.OnHitCollider -= HandleOnHitCollider;
            //mBlock.OnEnterHover -= HandleOnEnterHover;
        }
        UIManager.instance.Dragging = false;

        //Debug.Log("======================Dragging: " + UIManager.instance.Dragging);
    }
}
