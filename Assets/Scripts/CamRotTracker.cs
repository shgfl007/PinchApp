using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.VR;
public class CamRotTracker : VRObject
{
    public bool camRotTracking = true;

    // Tweakables

#if UNITY_EDITOR
    float thresh = 60;
    float maxVelocity = 10000;
    float timeLimit = 0.4f;
#else
    //float thresh = 1;
    //float timeLimit = 0.8f;
    //float maxVelocity = 10000;
    float thresh = 60;
    float maxVelocity = 10000;
    float timeLimit = 0.4f;
#endif
    // Flag variables
    float currentRot = 0, prevRot = 0;
    float timeSinceLastSwitch = 0;
    float cooldown = 0;
    string currentDirection = "right";
    int numChanges = 0;
    public bool inCamPreview = false; // 

    public Transform everythingContainer;
    //Transform stationaryObjects;
    public Transform cameraPreview = null;
    //public Text text;
    // Use this for initialization
    void Start()
    {
    //    everythingContainer = GameObject.Find("EverythingContainer").transform;
        if (cameraPreview == null)
        {
            cameraPreview = GameObject.Find("CameraPreview").transform;
        }
        //stationaryObjects = GameObject.Find ("StationaryObjects").transform;

       // cameraPreview.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
           // StopAllCoroutines();
          //  StartCoroutine(toggleCamPreview());
        }

        if (camRotTracking)
        {
            prevRot = currentRot;
#if UNITY_EDITOR
            currentRot = transform.rotation.eulerAngles.y;
#else
            //currentRot = InputTracking.GetLocalRotation(VRNode.CenterEye).y;
            currentRot = transform.rotation.eulerAngles.y;
#endif
            // Get dy
            float dy = currentRot - prevRot;
            if (Mathf.Abs(dy) > 360)
            {
                dy = dy / Mathf.Abs(dy) * (Mathf.Abs(dy) - 360);
            }

            // Check velocity
            float velocity = dy / Time.deltaTime;
            float absVel = Mathf.Abs(velocity);
            //text.text = absVel.ToString();
            //Debug.Log("absVel: " + absVel);
            if (absVel > thresh && absVel < maxVelocity)
            {
                // Switch direction accordingly
                if (velocity > 0 && currentDirection != "right")
                    switchDirection("right");
                else if (velocity < 0 && currentDirection != "left")
                    switchDirection("left");
            }

            // If the counter is larger than 0, keep counting.
            if (timeSinceLastSwitch > 0)
            {
                timeSinceLastSwitch += Time.deltaTime;

                // Stop counting if the time limit is reached and reset the counters
                if (timeSinceLastSwitch > timeLimit)
                {
                    // Reset if time limit is passed
                    numChanges = 0;
                    timeSinceLastSwitch = 0;
                }
            }

            // Cooldown code
            if (cooldown > 0)
            {
                cooldown += Time.deltaTime;
                if (cooldown > 0.5f)
                    cooldown = 0;
            }
        }
        else if (numChanges != 0 || timeSinceLastSwitch != 0)
        {
            // Reset if the cam rot tracker is turned off
            numChanges = 0;
            timeSinceLastSwitch = 0;
        }
    }

    void switchDirection(string direction)
    {
        currentDirection = direction;
        // Start counting if not already (counter must be greater than 0 to start counting)
        if (timeSinceLastSwitch == 0)
        {
            timeSinceLastSwitch += 0.0001f;
        }
        // If counting, increment the number of direction changes
        else if (timeSinceLastSwitch > 0)
        {
            numChanges++;

            // If the number of changes is reached, reset the counters and start the coroutine task
            if (numChanges >= 3)
            {
                // Reset
                numChanges = 0;
                timeSinceLastSwitch = 0;
                if (cooldown == 0)
                {
                    StopAllCoroutines();
               //     StartCoroutine(toggleCamPreview());
                }
            }
        }
    }

    public IEnumerator calibRoutine()
    {
        // Wait for user to be ready
        for (int i = 0; i < 60 * 2; i++)
        {
            yield return new WaitForSeconds(0.016f);
        }
        // Give 5 seconds or less to register clicks
        while (!(TrackerScript.instance.rightCursor.clicked || TrackerScript.instance.leftCursor.clicked))
        {
            if (TrackerScript.instance.rightCursor.clicked || TrackerScript.instance.leftCursor.clicked)
            {
                break;
            }
            yield return new WaitForSeconds(0.016f);
        }
       // StartCoroutine(toggleCamPreview());
    }

    public IEnumerator toggleCamPreview()
    {
        cooldown = 0.0001f;
        if (inCamPreview)
        {
            TrackerScript.instance.outputCam = false;
            cameraPreview.gameObject.SetActive(false);
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            StartCoroutine(fadeIn(everythingContainer, 20, true));
            //StartCoroutine(fadeIn(stationaryObjects, 20, true));
            inCamPreview = false;
        }
        else
        {
            TrackerScript.instance.outputCam = true;
            Camera.main.clearFlags = CameraClearFlags.Skybox;
            cameraPreview.gameObject.SetActive(true);
            StartCoroutine(fadeOut(everythingContainer, 20, true));
            //StartCoroutine(fadeOut(stationaryObjects, 20, true));
            inCamPreview = true;
           // StartCoroutine(calibRoutine());
        }
        yield return new WaitForSeconds(0.016f);
    }
}
