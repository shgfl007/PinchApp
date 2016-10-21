using UnityEngine;

using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;

public class CheckAccelerometer : MonoBehaviour
{


    //public Text tX, tY, tZ;
    bool checkCoroutine, isChecking;
    float timeLeft = 5;

    // Use this for initialization
    void Start()
    {
        checkCoroutine = false;
        isChecking = false;
    }

    // Update is called once per frame
    void Update()
    {

        //transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
        //tX.text = "X is " + Input.acceleration.x.ToString();
     

        if (Input.acceleration.y > -0.35f)
        {

            if (!isChecking)
            {
                checkCoroutine = true;
                isChecking = true;
            }

        }

        if (checkCoroutine)
        {

            if (Input.acceleration.y > -0.35f)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft < 0)
                {
                    checkCoroutine = false;
                    SceneManager.LoadScene(0);
                    Invoke("resetChecking", 3);
                    //isChecking = false;


                }
            }
            else
            {

                checkCoroutine = false;
                Invoke("resetChecking", 3);
            }

        }



    }

    void resetChecking()
    {

        isChecking = false;
        timeLeft = 3;
    }

}
