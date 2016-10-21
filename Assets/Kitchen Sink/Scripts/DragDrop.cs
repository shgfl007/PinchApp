//Kitchen Sink
using UnityEngine;
using System.Collections;

public class DragDrop : VRObject {

    public GameObject dropSquare_1, dropSquare_2, currentAssign;//needed objects
    bool mUpdate; //update square
    Cursor mCursor; //cursor


    void Start () {
        currentAssign = dropSquare_2; //currently assigned drop target
    }

    public override void start(ref Cursor cursorInput)
    {
        if (!mUpdate)
        {
            mUpdate = true;
            mCursor = cursorInput;
        }
        base.start(ref cursorInput);
    }

    //public override void
    public override void end()
    {
        mUpdate = false;
        transform.position = currentAssign.transform.position;
        base.end();
    }

    // Update is called once per frame
    void Update () {

        if (mUpdate) {

            //handle drag and drop
            Vector3 worldToScreen = Camera.main.WorldToScreenPoint(mCursor.transform.position);
            Vector3 cubeScreen = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 cursorToCube = new Vector3(worldToScreen.x, worldToScreen.y, cubeScreen.z);
            Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(cursorToCube); //fetch screen to world coordinates
            transform.position = screenToWorld;

        }
	}
}
