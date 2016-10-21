using UnityEngine;
using System.Collections;


// Information holding struct
public class HeldObjectInfo
{
    public delegate void RayCastHitObj(GameObject obj, HeldObjectInfo heldObjectInfo);
    public RayCastHitObj rayHitObj;
    public Cursor cursor;
    public GameObject cursorObject;
    public bool click = false;
    public bool previousClickState = false;
    public bool doRayCast = true;
    public Ray ray;
    public RaycastHit hit;
    public Transform heldTransform;
    public Transform hoveringTransform;
    public Transform previousHoveringTransform;
    public Transform previousHeldTransform;
    public Transform handPuppet;
    public bool hold = false; // Switches turn on when an object is hit, and turn off when cursor is no longer clicking
    public bool buttonClicked = false;

    public float originalRotationWithRespectToZero;

    public Vector3 previousOffset;
    public Vector3 previousCursorPos;
    public Vector3 currentCursorPos;
    public float distMoved = 0;

    public bool objectHitByRay = false;
    public float speed = 0, horizontalSpeed = 0, verticalSpeed = 0;

    public int interactionMode = -1;
    public int previousInteractionMode = -1;
}

