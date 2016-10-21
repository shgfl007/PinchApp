using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.VR;

public class CatalogueUI : MonoBehaviour
{
    private static CatalogueUI mInstance;
    public static CatalogueUI instance { get { return mInstance; } }
    public  static float floorZ = -26.3f;
    public float distance = 100;
    public GameObject uiRoot;
    public RectTransform iconsContainer;
    public GameObject environmentRoot;
    public List<GameObject> buttons = new List<GameObject>();
    public List<GameObject> catalogueButtons = new List<GameObject>();
    public GameObject addPrefab;
    public GameObject creatingCancleMenuLeft;
    public GameObject creatingCancleMenuRight;
    public VRButton cancleButtonLeft;
    public VRButton cancleButtonRight;
    CatalogButton mLastCatalogButton;
    float mDistance;
    Cursor mCursor;
    GameObject mNewItem;
    FreeDrag mFreeDrag;
    Block mBlock;
    FreeDrag mCatalogueFreeDrag;
    public bool showing;
    public GameObject player;
    bool mCreating;

    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
        CreateButtons();

        if (cancleButtonLeft)
        {
            cancleButtonLeft.onClicked += HideCancle;
        }
        if (cancleButtonRight)
        {
            cancleButtonRight.onClicked += HideCancle;
        }
    }

    void ShowCancle()
    {
        if (mCursor == LeftCursor.instance)
        {
            creatingCancleMenuLeft.gameObject.SetActive(true);
        }
        else
        {
            creatingCancleMenuRight.gameObject.SetActive(true);
        }
    }

    void HideCancle(VRObject vrObject, Cursor cursorInput)
    {
        creatingCancleMenuLeft.gameObject.SetActive(false);
        creatingCancleMenuRight.gameObject.SetActive(false);
        if (mNewItem != null)
        {
            mCursor.cursoHold -= HandleItemDropped;
            Destroy(mNewItem);
        }
        if (UIManager.instance)
        {
            UIManager.instance.Dragging = false;
            UIManager.instance.CanOpenMenu(true);
        }
        mCursor = null;
        mCreating = false;
    }

    void CreateButtons()
    {
        if(buttons.Count >0 && uiRoot)
        {
            foreach(GameObject g in buttons)
            {
                GameObject button = (GameObject)Instantiate(g);
                button.transform.SetParent(uiRoot.transform);
                button.transform.localScale = Vector3.one;
                button.transform.localPosition = new Vector3(mDistance, 0, 0);
                button.transform.localRotation = Quaternion.AngleAxis(0, Vector3.zero);
                VRObject vrObject = button.GetComponent<VRObject>();
                if(vrObject)
                {
                    vrObject.onClicked += HandleClicked;
                }
                mDistance += distance;
                catalogueButtons.Add(button);
            }
        }

        iconsContainer.sizeDelta = new Vector2(80 * buttons.Count, 100);
        CloseMenu();
    }

    void HandleClicked(VRObject vrObject,Cursor cursor)
    {
        if (mCursor != null)
        {
            return;
        }
        mCursor = cursor;
        CatalogButton button = vrObject.GetComponent<CatalogButton>();
        if (button)
        {
            HideUI();
            AddItemByPrefab(button);
        }
        if(UIManager.instance)
        {
            UIManager.instance.CanOpenMenu(false);
        }
    }

    public void DuplicateObject(CatalogButton btn, Cursor cursor)
    {
        mCursor = cursor;
        HideUI();
        AddItemByPrefab(btn);
        if (UIManager.instance)
        {
            UIManager.instance.CanOpenMenu(false);
        }
    }

    void AddItemByPrefab(CatalogButton button)
    {
        if (mNewItem == null && mFreeDrag == null && !mCreating)
        {
            mCreating = true;
            mLastCatalogButton = button;
            mNewItem = (GameObject)Instantiate(button.prefab);
            mBlock = mNewItem.GetComponent<Block>();
            mBlock.catalogButton = button;
            mBlock.FreezeClicking();
            Vector3 cursorPos = mCursor.transform.position;
            Vector3 targetPos = new Vector3(cursorPos.x, cursorPos.y, floorZ);
            mNewItem.transform.position = targetPos;
            mNewItem.transform.parent = environmentRoot.transform;
            mNewItem.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            mCursor.cursoHold += HandleItemDropped;
            mFreeDrag = mNewItem.AddComponent<FreeDrag>();
            mFreeDrag.FollowGameObject(mCursor, 0);
            CloseMenu();
            ShowCancle();
            if(UIManager.instance)
            {
                UIManager.instance.UpdateRotationTarget(mNewItem);
            }
        }
    }

    void HandleItemDropped(Cursor c)
    {
        if (mFreeDrag)
        {
            mCreating = false;
            Destroy(mFreeDrag);
            mBlock.BlockDropped(true);
            mCursor.cursoHold -= HandleItemDropped;
            if(mFreeDrag)
            {
                Destroy(mFreeDrag);
            }
        }
        mFreeDrag = null;
        mNewItem = null;
        AddItemByPrefab(mLastCatalogButton);
    }

    public void HandleItemDropped()
    {
        if (mCursor)
        {
            HandleItemDropped(mCursor);
            mCursor = null;
            mCreating = false;
        }
    }

    public void CloseMenu()
    {
        foreach(GameObject g in catalogueButtons)
        {
            g.SetActive(false);
        }
        showing = false;
    }

    public void OpenMenu()
    {
        foreach (GameObject g in catalogueButtons)
        {
            g.SetActive(true);
        }
        uiRoot.gameObject.SetActive(true);
        uiRoot.gameObject.transform.position = Camera.main.transform.position +   Camera.main.transform.forward ;

		//Vector3 temp = uiRoot.gameObject.transform.position;


//		uiRoot.gameObject.transform.position = temp;

        Transform target = Camera.main.transform;
        uiRoot.gameObject.transform.LookAt(uiRoot.transform.position + target.transform.rotation * Vector3.forward,
          target.transform.rotation * Vector3.up);
        showing = true;
    }

    void HandleMove(VRObject vrObject, Cursor cursor)
    {
        mCatalogueFreeDrag = uiRoot.AddComponent<FreeDrag>();
        mCatalogueFreeDrag.FollowGameObject(cursor, -40);
        cursor.cursoHold += HandleCatalogueDropped;
    }

    void HandleCatalogueDropped(Cursor cursor)
    {
        cursor.cursoHold -= HandleCatalogueDropped;
        
        if(mCatalogueFreeDrag)
        {
            Destroy(mCatalogueFreeDrag);
        }
    }

    public void HideUI()
    {
        if (uiRoot)
        {
            uiRoot.SetActive(false);
        }
    }
}
