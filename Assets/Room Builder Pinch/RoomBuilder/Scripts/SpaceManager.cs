using UnityEngine;
using System.Collections;

public class SpaceManager : MonoBehaviour
{

    public GameObject macyScene; //Macy Space
    public GameObject planDraw; //Plan Drawing Tool
    public GameObject planUI; //Plan UI
    public GameObject introScene; // First Intro Scene
    public GameObject introScene2;// Second Intro Scene
    public GameObject floorCollider;
    //public GameObject cObject;
    public ManualHeadRot cHead;

    public GameObject ec;// Everything Container
    public GameObject cMain; //Cardboard Main

    public Transform toMacy;
    public Transform toEC;

    Vector3 camInitPos, ecInitPos;
    public Transform defaultCam, defaultEC;

    public int sceneState = 1;

    private static SpaceManager mInstance;
    public static SpaceManager instance { get { return mInstance; } }

    public bool isSpaceBuilder = false;
    public bool toggleTopDown = false;

    bool camSet = false;

    public GameObject dummyToExpand;
    //public GameObject cMain;

    public bool checkZoom;

	public GameObject carpet;


    // Use this for initialization
    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }

        GameObject blackPanel = GameObject.Find("BlackImage");
        GameObject mainCamera = GameObject.Find("Main Camera");

        blackPanel.transform.parent = mainCamera.transform;
        blackPanel.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
        blackPanel.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
		sceneState = 1; // to remove splash screen
        cHead.enabled = false;
        Cardboard.SDK.Recenter();
        changeSceneState();

    }



    public void changeSceneState()
    {

        switch (sceneState)
        {

            case 0:  //Start Drawing Intro 1
                break;
                camInitPos = cMain.transform.position;
                ecInitPos = ec.transform.position;

                //camInitRot.transform.rotation = cMain.transform.rotation;
                //ecInitRot.transform.rotation = ec.transform.rotation;

                macyScene.SetActive(false);
                planDraw.SetActive(false);
                planUI.SetActive(false); //Plan UI
                introScene2.SetActive(false);// Second Intro Scene
                introScene.SetActive(true);
			carpet.gameObject.SetActive (false);

            case 1: //Start Drawing Intro 2
				camInitPos = cMain.transform.position;
				ecInitPos = ec.transform.position;

                macyScene.SetActive(false);
                planDraw.SetActive(false);
                planUI.SetActive(true); //Plan UI
                introScene2.SetActive(true);// Second Intro Scene
                introScene.SetActive(false);

                introScene2.GetComponent<FadeInCanvases>().fadeInCanvas = true;
                planUI.GetComponent<FadeInCanvases>().fadeInCanvas = true;
			carpet.gameObject.SetActive (false);

                break;

            case 2: //To Draw
                macyScene.SetActive(false);
                planDraw.SetActive(true);
                planUI.SetActive(true); //Plan UI
                introScene2.SetActive(false);// Second Intro Scene
                introScene.SetActive(false);

                dummyToExpand.transform.localScale = new Vector3(1, 1, 1);
                checkZoom = true;

                GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

                foreach (GameObject wall in walls)
                {
                    wall.gameObject.transform.GetComponent<MeshRenderer>().enabled = false;
                    wall.transform.parent.GetChild(2).gameObject.SetActive(true);
                    //wall.GetComponent<BoxCollider>().enabled = false;
                }
                //floorCollider.SetActive(false);
                ManualHeadRot.instance.reCenterCardboard();
                //Cardboard.SDK.Recenter ();
			carpet.gameObject.SetActive (false);

                cHead.enabled = false;
                //Debug.Log ();
                PlanBuilder();

                break;

		case 3: //Macy's Scene

			macyScene.SetActive (true);
			planDraw.SetActive (false);
			planUI.SetActive (false); //Plan UI
			introScene2.SetActive (false);// Second Intro Scene
			introScene.SetActive (false);
			carpet.gameObject.SetActive (true);
                dummyToExpand.transform.localScale = new Vector3(30, 30, 30);


                GameObject[] walls2 = GameObject.FindGameObjectsWithTag("Wall");
                foreach (GameObject wall in walls2)
                {
                    wall.gameObject.transform.GetComponent<MeshRenderer>().enabled = true;
                    wall.transform.parent.GetChild(2).gameObject.SetActive(false);
                    wall.GetComponent<BoxCollider>().enabled = true;
                }
                //floorCollider.SetActive(true);
                cHead.enabled = true;
                ManualHeadRot.instance.reCenterCardboard();

                SpaceBuilder();

                break;


            default:

                break;

        }

    }

    void SpaceBuilder()
    {

        //Cardboard.SDK.Recenter ();

        ec.transform.position = toEC.transform.position;
        ec.transform.localRotation = toEC.transform.localRotation;

        //cMain.transform.position = toMacy.transform.position;

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        Vector3 center = new Vector3();
        foreach (GameObject wall in walls)
        {
            //wall.GetComponent<BoxCollider>().enabled = true;
            center += wall.transform.position;
        }

        center /= walls.Length;

        cMain.transform.position = center;
        cMain.transform.localRotation = toMacy.transform.localRotation;


    }

    void PlanBuilder()
    {
        //Cardboard.SDK.Recenter ();

        cMain.transform.position = defaultCam.transform.position;
        cMain.transform.localRotation = defaultCam.transform.localRotation;

        //Debug.Log(cMain.transform.position);

        ec.transform.position = defaultEC.transform.position;
        ec.transform.localRotation = defaultEC.transform.rotation;


    }

    // Update is called once per frame
    void Update()
    {
        if (checkZoom && !toggleTopDown)
        {
            if (cMain.gameObject.transform.position.y < 52)
            {

                Invoke("toggleCamView", 0.7f);
                checkZoom = false;
            }
        }
        

    }

    public void toggleCamView()
    {
    //    Debug.Log("CHANGE THE VIEW");

        if (!isSpaceBuilder)
        {

            StartCoroutine(ChangeView(3, 0.7f));
            FadeBlack.instance.fadeInBetween = true;
         //CB   RayCast.instance.planDrawing = false;
      //CB      RayCast.instance.everythingContainer = RayCast.instance.newEC;
      //CB      RayCast.instance.swimmingParams.swimSpeed = 6;
            /*
            sceneState = 3;
            changeSceneState();
            isSpaceBuilder = true;
            */
            //   print("State 3 switch");

        }
        else
        {
            StartCoroutine(ChangeView(2, 0.75f));
            FadeBlack.instance.fadeInBetween = true;
          //CB  RayCast.instance.planDrawing = true;
            //CB RayCast.instance.everythingContainer = RayCast.instance.oldEC;
        //CB    RayCast.instance.swimmingParams.swimSpeed = 60;

        }

    }

    IEnumerator ChangeView(int caseNumber, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        sceneState = caseNumber;
        changeSceneState();
        isSpaceBuilder = !isSpaceBuilder;
        //	print("State 2 switch");
    }

}