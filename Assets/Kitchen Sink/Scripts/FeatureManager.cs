//Kitchen Sink
using UnityEngine;
using System.Collections;

public class FeatureManager : MonoBehaviour {
    //This script basically helps with disabling and enabling active features

    private static FeatureManager mInstance;
    public static FeatureManager instance { get { return mInstance; } }
    public GameObject activeFeature;
    public GameObject[] resetAll;

    // Use this for initialization
    void Start () {
        if (mInstance == null)
        {
            mInstance = this;
        }
        resetAll = GameObject.FindGameObjectsWithTag("KSIcon");
    }

    // Update is called once per frame
    void Update () {

    }

    public void setActiveFeature(GameObject currentFeature)
    {
        activeFeature = currentFeature;
    }


    //Start Features
    public void StartFeature() {
        activeFeature.gameObject.GetComponent<HoverIcons>().ActivateFeature();
    }

    public void IntroComplete() {

        foreach (GameObject g in resetAll)
        {
            g.gameObject.GetComponent<HoverIcons>().introComplete = true;
        }
    }

    //Reset Active Features
    public void ResetFeature() {
        //just resets all features incase some were left open
        foreach (GameObject g in resetAll) {
            g.gameObject.GetComponent<HoverIcons>().Reset();
        }

    }


}
