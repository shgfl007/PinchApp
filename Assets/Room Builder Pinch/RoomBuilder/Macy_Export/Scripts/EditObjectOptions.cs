using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class EditObjectOptions : MonoBehaviour
{
    private static EditObjectOptions mInstance;
    public static EditObjectOptions instance { get { return mInstance; } }
    
    public VRButton rotate;
    public VRButton duplicate;
    public VRButton delete;
    public delegate void HideOption(bool show);
    public event HideOption onOptionMenuChanged;
    public Block currentBlock;
    public GameObject root;
    public GameObject rotationToolPrefab;
    bool mClicked;
    Vector3 mOriginalScale;
    VRObject mPreviousClickable;
    bool mScalling;
    bool mChanging;
    GameObject mPosArrows;
    GameObject mTarget;
    bool mMoving;
    bool mBothClicked;
    bool mScalledOnce;
    RotateTool mRotationXTool;
    //RotateTool mRotationYTool;
    bool mRotating;
    float xRotationScale = 0.4f;
    float yRotationScale = 0.38f;


	bool saBool;
	void switchArrows () {


	}

    void ShowRoot(bool show, bool changeMainUI, bool affectArrows)
    {
        if(root)
        {
            root.SetActive(show);
        }
        if (mPosArrows != null && affectArrows)
        {
			saBool = show;
			mPosArrows.SetActive(show);

			Invoke ("switchArrows", 0.1f);

        }
        if(!show)
        {
            UnRegisterEvents();
        }
        else
        {
            
            mMoving = false;
            RegisterEvents();
            PositionOptions();
        }
        if(!show && UIManager.instance)
        {
            UIManager.instance.CanRotateScene(true);
        }
        if(mRotationXTool)
        {
            mRotationXTool.gameObject.SetActive(false);
            mRotationXTool.onRotating -= HandleRotation;
        }
     /*   if(mRotationYTool)
        {
            mRotationYTool.gameObject.SetActive(false);
            mRotationYTool.onRotating -= HandleRotation;
        }*/
    }

    public void Init(Block block)
    {
        if (currentBlock != null)
        {
            if (currentBlock != block)
            {
                currentBlock.OnDoalUpdate -= ScaleTarget;
                currentBlock.Deselected();
            }
        }
        GameObject arrows = block.arrows;
        if (arrows)
        {
            mPosArrows = arrows;
        }

        if (mRotationXTool == null)
        {
            GameObject rotation = (GameObject)Instantiate(rotationToolPrefab);
            rotation.transform.SetParent(transform);
            mRotationXTool = rotation.GetComponent<RotateTool>();
        }

   /*     if (mRotationYTool == null)
        {
            GameObject rotation = (GameObject)Instantiate(rotationToolPrefab);
            rotation.transform.SetParent(transform);
            mRotationYTool = rotation.GetComponent<RotateTool>();
            mRotationYTool.rotationDir = RotateDir.Y;
        }
        */
       

        mTarget = block.gameObject;
        currentBlock = block;
        currentBlock.OnDoalUpdate += ScaleTarget;
        ShowOptions();

        ResizeRotate();
     //   mRotationYTool.gameObject.SetActive(false);
    }

    public void PositionOptions()
    {
        if (mTarget)
        {
            Vector3 cameraPos = InputTracking.GetLocalPosition(VRNode.Head);
            Vector3 distance = (cameraPos - transform.position) / 10;
            Vector3 smallestScale = new Vector3(4,4,4);
            Vector3 targetScale = new Vector3(Mathf.Abs(distance.z), Mathf.Abs(distance.z), Mathf.Abs(distance.z));
            if(targetScale.x < smallestScale.x)
            {
                targetScale = smallestScale;
            }
         //   transform.localScale = targetScale;
            MeshFilter mesh = mTarget.GetComponent<MeshFilter>();
            float height = mesh.mesh.bounds.size.y * mTarget.transform.localScale.y;
            transform.position = mTarget.transform.position + new Vector3(height/2 - 2.0f, height + 3.0f, 0);
            Vector3 target = new Vector3(cameraPos.x, transform.position.y, cameraPos.z);
            //transform.LookAt(target);
            ResizeRotate();

        }
    }

    void ResizeRotate()
    {
        MeshFilter mesh = mTarget.GetComponent<MeshFilter>();

        float height = mesh.mesh.bounds.size.y *mTarget.transform.lossyScale.y;
        float width = mesh.mesh.bounds.size.x * mTarget.transform.lossyScale.x;
        float depth = mesh.mesh.bounds.size.z * mTarget.transform.lossyScale.z;

        float horizontal = width;
        if(depth>width)
        {
            horizontal = depth;
        }
        if(height>depth)
        {
            horizontal = height;
        }
        mRotationXTool.gameObject.transform.localScale = new Vector3(horizontal * xRotationScale, horizontal * xRotationScale,
                 xRotationScale * horizontal *0.8f);
        mRotationXTool.gameObject.transform.position = mTarget.transform.position;
     /*   mRotationYTool.gameObject.transform.localScale = new Vector3(horizontal * yRotationScale, horizontal * yRotationScale,
                yRotationScale * horizontal * 0.8f);
        mRotationYTool.gameObject.transform.position = mTarget.transform.position;
        mRotationYTool.gameObject.transform.localRotation = Quaternion.AngleAxis(90, Vector3.up);*/
        }

    // Use this for initialization
    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
        RegisterEvents();
    }

    void UnSelectedEditing(Cursor c)
    {
        if (c.hit.collider)
        {
            if (mRotating && c.hit.transform.tag == "Floor")
            {
                HideRotationCircle();
                return;
            }
        }
        if(RayCast.instance.currentMode == RayCast.MODE.STOPDUAL)
        {
            mScalledOnce = true;
            StopScale();
            return;
        }
        if (!mMoving && !mScalling && !mBothClicked)
        {
            if (c.hit.collider)
            {
                if (c.hit.transform.tag == "Floor")
                {
                    LeftCursor.instance.cursoHold -= UnSelectedEditing;
                    RightCursor.instance.cursoHold -= UnSelectedEditing;
                    currentBlock.BlockDropped(true);
                    ShowRoot(false,true, true);
                }
            }
        }
    }

    void ScaleTarget(VRObject vrObject, Cursor cursorInput)
    {
        if (!mScalling)
        {
            mScalledOnce = false;
            mScalling = true;
            mChanging = true;
            mBothClicked = false;
            mOriginalScale = mTarget.transform.localScale;
            //cursorInput.cursoHold += StopScale;
            HideOptions(true,false);
            mPreviousClickable = vrObject;
            if (RayCast.instance)
            {
                RayCast.instance.currentMode = RayCast.MODE.STOPDUAL;
            }
        }
    }

    void DeleteItem(VRObject vrObject, Cursor cursorInput)
    {
        HideOptions(true,false);
        UIManager.instance.CanOpenMenu(true);
        Destroy(mTarget.gameObject);
        if(EditObjectOptions.instance)
        {
            Destroy(EditObjectOptions.instance.gameObject);
        }
    }

    void StopScale()
    {
        //Debug.Log("StopScale");
        ListenToClick();
        RayCast.instance.currentMode = RayCast.MODE.NAVIGATE;
        mScalling = false;
        mChanging = false;
        ShowOptions();
    }

    void DuplicateBlock(VRObject vrObject, Cursor cursorInput)
    {
        if(CatalogueUI.instance)
        {
            CatalogueUI.instance.DuplicateObject(currentBlock.catalogButton, cursorInput);
        }
        HideOptions(true, true);
        if (EditObjectOptions.instance)
        {
            Destroy(EditObjectOptions.instance.gameObject);
        }
    }

    void Clicked(Cursor c)
    {
        if (c.hit.collider)
        {
            Block b = c.hit.collider.gameObject.GetComponent<Block>();
            if (b != null)
            {
                if (b != currentBlock)
                {
                    LeftCursor.instance.cursoHold -= Clicked;
                    RightCursor.instance.cursoHold -= Clicked;
                    currentBlock.Deselected();
                    mMoving = false;
                    mPosArrows.SetActive(false);
                    gameObject.SetActive(true);
                    Init(b);
                    //Debug.Log("Clicked On Other Cube");
                    return;
                }
            }
            if (c.hit.transform.tag == "Floor")
            {
                LeftCursor.instance.cursoHold -= Clicked;
                RightCursor.instance.cursoHold -= Clicked;
                mPosArrows.SetActive(false);
                gameObject.SetActive(true);
                mMoving = false;
            }

            PositionOptions();
        }
    }

    void HandleClicked(Clickable c, Cursor corsor)
    {
        mClicked = false;
    }

    void MoveCounterClockWise(VRObject vrObject, Cursor cursorInput)
    {
        if (mTarget && !mClicked && !mChanging)
        {
            mChanging = true;
            mClicked = true;
            iTween.RotateAdd(mTarget, iTween.Hash("amount", new Vector3(0, -45, 0), "easeType", "easeIn", 
                "onCompleteTarget", gameObject, "oncomplete", "Reset"));
            mPreviousClickable = vrObject;
        }
    }

    void Reset()
    {
        mChanging = false;
        mClicked = false;
        ListenToClick();
    }

    void ShowRotationCircle(VRObject vrObject, Cursor cursorInput)
    {

        rotate.onClicked += HideRotationCircle;
        rotate.onClicked -= ShowRotationCircle;
        mRotating = true;
        if (mRotationXTool)
        {
            mRotationXTool.onRotating += HandleRotation;
            mRotationXTool.gameObject.SetActive(true);
        }
     //   if (mRotationYTool)
      //  {
       //     mRotationYTool.onRotating += HandleRotation;
        //    mRotationYTool.gameObject.SetActive(true);
       // }
        ResizeRotate();
        if (mPosArrows)
        {
            mPosArrows.gameObject.SetActive(false);
        }
        if(UIManager.instance)
        {
            UIManager.instance.CanRotateScene(false);
        }
    }

    void HideRotationCircle(VRObject vrObject, Cursor cursorInput)
    {
        rotate.onClicked -= HideRotationCircle;
        rotate.onClicked += ShowRotationCircle;
        HideRotationCircle();
    }

    void HideRotationCircle()
    {
        mRotating = false;
        if (mRotationXTool)
        {
            mRotationXTool.onRotating -= HandleRotation;
            mRotationXTool.gameObject.SetActive(false);
        }
    //    if (mRotationYTool)
    //    {
    //        mRotationYTool.onRotating -= HandleRotation;
    //        mRotationYTool.gameObject.SetActive(false);
    //    }
        if (mPosArrows)
        {
            mPosArrows.gameObject.SetActive(true);
        }
        if (UIManager.instance)
        {
            UIManager.instance.CanRotateScene(true);
        }
    }

    void HandleRotation(float angle, RotateTool tool)
    {
        switch (tool.rotationDir)
        {
            case RotateDir.X:
                currentBlock.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
                break;
            case RotateDir.Y:
                currentBlock.transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
                break;
        }
    }

    void MoveClockWise()
    {
        if (mTarget && !mClicked &&!mChanging)
        {
            mClicked = true;
            iTween.RotateAdd(mTarget, iTween.Hash("amount", new Vector3(0,45,0), "easeType", "easeIn",
                "onCompleteTarget", gameObject, "oncomplete", "Reset"));
        }
    }

    void Update()
    {
        if(mScalling && mTarget)
        {
            //float x = mScaleCursor.GetCursorObject().transform.position.x;
            if (LeftCursor.instance.clicked && RightCursor.instance.clicked && !mScalledOnce)
            {
                Vector3 left = Camera.main.WorldToScreenPoint(LeftCursor.instance.transform.position);
                Vector3 right = Camera.main.WorldToScreenPoint(RightCursor.instance.transform.position);
                float scale = Mathf.Abs(left.x - right.x) / 500;
                Vector3 newScale = mOriginalScale * scale;
                mTarget.transform.localScale = newScale;
                mBothClicked = true;
                //Debug.Log("mBothClicked: " + mBothClicked);
            }
            if(mBothClicked)
            {
                if(!LeftCursor.instance.clicked || !RightCursor.instance.clicked)
                {
                    //Debug.Log("Released!!!");
                    mBothClicked = false;
                    mScalling = false;
                    StopScale();
                }
            }
        }
    }

    public void HideOptions(bool changeMainUI, bool affectArrows)
    {
        ShowRoot(false, changeMainUI, affectArrows);
        UnRegisterEvents();
        rotate.gameObject.SetActive(false);
        duplicate.gameObject.SetActive(false);
        delete.gameObject.SetActive(false);
        if (onOptionMenuChanged != null)
        {
            onOptionMenuChanged(false);
        }
    }

    public void ShowOptions()
    {
        ShowRoot(true, true, true);
        rotate.gameObject.SetActive(true);
        duplicate.gameObject.SetActive(true);
        delete.gameObject.SetActive(true);
        if (onOptionMenuChanged != null)
        {
            onOptionMenuChanged(true);
        }
        RegisterEvents();
        PositionOptions();
    }

    void RegisterEvents()
    {
        duplicate.onClicked += DuplicateBlock;
        delete.onClicked += DeleteItem;
        rotate.onClicked += ShowRotationCircle;
        LeftCursor.instance.cursoHold += UnSelectedEditing;
        RightCursor.instance.cursoHold += UnSelectedEditing;
    }

    void UnRegisterEvents()
    {
        mClicked = false;
        mChanging = false;
        duplicate.onClicked -= DuplicateBlock;
        delete.onClicked -= DeleteItem;
        rotate.onClicked -= HideRotationCircle;
        rotate.onClicked -= ShowRotationCircle;
        LeftCursor.instance.cursoHold -= UnSelectedEditing;
        RightCursor.instance.cursoHold -= UnSelectedEditing;
        if (mPreviousClickable)
        {
           // mPreviousClickable.IgnoreClick();
            mPreviousClickable = null;
        }
        if(mRotationXTool)
        {
            mRotationXTool.onRotating -= HandleRotation;
        }
      //  if (mRotationYTool)
       // {
        //    mRotationYTool.onRotating -= HandleRotation;
       // }
    }

    void OnDestory()
    {
        UnRegisterEvents();
    }

    public void ListenToClick()
    {
        if(mPreviousClickable)
        {
            //mPreviousClickable.ListenToClick();
        }
    }

}
