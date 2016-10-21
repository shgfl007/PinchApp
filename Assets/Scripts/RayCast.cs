using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.VR;
using UnityEngine.SceneManagement;


public class RayCast : MonoBehaviour {
    float cursorScale_x, cursorScale_y, cursorScale_z;

    private static RayCast mInstance;
	public static RayCast instance { get { return mInstance; } }
	public  enum MODE
	{
		STOPDUAL,
		NAVIGATE
	}
	[HideInInspector]
	public MODE currentMode = MODE.STOPDUAL;

	public enum SWIMMODE
	{
		MOVE,
		ROTATE
	}

	[Header("Navigation Boundries")]
	public bool clampX = false;
	public float minX, maxX;
	public bool clampY = false;
	public float minY, maxY;
	public bool clampZ = false;
	public float minZ, maxZ;
	//public float maxZ, minZ, minY, maxY, minX, maxX;


	[Header("if Everything container is cardboard main")]
	public bool cameraControl = false; // if Everything container is cardboard main


    [Header("Pinch & Zoom")]
	public bool StopZooming = false;


	[Header("Tracker & Everything Container")]
	public GameObject tracker;
	public GameObject everythingContainer; // the object that moves on pinch and zoom


	[Header("Swimming Parameters")]
	public float swimSpeed = 60;
	public bool postInertia = false;
	public float decelerationFactor = 100;
	public int deceleratedAmount = 20;
	//----------------------------------------------
	// FIELD
	//----------------------------------------------

	//float grabDistance = 5f; // Standardized for the scene
	//float verticalScrollAmplification = 700;
	//float horizontalScrollAmplification = 1500;
	[HideInInspector]
	public  Cursor right, left;
	[HideInInspector]
	public GameObject cameraObject;


	float originalLeftToRightLength;
	private TrackerScript trackerScript;
	// Clickable mHovered;
	// Hold Modes
	const int VERTICAL_SCROLL_MODE = 0;
	const int HORIZONTAL_SCROLL_MODE = 1;
	const int MOVE_MODE = 2;
	const int BUTTON_MODE = 3;

	// Swimming parameters
	SwimmingParams swimmingParams = new SwimmingParams();



	Tweener mTween;
	[HideInInspector]
	public SWIMMODE swimMode = SWIMMODE.MOVE;
	bool mInited;
	bool mCanRotate = true;
	bool mDeselected;





	//----------------------------------------------
	// INITIALIZATION
	//----------------------------------------------

	// Use this for initialization
	void Start () {
		if (mInstance == null)
		{
			mInstance = this;
		}

		swimmingParams.swimSpeed = swimSpeed;
		swimmingParams.postInertia = postInertia;
		swimmingParams.decelerationFactor = decelerationFactor;
		swimmingParams.deceleratedAmount = deceleratedAmount;
	}

	public void Init(Cursor leftCursorObject, Cursor rightCursorObject)
	{
		right = rightCursorObject;

		left = leftCursorObject;

		swimmingParams.vectorMoved = Vector3.zero;
		mInited = true;

        cursorScale_x = LeftCursor.instance.gameObject.transform.localScale.x;
        cursorScale_y = LeftCursor.instance.gameObject.transform.localScale.y;
        cursorScale_z = LeftCursor.instance.gameObject.transform.localScale.z;
    }

	//----------------------------------------------
	// PER FRAME CODE
	//----------------------------------------------

	void Update() {
		if (mInited)
		{
			setCursors();


			//--------------------------------------------------------------
			// DUAL CURSOR OVERRIDE
			//--------------------------------------------------------------

			try
			{
				if (left.click && right.click)
				{
					// End holds abruptly
					left.hold = false;
					right.hold = false;

					// Updates necessary cursor information
					left.previousCursorPos = left.currentCursorPos;
					right.previousCursorPos = right.currentCursorPos;
					left.currentCursorPos = left.transform.localPosition;
					right.currentCursorPos = right.transform.localPosition;

					if (!swimmingParams.dualCursorInit && RayCast.instance.currentMode == MODE.NAVIGATE)
					{
						if (swimmingParams.dualCursorOverride)
						{
							swimmingParams.postSwim = true;
							// Do dual cursor stuff
							swimmingParams.dualCursorObject.GetComponent<VRObject>().dualCursorUpdate(ref left, ref right);
						}
						else {
							// Set states
							if (currentMode == MODE.NAVIGATE)
							{
#if UNITY_EDITOR
                                //Debug.Log("currentMode: " + currentMode);
#endif
                                swimmingParams.postSwim = true;
								swimmingParams.postInertia = true;
								swimmingParams.deceleratedAmount = 0;
								// Do swim
								if (swimMode == SWIMMODE.MOVE)
								{
									swimmingParams.vectorMoved = SwimMove();
								}
								else
								{
									SwimRotate();
								}
								swimmingParams.deceleration = swimmingParams.vectorMoved / swimmingParams.decelerationFactor;
								//  if (EditObjectOptions.instance)
								// {
								//   EditObjectOptions.instance.PositionOptions();
								//}
							}
						}
						return;
					}
					else {
						// Do initialization
						// Get the hover transform
						doRayCast(ref right);
						doRayCast(ref left);
						right.objectHitByRay = false;
						left.objectHitByRay = false;

						// Check dual cursor mode
						if (left.hoveringTransform.tag == "DualCursor" && right.hoveringTransform.tag == "DualCursor")
						{
							swimmingParams.dualCursorOverride = true;
							swimmingParams.dualCursorObject = left.hoveringTransform.gameObject;
						}
						// Check if super fast swim mode
						else if (left.hoveringTransform.tag == "BoardBlocker" && right.hoveringTransform.tag == "BoardBlocker")
						{
							// Set speed to super fast
							swimmingParams.swimSpeed = swimmingParams.fastSpeed;
							swimmingParams.dualCursorOverride = false;
						}
						// Default swim mode
						else {
							swimmingParams.dualCursorOverride = false;
						}

						// End initialization
						swimmingParams.dualCursorInit = false;
						return;
					}
				}

				// Do inertia if the swim speed is normal
				if (swimmingParams.postInertia && swimmingParams.swimSpeed != swimmingParams.fastSpeed)
				{
					everythingContainer.transform.position = everythingContainer.transform.position + swimmingParams.vectorMoved;
					swimmingParams.vectorMoved -= swimmingParams.deceleration;
					swimmingParams.deceleratedAmount++;
#if UNITY_EDITOR
                    //Debug.Log(swimmingParams.vectorMoved.magnitude);
#endif
                    if (swimmingParams.deceleratedAmount == swimmingParams.decelerationFactor)
						swimmingParams.postInertia = false;
				}

				// If swim speed is fast, set it back to normal, turn off postInertia, and move to the nearest managed interface
				if (!swimmingParams.dualCursorInit && swimmingParams.swimSpeed == swimmingParams.fastSpeed)
				{
					swimmingParams.swimSpeed = swimmingParams.regularSpeed;
					swimmingParams.postInertia = false; // No inertia if going super fast

                    // Move to nearest managed interface
#if UNITY_EDITOR
                    //Debug.Log("Here moves around");
#endif
                    //GameObject.Find("EverythingContainer").GetComponent<EverythingContainerScript>().goToNearestManagedInterface();
                }
                swimmingParams.dualCursorInit = true;

				// Do not continue after swimming until both cursors are unclicked
				if (swimmingParams.postSwim)
				{
					if (!left.click && !right.click)
					{
						swimmingParams.postSwim = false;
					}
					return;
				}
			}
			catch (NullReferenceException e)
			{
#if UNITY_EDITOR
                Debug.LogError(e);
#endif
            }


			//--------------------------------------------------------------
			// REGULAR CURSOR CODE
			//--------------------------------------------------------------
			if (left)
			{
				if (left.GetComponent<Renderer>().enabled)
					left.doRayCast = true;
				else {
					left.doRayCast = false;
					left.interactionMode = -1;
				}
			}
			if (right)
			{
				if (right.GetComponent<Renderer>().enabled)
					right.doRayCast = true;
				else {
					right.doRayCast = false;
					right.interactionMode = -1;
				}
			}

			try
			{
				// Turn off the hold only if the click ends
				// Actually ends hold one frame after the click ends to detect the frame at which the cursor releases

				// Left
				if (!left.click && !left.previousClickState && left.hold && left.heldTransform != null)
				{
					VRObject objectScript = (VRObject)left.heldTransform.gameObject.GetComponent<VRObject>();
					if (objectScript != null)
					{
						objectScript.end();
						if (left.distMoved < 6000)
						{
							objectScript.onClick();
						}
					}
					left.hold = false;
				}
				else if (!left.click && !left.previousClickState && left.hold)
					left.hold = false;

				// Right
				if (!right.click && !right.previousClickState && right.hold && right.heldTransform != null)
				{
					VRObject objectScript = (VRObject)right.heldTransform.gameObject.GetComponent<VRObject>();
					if (objectScript != null)
					{
						objectScript.end();
						if (right.distMoved < 6000)
						{
							objectScript.onClick();
						}
					}
					right.hold = false;
				}
				else if (!right.click && !right.previousClickState && right.hold)
					right.hold = false;
			}
			catch (MissingReferenceException e)
			{
#if UNITY_EDITOR
                Debug.Log("exception: " +e);
#endif
            }

			//--------------------------------------------------------------
			// RAY CASTING
			//--------------------------------------------------------------

			doRayCast(ref left);
			doRayCast(ref right);


			//--------------------------------------------------------------
			// STARTING NEW HOLD
			//--------------------------------------------------------------

			// Start a new hold if there wasn't already one
			if (left.objectHitByRay && !left.hold && left.click)
			{
				startNewHold(ref left, ref left.hit);
			}
			if (right.objectHitByRay && !right.hold && right.click)
			{
				startNewHold(ref right, ref right.hit);
			}


			//--------------------------------------------------------------
			// HOVERING
			//--------------------------------------------------------------

			try
			{
				if (right.previousHoveringTransform != null)
				{
					if (right.hoveringTransform == right.previousHoveringTransform && right.hoveringTransform != null)
					{
						VRObject rightObjectScript = (VRObject)right.hoveringTransform.gameObject.GetComponent<VRObject>();
						if (rightObjectScript != null && !rightObjectScript.hovering)
						{
							rightObjectScript.enterHover(RightCursor.instance);
							rightObjectScript.hovering = true;
						}
					}
					else if (right.previousHoveringTransform.gameObject != null)
					{
						VRObject rightObjectScript = (VRObject)right.previousHoveringTransform.gameObject.GetComponent<VRObject>();
						if (rightObjectScript != null)
						{
							rightObjectScript.exitHover(RightCursor.instance);
							rightObjectScript.hovering = false;
						}
					}
				}
				if (left.previousHoveringTransform != null)
				{
					if (left.hoveringTransform == left.previousHoveringTransform && left.hoveringTransform != null)
					{
						VRObject leftObjectScript = (VRObject)left.hoveringTransform.gameObject.GetComponent<VRObject>();
						if (leftObjectScript != null && !leftObjectScript.hovering)
						{
							leftObjectScript.enterHover(LeftCursor.instance);
							leftObjectScript.hovering = true;
						}
					}
					else if (left.previousHoveringTransform.gameObject != null)
					{
						VRObject leftObjectScript = (VRObject)left.previousHoveringTransform.gameObject.GetComponent<VRObject>();
						if (leftObjectScript != null)
						{
							leftObjectScript.exitHover(LeftCursor.instance);
							leftObjectScript.hovering = false;
						}
					}
				}
			}
			catch (MissingReferenceException e)
			{
#if UNITY_EDITOR
                Debug.Log("Exception: Object script not found: " + e);
#endif
            }


			//--------------------------------------------------------------
			// RUN HOLD SCRIPT
			//--------------------------------------------------------------

			// If both are holding the same object, then it must mean rotation or pinch to zoom
			if (left.hold && right.hold && right.heldTransform == left.heldTransform)
			{
				if (left.interactionMode == MOVE_MODE && right.interactionMode == MOVE_MODE)
				{
#if UNITY_EDITOR
                    Debug.Log("both hit");
#endif
                }
			}
			else {
				if (left.hold)
				{
					singleHold(ref left);
				}
				if (right.hold)
				{
					singleHold(ref right);
				}
			}
		}
	}


	//--------------------------------------------------------------
	// HELPER FUNCTIONS
	//--------------------------------------------------------------
	void setCursors() {
		// Converts points for Unity integration
		trackerScript = tracker.GetComponent<TrackerScript> ();

		// Set cursor positions
		if(mTween == null)
		{
			mTween = gameObject.AddComponent<Tweener>();
		}
		if (left)
		{
            mTween.tween(left.gameObject, "local position", new Vector3(-left.x * 1.8f, left.y * 4f, 0), 0.14f, 0f, Ease.Linear, false); // leftX * 2f
        }
		if (right)
		{
            mTween.tween(right.gameObject, "local position", new Vector3(-right.x * 1.8f, right.y * 4f, 0), 0.14f, 0f, Ease.Linear, false);
        }
		// Get cursor info
		left.previousClickState = left.click;
		right.previousClickState = right.click;
		if (trackerScript != null && LeftCursor.instance != null && RightCursor.instance != null)
		{
			left.click = left.getClickState();
#if UNITY_EDITOR
            //Debug.Log("left.click: "+ left.click);
#endif
            right.click = right.getClickState();
			// Disappear cursors if they aren't found
			if (left)
			{
				Renderer leftRenderer = left.GetComponent<Renderer>();
				if (leftRenderer)
				{
					if (!LeftCursor.instance.cursorFound() && leftRenderer.enabled == true)
					{
						leftRenderer.enabled = false;
						left.handPuppet.gameObject.SetActive(false);
					}
					else if (LeftCursor.instance.cursorFound() && leftRenderer.enabled == false)
					{
						leftRenderer.enabled = true;
						left.handPuppet.gameObject.SetActive(true);
					}
				}
                float leftRot = (left.x - 340) * 0.04f;
                if (left.handPuppet)
                {
					left.handPuppet.localRotation = Quaternion.Euler(new Vector3(0, 0, leftRot));
                    LeftCursor.instance.gameObject.transform.localScale = new Vector3(cursorScale_x * left.depthScale, cursorScale_y * left.depthScale, cursorScale_z * left.depthScale);
                }
			}
			if (right)
			{ 
				Renderer rightRenderer = right.GetComponent<Renderer>();
				if (rightRenderer)
				{
					if (!trackerScript.rightCursor.cursorFound() && rightRenderer.enabled == true)
					{
						rightRenderer.enabled = false;
						right.handPuppet.gameObject.SetActive(false);
					}
					else if (trackerScript.rightCursor.cursorFound() && rightRenderer.enabled == false)
					{
						rightRenderer.enabled = true;
						right.handPuppet.gameObject.SetActive(true);
					}
                    // Set hand puppet rotation

                    float rightRot = (right.x + 340) * 0.04f;

                    right.handPuppet.localRotation = Quaternion.Euler(new Vector3(0, 0, rightRot));
                    RightCursor.instance.gameObject.transform.localScale = new Vector3(cursorScale_x * right.depthScale, cursorScale_y * right.depthScale, cursorScale_z * right.depthScale);
                }
			}


		}
	}

	float getYRotationAngle(Vector3 position) {
		// Vectors in the x-z plane
		Vector3 xzRay = new Vector3(position.x, 0, position.z);
		Vector3 xzForward = new Vector3(0, 0, 1);
		// Acute angle between the two vectors
		float yRotationAngle = Vector3.Angle(xzForward, xzRay);
		if (xzRay.x < 0) {
			yRotationAngle = - yRotationAngle;
		}

		return yRotationAngle;
	}

	void startNewHold(ref Cursor cursor, ref RaycastHit raycastHit) {
		cursor.hold = true;
		cursor.previousHeldTransform = cursor.heldTransform;
		cursor.heldTransform = raycastHit.transform;
		cursor.previousCursorPos = cursor.transform.localPosition;

		try {
			if (cursor.heldTransform != null) {
				cursor.CursorHold();
				VRObject objectScript = (VRObject)cursor.heldTransform.gameObject.GetComponent <VRObject>();
				if (objectScript != null) {
					objectScript.start (ref cursor);
					cursor.distMoved = 0;
				}
			}
		}
		catch (MissingReferenceException e) {
#if UNITY_EDITOR
            Debug.Log("exception: " + e);
#endif
        }
    }

	void singleHold(ref Cursor cursor) {
		Vector3 newCursorPos = cursor.transform.localPosition;

		try {
			if (cursor.heldTransform != null) {
				VRObject objectScript = (VRObject)cursor.heldTransform.gameObject.GetComponent <VRObject>();
				if (objectScript != null) {
					objectScript.AfterUpdate();
					cursor.distMoved += (cursor.previousCursorPos - cursor.currentCursorPos).magnitude;
				}
			}
		}
		catch (MissingReferenceException e) {
#if UNITY_EDITOR
            Debug.Log("exception: " + e);
#endif
        }

		cursor.previousCursorPos = newCursorPos;
	}

	void doRayCast(ref Cursor cursor) {

		Vector3 cursorScreen = Camera.main.WorldToScreenPoint(cursor.transform.position);
		Ray screenRay = Camera.main.ScreenPointToRay(cursorScreen);

		cursor.ray = screenRay;
		cursor.objectHitByRay = false;
		if (Physics.Raycast(cursor.ray, out cursor.hit))
		{
			if (cursor.rayHitObj != null && cursor.click && !cursor.canCallRelease)
			{
				cursor.rayHitObj(cursor.hit.collider.gameObject, cursor);
				cursor.canCallRelease = true;
			}

			if ((cursor.hit.collider.gameObject.tag == "Floor" || cursor.hit.collider.gameObject.tag == "Wall") && !cursor.hold && cursor.clicked)
			{
#if UNITY_EDITOR
                //Debug.Log(cursor.hit.collider.gameObject.tag);
#endif
                if (cursor.Deselected != null)
				{
					mDeselected = true;
					cursor.Deselected();
					//                    Vector3 cameraPos = Camera.main.WorldToScreenPoint(cursor.transform.position);
				}
			}
			else if ((cursor.hit.collider.gameObject.tag != "Floor" && cursor.hit.collider.gameObject.tag != "Wall") && !cursor.hold && cursor.clicked)
			{
				mDeselected = false;
			}
			else if(mDeselected && cursor.hold)
			{
				Vector3 relativePos = cursor.transform.position - InputTracking.GetLocalPosition(VRNode.Head);
				Quaternion rotation = Quaternion.LookRotation(relativePos);
				if (cursor.OnCursorDeselectedHold!=null)
				{
					cursor.OnCursorDeselectedHold(rotation);
				}
			}

			cursor.previousHoveringTransform = cursor.hoveringTransform;

			if (cursor.doRayCast)
			{
				cursor.hoveringTransform = cursor.hit.transform;
			}
			else
			{
				cursor.hoveringTransform = null;
			}
			if (cursor.doRayCast && !cursor.hold)
			{
				cursor.objectHitByRay = true;
			}
		}
	}

	// Returns the direction and distance moved
	void SwimRotate() 
	{
		if (mCanRotate)
		{
			// Do move
			Vector3 averageCurrentPos = (left.transform.localPosition + right.transform.localPosition) / 2;
			Vector3 averagePrevPos = (left.previousCursorPos + right.previousCursorPos) / 2;
			Vector3 posDiff = averageCurrentPos - averagePrevPos;

			GameObject head = null;
			if (HeadRotation.instance)
			{
				head = Camera.main.gameObject;
			}
			Vector3 sidewayDirection = head.transform.right.normalized;
			Vector3 vectorToMove = posDiff.x * sidewayDirection;
			float x = vectorToMove.x;
			mCanRotate = false;
			iTween.RotateAdd(everythingContainer, iTween.Hash("amount", new Vector3(0, x*30, 0), "easeType", "easeIn",
				"onCompleteTarget", gameObject, "oncomplete", "Reset"));
		}
	}

	void Reset()
	{
		mCanRotate = true;
	}

	Vector3 SwimMove()
	{


		Vector3 newEverythingPosition = everythingContainer.transform.position;
		Vector3 originalEverythingPosition = everythingContainer.transform.position;

		if (!StopZooming) {
			// Do zoom
			Vector3 currentDist = left.transform.localPosition - right.transform.localPosition;
			Vector3 prevDist = left.previousCursorPos - right.previousCursorPos;
			float magnitudeDiff = currentDist.magnitude - prevDist.magnitude;

			if (cameraControl) {
				magnitudeDiff *= -1;
			}
			if (HeadRotation.instance) {
				Vector3 forwardDirection = Camera.main.gameObject.transform.forward;
				newEverythingPosition = newEverythingPosition - forwardDirection * magnitudeDiff / swimmingParams.swimSpeed;
			}
			// Do move
			Vector3 averageCurrentPos = (left.transform.localPosition + right.transform.localPosition) / 2;
			Vector3 averagePrevPos = (left.previousCursorPos + right.previousCursorPos) / 2;
			Vector3 posDiff = averageCurrentPos - averagePrevPos;

			GameObject head = null;
			if (HeadRotation.instance) {
				head = Camera.main.gameObject;
			}
			Vector3 sidewayDirection = head.transform.right.normalized;
			Vector3 upwardDirection = head.transform.up.normalized;
			Vector3 vectorToMove = posDiff.x * sidewayDirection + posDiff.y * upwardDirection;

			if (cameraControl) {
				vectorToMove *= -1;
			}

			newEverythingPosition = newEverythingPosition + vectorToMove / swimmingParams.swimSpeed;

			Vector3 temp = newEverythingPosition;

			if(clampX)
				temp.x = Mathf.Clamp (temp.x, minX, maxX);
			if(clampY)
				temp.y = Mathf.Clamp (temp.y, minY, maxY);
			if(clampZ)
				temp.z = Mathf.Clamp(temp.z, minZ, maxZ);


			everythingContainer.transform.position = temp;
		}
		//everythingContainer.transform.position = newEverythingPosition;
		// Returns vector moved
		return everythingContainer.transform.position - originalEverythingPosition;
	}

	class SwimmingParams {
		public float swimSpeed = 60;
		public float regularSpeed = 20;
		public float fastSpeed = 2;
		public bool dualCursorInit = true;
		public bool fastSwim = false;
		public bool postRotate = false;
		public bool postSwim = false;

		// For inertia
		public bool postInertia = false;
		public Vector3 vectorMoved;
		public Vector3 deceleration;
		public float decelerationFactor = 100;
		public int deceleratedAmount = 20;

		public bool dualCursorOverride = false;
		public GameObject dualCursorObject;
	}
}
