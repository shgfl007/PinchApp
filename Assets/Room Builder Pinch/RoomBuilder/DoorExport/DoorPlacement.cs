using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DoorPlacement : VRObject
{
    GameObject[] walls;
    List<float> distance = new List<float>();
    List<Vector3> wallPos = new List<Vector3>();
    List<Vector3> wallRot = new List<Vector3>();

    public GameObject planBuilder;
    public GameObject meshCreator;
    public GameObject doorButton;
    
    public LayerMask myLayerMask;

    Vector3 center = new Vector3();
    Vector3 nearestWallPos = new Vector3();
    Vector3 nearestWallRot = new Vector3();
    Vector3[] directions;
    Vector3 doorDirection;
    Vector3 mousePos;
    Vector3 doorPos;
    Vector3 temp;

  //  public GameObject doorPrefab;

    RaycastHit mouseHit;

    Cursor c, currentCursor;

    bool initClick = true;
    bool m_Update = false;
    bool doorDropped = false;

    float minDistance;
    float DoorPosY = 50.0f;

    int num = 0;

    Vector3 initPos;
    Quaternion initRot;

    MeshRenderer mRenderer;


    // Use this for initialization
    void Start()
    {


       // planBuilder.SetActive(false);
        mRenderer = GetComponent<MeshRenderer>();
        //mRenderer.enabled = false;

        initPos = transform.position;
        initRot = transform.rotation;

        directions = new Vector3[4];
        directions[0] = Vector3.forward;
        directions[1] = Vector3.right;
        directions[2] = Vector3.back;
        directions[3] = Vector3.left;


    }

    public override void start(ref Cursor cursorInput)
    {
        transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

        num = 0;
        distance.Clear();
        wallPos.Clear();
        wallRot.Clear();

        doorDropped = false;
        m_Update = true;

        if (mRenderer.enabled == false)
            mRenderer.enabled = true;

        if (initClick)
        {
            currentCursor = cursorInput;
            initClick = false;
        }
        c = cursorInput;
    }

    public override void end()
    {
        doorDropped = true;
        m_Update = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Update)
        {
            Vector3 temp = c.transform.position;
            Vector3 worldToScreen = Camera.main.WorldToScreenPoint(temp);
            Vector3 cubeScreen = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 cursorToCube = new Vector3(worldToScreen.x, worldToScreen.y, cubeScreen.z);
            Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(cursorToCube);
            screenToWorld.y = DoorPosY;
            transform.position = screenToWorld;

        }
        else if (doorDropped)
        {

            if (num == 0)
            {
               // GameObject newDoor = (GameObject)Instantiate(doorPrefab, initPos, initRot);

                Vector3 temp = c.transform.position;
                Vector3 worldToScreen = Camera.main.WorldToScreenPoint(temp);
                Vector3 cubeScreen = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 cursorToCube = new Vector3(worldToScreen.x, worldToScreen.y, cubeScreen.z);
                Ray screenToWorld = Camera.main.ScreenPointToRay(cursorToCube);

                if (Physics.Raycast(screenToWorld, out mouseHit, Mathf.Infinity, myLayerMask))
                {
                    
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, DoorPosY, transform.position.z);


                    walls = GameObject.FindGameObjectsWithTag("Wall");
                    foreach (GameObject wall in walls)
                    {
                        wall.GetComponent<BoxCollider>().enabled = true;
                    }

                    RaycastHit[] hit = new RaycastHit[directions.Length];

                    for (int i = 0; i < directions.Length; i++)
                    {
                        if (Physics.Raycast(transform.position, directions[i], out hit[i]))
                        {
                            if (hit[i].collider.gameObject.tag == "Wall")
                            {
                                distance.Add(Vector3.Distance(transform.position, hit[i].collider.gameObject.transform.position));                              
                                wallPos.Add(hit[i].collider.gameObject.transform.position);
                                wallRot.Add(hit[i].collider.gameObject.transform.parent.localEulerAngles);
                            }
                        }
                    }
                    
                    minDistance = distance.Min();

                    for (int i = 0; i < distance.Count; i++)
                    {
                        if (distance[i] == minDistance)
                        {
                            nearestWallPos = wallPos[i];
                            nearestWallRot = wallRot[i];
                            doorDirection = directions[i];
                        }
                    }
                    
                    //center = sum of positions / number of positions
                         foreach (Vector3 pos in wallPos)
                            center += pos;

                    center /= wallPos.Count;

                 //   Debug.Log("CLOSEST WALL DIRECTION: " + doorDirection);

                    foreach (GameObject wall in walls)
                    {
                        wall.GetComponent<BoxCollider>().enabled = false;
                    }

                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, nearestWallRot.y, transform.localEulerAngles.z);


                    if (doorDirection == Vector3.left || doorDirection == Vector3.right)
                    {
                        transform.position = new Vector3(nearestWallPos.x, transform.position.y, transform.position.z);
                        Vector3 rotOffset;
                        Vector3 posOffset;
                        Vector3 modelPosOffset;
                        if (center.x > transform.position.x)
                        {
                            rotOffset = new Vector3(0.0f, -90.0f, 0.0f);
                            posOffset = new Vector3(0.5f, 0.0f, 0.0f);
                            modelPosOffset = new Vector3(0.0f, -48.0f, 65.0f);
                        }
                        else
                        {
                            rotOffset = new Vector3(0.0f, -90.0f, 0.0f);
                            posOffset = new Vector3(-0.5f, 0.0f, 0.0f);
                            modelPosOffset = new Vector3(0.0f, -48.0f, 97.5f);
                        }

                       // Debug.Log("THIS SHOULD REALLY WORK");
                        transform.localEulerAngles += rotOffset;
                        transform.position += posOffset;
                        transform.GetChild(0).localPosition = modelPosOffset;


                        //   transform.LookAt(new Vector3(center.x, transform.position.y, transform.position.z));
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, nearestWallPos.z);

                        Vector3 rotOffset;
                        Vector3 posOffset;
                        Vector3 modelPosOffset;

                        if (center.z > transform.position.z)
                        {
                            rotOffset = new Vector3(0.0f, -90.0f, 0.0f);
                            posOffset = new Vector3(0.0f, 0.0f, 0.5f);
                            modelPosOffset = new Vector3(0.0f, -48.0f, 32.5f);
                        }
                        else
                        {
                            rotOffset = new Vector3(0.0f, -90.0f, 0.0f);
                            posOffset = new Vector3(0.0f, 0.0f, -0.5f);
                            modelPosOffset = new Vector3(0.0f, -48.0f, 65.0f);
                        }

                        transform.localEulerAngles += rotOffset;
                        transform.position += posOffset;

                        transform.GetChild(0).localPosition = modelPosOffset;

                        //      transform.LookAt(new Vector3(transform.position.x, transform.position.y, center.z));
                    }

                   // transform.position 

                }
                num = 1;
            }
        }

      //  if (Input.GetKeyDown(KeyCode.V))
      //  {
      //      isPerspective = !isPerspective;
     //   }

        if (SpaceManager.instance.isSpaceBuilder)
        {
            if (mRenderer.enabled)
            {
                GetComponent<BoxCollider>().enabled = false;
                doorButton.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                mRenderer.enabled = false;
            }
        }
        else
        {
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                GetComponent<BoxCollider>().enabled = true;
                doorButton.SetActive(true);
                mRenderer.enabled = true;
                transform.GetChild(0).gameObject.SetActive(false);

            }
        }
        
    }
}