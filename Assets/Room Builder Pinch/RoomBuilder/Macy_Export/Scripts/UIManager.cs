using UnityEngine;
using UnityEngine.VR;

public class UIManager : MonoBehaviour
{
    private static UIManager mInstance;
    public static UIManager instance { get { return mInstance; } }
    public GameObject leftCreationMenu;
    public VRButton closeButton;
    public VRButton addButton;
    public VRButton viewButton;


	public VRButton homeButton;

    public GameObject everythingContainer;
    public GameObject player;
    public bool Dragging;
    bool mCanOpen = true;
    bool mCanRotate;
    float mWaitForOpen;
    bool mLeftDeselected;
    bool mRotating;
    Vector3 mTargetPos;
    Vector3 mSwimStartPos;
    bool mCanMove;
    Quaternion mAngle;
    GameObject mRotateTarget;
    bool moveForward = true;
    bool moveY = true;
    Cursor mCursor;

    public void UpdateRotationTarget(GameObject target)
    {
        mRotateTarget = target;
    }

    // Use this for initialization
    void Start()
    {

        if (mInstance == null)
        {
            mInstance = this;
        }
		//Init();


    }

    void Update()
    {
        if (mLeftDeselected && LeftCursor.instance.hold)
        {
            // no waiting for closing menu
            if (leftCreationMenu.gameObject.activeInHierarchy)
            {
                mWaitForOpen = 0.3f;
            }
            if (mCanMove)
            {
                mWaitForOpen = 0;
            }
            mWaitForOpen += Time.deltaTime;
            if (mWaitForOpen >= 0.3f)
            {
                OpenMenu();
            }
            //  Debug.Log("mWaitForOpen: "+ mWaitForOpen);
        }
        else
        {
            mWaitForOpen = 0;
        }
        if (mRotating && mCanRotate && mCursor != null)
        {
            if (mCursor.hold)
            {
                if (!mCanMove)
                {
                    Vector3 distance = mSwimStartPos - mCursor.transform.position;
                    if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                    {
                        moveY = false;
                    }
                    else
                    {
                        moveY = true;
                    }
                    if (moveY)
                    {
                        if (distance.y < 0)
                        {
                            moveForward = false;
                        }
                        else
                        {
                            moveForward = true;
                        }
                        if (Mathf.Abs(distance.y) > 0.1f)
                        {
                            mCanMove = true;
                        }
                    }
                    else
                    {
                        if (distance.x < 0)
                        {
                            moveForward = false;
                        }
                        else
                        {
                            moveForward = true;
                        }
                        if (Mathf.Abs(distance.x) > 0.1f)
                        {
                            mCanMove = true;
                        }
                    }
                }
                if (mCanMove)
                {
                    float angleY = 0;
                    float angleZ = 0;
                    if (!moveY)
                    {

                        //cursorPos = new Vector3(cursorPos.x, player.transform.position.y, player.transform.position.z);
                    }
                    if (moveForward)
                    {
                        if (mCursor == LeftCursor.instance)
                        {
                            angleY = mAngle.y;
                        }
                        else
                        {
                            angleZ = -mAngle.z;
                            angleY = -mAngle.y;
                        }
                        // player.transform.position = Vector3.MoveTowards(player.transform.position, cursorPos, Time.deltaTime * 30);
                    }
                    else
                    {
                        if (mCursor == RightCursor.instance)
                        {
                            angleZ = mAngle.z;
                            angleY = mAngle.y;
                        }
                        else
                        {
                            angleY = -mAngle.y;
                        }
                        //player.transform.position = Vector3.MoveTowards(player.transform.position, -cursorPos, Time.deltaTime * 30);
                    }
                    //Vector3 XOnly = mAngle.eulerAngles;
                    //EditObjectOptions.instance.currentBlock.transform.position;
                    // prevent menu open
                    mWaitForOpen = 0.0f;
                    //player.transform.rotation = Quaternion.AngleAxis(XOnly.y, Vector3.up);
                    if (mRotateTarget != null)
                    {
                        //float angle = Vector3.Angle(cursorPos, mRotateTarget.transform.position);
                        //everythingContainer.transform.Rotate(Vector3.up, angle);
                    }
                    Quaternion zlock = mAngle;
                    zlock.z = 0;

                    Vector3 dir = mCursor.transform.position - mSwimStartPos;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    //Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.right);
                    if (moveForward)
                    {
                        everythingContainer.transform.eulerAngles = new Vector3(0, angle * 0.2f, 0);
                    }
                    else
                    {
                        everythingContainer.transform.eulerAngles = new Vector3(angle * 0.2f, 0, 0);
                    }
                }
            }
            else
            {
                mCanMove = false;
                //cursorPos = Vector3.zero;
                mRotating = false;
            }
        }
    }


    public void Init()
    {
        TrackerScript.instance.leftCursor.Deselected += LeftDeselected;
        TrackerScript.instance.rightCursor.Deselected += RotateEnvironmentRight;

        if (closeButton)
        {
            closeButton.onClicked += CloseMenu;
        }
        if (addButton)
        {
            addButton.onClicked += OpenCatalogue;
        }
        if (viewButton)
        {
            viewButton.onClicked += ToggleView;
        }

		if (homeButton) {

			homeButton.onClicked += goHome;
		}

    }

	void goHome(VRObject v, Cursor c) {
		
	}

    void RotateEnvironmentRight()
    {
        mCursor = RightCursor.instance;


        RotateEnvironment();
    }

    void RotateEnvironment()
    {
        if (mCanRotate)
        {
            if (!mRotating)
            {
                GameObject hitObj = mCursor.hit.collider.gameObject;
                if (hitObj.tag == "Floor")
                {
                    mRotating = true;
                }
                mSwimStartPos = mCursor.transform.position;


            }

        }
    }

    void OpenCatalogue(VRObject vrObject, Cursor cursorInput)
    {
        if (CatalogueUI.instance)
        {
            CatalogueUI.instance.OpenMenu();
            TrackerScript.instance.leftCursor.Deselected -= LeftDeselected;
            leftCreationMenu.gameObject.SetActive(false);
        }
    }

    void LeftDeselected()
    {
        if (mCanOpen)
        {
            mLeftDeselected = true;
        }
        if (mCanRotate)
        {
            mRotating = false;
        }
    }

    void OpenMenu()
    {
        mLeftDeselected = false;
        mWaitForOpen = 0;
        if (leftCreationMenu.gameObject.activeInHierarchy)
        {
            leftCreationMenu.gameObject.SetActive(false);
        }
        else
        {
            leftCreationMenu.gameObject.SetActive(true);
        }
        if (CatalogueUI.instance)
        {
            CatalogueUI.instance.CloseMenu();
        }
      //  Debug.Log("Menu Opens");

    }

    void CloseMenu(VRObject vrObject, Cursor cursorInput)
    {
        leftCreationMenu.gameObject.SetActive(false);
        Dragging = false;
    //    Debug.Log("======================Dragging: " + Dragging);
    }

    void CloseCatalogue()
    {
        TrackerScript.instance.leftCursor.Deselected += LeftDeselected;
        if (CatalogueUI.instance)
        {
            CatalogueUI.instance.CloseMenu();
        }
    }

    public void CanOpenMenu(bool canOpen)
    {
        if (canOpen)
        {
            TrackerScript.instance.leftCursor.Deselected += LeftDeselected;
        }
        mCanOpen = canOpen;
    }

    public bool MenuAvailable()
    {
        return mCanOpen;
    }

    void OnDestory()
    {
        TrackerScript.instance.leftCursor.Deselected -= LeftDeselected;
        TrackerScript.instance.rightCursor.Deselected -= RotateEnvironmentRight;
    }

    public void CanRotateScene(bool canRotate)
    {
        mCanRotate = canRotate;
    }

    void ToggleView(VRObject vrObject, Cursor cursorInput)
    {
        leftCreationMenu.gameObject.SetActive(false);
        SpaceManager.instance.toggleTopDown = false;
        SpaceManager.instance.toggleCamView();
        Dragging = false;

    }

}
