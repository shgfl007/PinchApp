using UnityEngine;
using System.Collections;

public class ToggleCamButton : VRObject {

    Cursor c;



    // Use this for initialization
    void Start () {
	
	}

    public override void start(ref Cursor cursorInput)
    {
        c = cursorInput;


    }

    public override void end()
    {
        SpaceManager.instance.toggleTopDown = true;
        SpaceManager.instance.toggleCamView();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
