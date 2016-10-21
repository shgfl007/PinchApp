using UnityEngine;
using System.Collections;

public class Block : VRObject
{
    public delegate void PlaceBlock();
    public delegate void HitCollider(Collision other);
    public event HitCollider OnHitCollider;
    public delegate void DoalUpdate(VRObject vrObject, Cursor cursorInput);
    public delegate void ExitHover(Cursor cursor);
    public event ExitHover OnExitHover;
    public delegate void EnterHover(Cursor cursor);
    public event EnterHover OnEnterHover;
    public event DoalUpdate OnDoalUpdate;
    public delegate void DoalEnd();
    public event DoalEnd OnDoalEnd;
    Cursor mCursor;
    public CatalogButton catalogButton;

    public enum EidtingMode
    {
        DEFAULT,
        OPTIONS,
        POSITION,
        FREEDRAG
    }
    public GameObject arrows;
    public Material selectedMaterial;
    public Material hoverMaterial;
    public Collider hitBox;
    public GameObject optionsPrefab;
    private EidtingMode mEditingMode = EidtingMode.DEFAULT;
    private EditObjectOptions mOptions;
    public SnappingTool snappingTool;
    public GameObject root;
    public MeshRenderer mesh;
    Material mOrigionalMaterial;
    public Rigidbody mRigidBody;
    FreeDrag mFreeDrag;
    bool mSelected;
    EidtingMode mPreviousMode;

    public void EnableFreeDrag(bool enable)
    {
        if(mFreeDrag)
        {
            mFreeDrag.enabled = enabled;
        }
    }

    void Start()
    {
        mOrigionalMaterial = mesh.material;
    }
    
    void IgnoreRayCast(bool changeLayer)
    {
        foreach (Transform g in gameObject.transform)
        {
            g.gameObject.layer = 2;
            if (changeLayer)
            {
                foreach (Transform g2 in g)
                {
                    g2.gameObject.layer = 2;
                }
            }
            else if (g.gameObject.name != "Arrows")
            {
                foreach (Transform g2 in g)
                {
                    g2.gameObject.layer = 2;
                }
            }
        }
        gameObject.layer = 2;
    }

    public void StartToChange(bool changeLayer)
    {
        //ignore raycast
        IgnoreRayCast(changeLayer);

         Rigidbody r = gameObject.GetComponent<Rigidbody>();
        if(r)
        {
            mRigidBody = r;
        }
        if (mRigidBody == null)
        {
            mRigidBody = gameObject.AddComponent<Rigidbody>();
        }
        mRigidBody.useGravity = false;
        mRigidBody.freezeRotation = true;
    }

    public void StopToChange()
    {
        UIManager.instance.Dragging = false;

        SetToEditingLayer();
        if (mRigidBody != null)
        {
            Destroy(mRigidBody);
        }
    }

    public void HideRoot(bool show)
    {
        root.SetActive(show);
    }

    void OnCollisionEnter(Collision other)
    {
        if (OnHitCollider != null)
        {
            OnHitCollider(other);
            //Debug.Log("other: " + other.gameObject.name);
        }
    }

    public override void enterHover(Cursor c)
    {
        if(CatalogueUI.instance.showing)
        {
            return;
        }
        if (RayCast.instance)
        {
            if (RayCast.instance.currentMode == RayCast.MODE.STOPDUAL)
            {
                //Debug.Log("it is scalling, ignore things");
            //CB    return;
            }
        }
        
        if(OnExitHover != null)
        {
            OnExitHover(c);
        }
		print ("It enters the block");


        if (!UIManager.instance.Dragging)
        {
            mesh.material = hoverMaterial;
            mCursor = c;


        }
        base.enterHover(c);
    }

    public override void exitHover(Cursor c)
    {
        if(mSelected)
        {
            mesh.material = selectedMaterial;
            return;
        }
        if (mOrigionalMaterial != null)
        {
            mesh.material = mOrigionalMaterial;
        }
        if(OnExitHover != null)
        {
            OnExitHover(c);
        }
        base.exitHover(c);
    }

    public override void dualCursorUpdate(ref Cursor left, ref Cursor right)
    {
        //Debug.Log("dualCursorUpdate");
        if(OnDoalUpdate != null)
        {
            OnDoalUpdate(this, left);
        }
        base.dualCursorUpdate(ref left, ref right);
    }

    public override void start(ref Cursor cursorInput)
    {
        if (UIManager.instance)
        {
            UIManager.instance.UpdateRotationTarget(gameObject);
        }
        if (CatalogueUI.instance.showing)
        {
            return;
        }
        // if the user is navigating, ignore clicks
        if (RayCast.instance)
        {
            if (RayCast.instance.currentMode == RayCast.MODE.STOPDUAL)
            {
                //Debug.Log("it is scalling, ignore things");
               // return;
            }
        }
        switch (mEditingMode)
        {
            case EidtingMode.DEFAULT:
                if (mSelected)
                {
                    if (mFreeDrag == null)
                    {
                        mFreeDrag = gameObject.AddComponent<FreeDrag>();
                    }
                    UIManager.instance.Dragging = true;
                  //  Debug.Log("======================Dragging: " + UIManager.instance.Dragging);
                    mFreeDrag.FollowGameObject(cursorInput, 0);
                    cursorInput.cursoHold += HandleItemDropped;
                    mPreviousMode = mEditingMode;
                    mEditingMode = EidtingMode.FREEDRAG;
                }
                else
                {
                    // prevent place object and show the option
                    if(!UIManager.instance.MenuAvailable())
                    {
                        return;
                    }
                    if (EditObjectOptions.instance == null)
                    {
                        GameObject obj = (GameObject)Instantiate(optionsPrefab);
                        //obj.transform.localPosition += new Vector3(0.0f, 5.0f, 0.0f);
                        //obj.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                        mOptions = obj.GetComponent<EditObjectOptions>();
                    }
                    else
                    {
                        mOptions = EditObjectOptions.instance;
                    }
                    if (mOptions)
                    {
                        mSelected = true;
                        //Debug.Log("Show Option");
                        mOptions.Init(this);
                        mOptions.onOptionMenuChanged += ChangeOptionMode;
                        mesh.material = selectedMaterial;
                    }
                    mEditingMode = EidtingMode.OPTIONS;
                    if (LeftCursor.instance)
                    {
                        LeftCursor.instance.rayHitObj += HandleClickedElseWhere;
                    }
                    if (RightCursor.instance)
                    {
                        RightCursor.instance.rayHitObj += HandleClickedElseWhere;
                    }
                    if (hitBox)
                    {
                        hitBox.enabled = true;
                    }
                }
                break;
            case EidtingMode.POSITION:
                if (mFreeDrag != null)
                {
                    SetToEditingLayer();
                    Destroy(mFreeDrag);
                }
                if(hitBox)
                {
                    hitBox.enabled = false;
                }
                if (snappingTool)
                {
                    snappingTool.SeekingSnapPoint();
                }
                if (mOptions)
                {
                    mOptions.ListenToClick();
                }
                if (hitBox)
                {
                    hitBox.enabled = true;
                }
                break;
            case EidtingMode.OPTIONS:
                if (mFreeDrag == null)
                {
                    mFreeDrag = gameObject.AddComponent<FreeDrag>();
                }
                mFreeDrag.FollowGameObject(cursorInput, 0);
                cursorInput.cursoHold += HandleItemDropped;
                mOptions.HideOptions(false,false);
                mOptions.onOptionMenuChanged -= ChangeOptionMode;
                mPreviousMode = mEditingMode;
                mEditingMode = EidtingMode.FREEDRAG;
                break;
        }
        base.start(ref cursorInput);
    }

    void SetToEditingLayer()
    {
        // set back to editing layer
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("EditingLayer");
            foreach (Transform g2 in t)
            {
                g2.gameObject.layer = LayerMask.NameToLayer("EditingLayer");
            }
        }
        gameObject.layer = LayerMask.NameToLayer("EditingLayer");
    }

    void HandleItemDropped(Cursor c)
    {
        if (mFreeDrag)
        {
            SetToEditingLayer();
            mOptions.ShowOptions();
            Destroy(mFreeDrag);
            BlockDropped(false);
            c.cursoHold -= HandleItemDropped;
        }
        StopToChange();
    }

    public void Deselected()
    {
        if (mEditingMode != EidtingMode.FREEDRAG)
        {
            mSelected = false;
            if (mRigidBody != null)
            {
                Destroy(mRigidBody);
            }
        }
        if (mOptions != null)
        {
            mOptions.onOptionMenuChanged -= ChangeOptionMode;
        }
        mEditingMode = EidtingMode.DEFAULT;
        arrows.SetActive(false);
        exitHover(mCursor);
    }

    void ChangeOptionMode(bool optionShowing)
    {
    //    Debug.Log("ChangeOptionMode: " + optionShowing);
        if(optionShowing)
        {
            mEditingMode = EidtingMode.OPTIONS;
        }
        else
        {
            if (RayCast.instance.currentMode == RayCast.MODE.NAVIGATE)
            {
                if (mEditingMode != EidtingMode.FREEDRAG)
                {
      //              Debug.Log("Change to POSITION mode: " + mEditingMode);
                    mEditingMode = EidtingMode.POSITION;
                }
            }
        }
    }

    void HandleClickedElseWhere(GameObject obj, Cursor c)
    {
        if(RayCast.instance.currentMode == RayCast.MODE.STOPDUAL)
        {
            return;
        }
        if (obj.layer != LayerMask.NameToLayer("EditingLayer"))
        {
            if (mOptions)
            { 
                switch (mEditingMode)
                {
                    case EidtingMode.OPTIONS:
                        mOptions.HideOptions(true,false);
                        if (UIManager.instance)
                        {
                            UIManager.instance.CanOpenMenu(true);
                        }
                        Deselected();
                        mEditingMode = EidtingMode.DEFAULT;
                        break;
                    case EidtingMode.POSITION:
                        mOptions.ShowOptions();
                        mEditingMode = EidtingMode.OPTIONS;
                        break;
                }
            }
        }
    }

    public void FreezeClicking()
    {
        if(snappingTool)
        {
            snappingTool.SeekingSnapPoint();
        }
        mEditingMode = EidtingMode.FREEDRAG;
    }

    public void BlockDropped(bool deselecBlock)
    {
        //when free drag in editing mode, we don't deselect
        if (deselecBlock)
        {
            Deselected();
            mEditingMode = EidtingMode.DEFAULT;
        }
        else
        {
            mEditingMode = EidtingMode.OPTIONS;
        }
        if (snappingTool)
        {
            snappingTool.StopSeekingSnapPoint();
        }
        SetToEditingLayer();
        UIManager.instance.Dragging = false;
    }

    public override void end()
    {
        base.end();
    }
}
