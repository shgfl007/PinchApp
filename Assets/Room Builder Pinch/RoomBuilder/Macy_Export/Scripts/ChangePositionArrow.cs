using UnityEngine;
using System.Collections;

public class ChangePositionArrow : VRButton
{
    static float floorY = -20;
    /// <summary>
    /// move objects closer to the camera.
    /// </summary>
    static float addZDistance = 4;
    public Direction positionDirection;
    public VisualDirection visualDirection;
    public Block target;
    public float speed = 0.5f;
    public GameObject arrowPrefab;
    public Material arrowNormalMaterial;
    public Material arrowSelectedMaterial;
    Cursor mCursor;
    Vector3 mWorkingPos;
    bool mUpdate;
    bool mSaved;
    float mX;
    float mZ;
    float mY;
    bool mHitOther;
    MeshRenderer mArrow;
    Vector3 mArrowScale = new Vector3(15, 15, 15);
    Vector3 mStartDistance = Vector3.zero;
    Vector3 mDistance;
    Transform mPartent;

    void Start()
    {
        if(arrowPrefab)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.localScale = mArrowScale;
            mArrow = arrow.GetComponent<MeshRenderer>();
            switch (visualDirection)
            {
                case VisualDirection.Left:
                    arrow.transform.localRotation = Quaternion.AngleAxis(90, Vector3.up);
                    break;
                case VisualDirection.Right:
                    arrow.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.up);
                    break;
                case VisualDirection.Up:
                    arrow.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);
                    break;
                case VisualDirection.Down:
                    arrow.transform.localRotation = Quaternion.AngleAxis(90, Vector3.right);
                    break;
                case VisualDirection.Forward:
                    arrow.transform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
                    break;
                case VisualDirection.Backward:
                    arrow.transform.localRotation = Quaternion.AngleAxis(180, -Vector3.up);
                    break;
            }
            mPartent = transform.parent;
        }
    }

    public enum Direction
    {
        X,
        Y,
        Z
    }

    public enum VisualDirection
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Backward
    }

    void OnDisable()
    {
        endMove();
    }
    
    public override void start(ref Cursor cursorInput)
    {
        if (mCursor == null)
        {
            base.start(ref cursorInput);
            if (CatalogueUI.instance)
            {
                CatalogueUI.instance.HideUI();
            }
            mUpdate = true;
            if (EditObjectOptions.instance)
            {
                EditObjectOptions.instance.HideOptions(true, false);
            }
            if (UIManager.instance)
            {
                UIManager.instance.CanRotateScene(false);
            }
            gameObject.tag = "Arrow";
            target.EnableFreeDrag(false);
            target.StartToChange(false);
            target.OnHitCollider += HandleHitCollider;
            gameObject.transform.SetParent(target.transform.parent);
            mWorkingPos = transform.position;
            mStartDistance = gameObject.transform.position - target.transform.position;
            mHitOther = false;
            mCursor = cursorInput;
        }
    }

    public override void end()
    {
        Vector3 pos = gameObject.transform.position;
        gameObject.tag = "Untagged";
        gameObject.transform.SetParent(mPartent);
        gameObject.transform.position = pos;
        mCursor = null;
        base.end();
        endMove();
        if (UIManager.instance)
        {
            UIManager.instance.CanRotateScene(true);
        }
        target.EnableFreeDrag(true);
    }
    
    void endMove()
    {
        mUpdate = false;
        mSaved = false;
        mHitOther = false;
        mCursor = null;
        target.EnableFreeDrag(true);
        target.StopToChange();
        target.OnHitCollider -= HandleHitCollider;
        if (EditObjectOptions.instance)
        {
            EditObjectOptions.instance.ShowOptions();
        }
    }

    void HandleHitCollider(Collision other)
    {
        mHitOther = true;
    }

    public override void enterHover(Cursor c)
    {
        if (mArrow)
        {
            mArrow.sharedMaterial = arrowSelectedMaterial;
        }
        base.enterHover(c);
    }

    public override void exitHover(Cursor c)
    {
        if (mArrow)
        {
            mArrow.sharedMaterial = arrowNormalMaterial;
        }
        base.exitHover(c);
    }

    public void Update()
    {
        if (mCursor != null)
        {
            if (!mCursor.hold && mUpdate)
            {
                mCursor = null;
                endMove();
                if (UIManager.instance)
                {
                    UIManager.instance.CanRotateScene(true);
                }
                target.EnableFreeDrag(true);
                return;
            }

            if (mCursor.hit.transform.gameObject.tag == "Arrow")
            {
                if (mHitOther)
                {
                    transform.position = mWorkingPos;
                }
                if (mUpdate)
                {

                    Vector3 hit = mCursor.hit.point;
                    switch (positionDirection)
                    {
                        case Direction.X:
                            gameObject.transform.position = new Vector3(hit.x, gameObject.transform.position.y, gameObject.transform.position.z);
                            break;
                        case Direction.Y:
						gameObject.transform.position = new Vector3(gameObject.transform.position.x, hit.y, gameObject.transform.position.z);
                            break;
                        case Direction.Z:
                            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, hit.z);
                            break;
                    }
                    target.transform.position = gameObject.transform.position - mStartDistance;
                    mWorkingPos = transform.position;
                }
            }
        }
    }
}
