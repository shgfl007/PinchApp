using UnityEngine;
using System.Collections;

public class toggleview : MonoBehaviour {

    public Transform wall;
    public Transform center;

    Vector3 perspectivePos;// = new Vector3();
    Vector3 orthoPos = new Vector3();
    bool isPerspective = false;
    float camSpeed = 100.0f;


    // Use this for initialization
    void Start () {

        orthoPos = transform.position;
        perspectivePos = orthoPos + new Vector3(0.0f, -7.0f, 0.0f);
        //Debug.Log(orthoPos + "   " + perspectivePos + "   " + transform.position);
        //perspectivePos = new Vector3(50.0f, 50.0f, 50.0f);
	}
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log("POS: " + transform.position + "   PERS: " + perspectivePos);

        if (Input.GetKeyDown(KeyCode.V))
        {
			Cardboard.SDK.Recenter ();
            isPerspective = !isPerspective;
        }

        if (isPerspective)
        {

            transform.position = Vector3.MoveTowards(transform.position, perspectivePos, 0.15f); // * Time.fixedDeltaTime);
            transform.LookAt(wall);
        }
        else
        {

			transform.LookAt(Vector3.down);

            if (transform.position.y < orthoPos.y)
            {
                transform.position = Vector3.MoveTowards(transform.position, orthoPos, 0.15f);// camSpeed * Time.fixedDeltaTime);
            }
            else
            {
                if (GetComponent<Camera>().orthographic != true)
                {
                    GetComponent<Camera>().orthographic = true;
                }
            }
        }

	}
}
