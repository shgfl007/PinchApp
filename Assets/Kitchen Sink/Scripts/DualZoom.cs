//Kitchen Sink
using UnityEngine;
using System.Collections;

public class DualZoom : VRObject {

    bool bothClicked = false;
    Vector3 originalScale;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!LeftCursor.instance.clicked || !RightCursor.instance.clicked){
            bothClicked = false; //if one cursor released stop scaling
           originalScale = transform.localScale;
        }

        if (bothClicked) //do this when both cursor clicked
        {
            Vector3 left = Camera.main.WorldToScreenPoint(LeftCursor.instance.transform.position);
            Vector3 right = Camera.main.WorldToScreenPoint(RightCursor.instance.transform.position);

            float scale = Mathf.Abs(left.x - right.x) / 300; 
            Vector3 newScale = originalScale * scale; //get new scale value

            //clamp scales
            newScale.x = Mathf.Clamp(newScale.x, 80, 130);
            newScale.y = Mathf.Clamp(newScale.x, 80, 130);
            newScale.z = Mathf.Clamp(newScale.x, 80, 130);
            transform.localScale = newScale; //assign new scale
 
        }


    }

    public override void dualCursorUpdate(ref Cursor left, ref Cursor right)
    {
        bothClicked = true; //when both cursors get clicked set this to true
        base.dualCursorUpdate(ref left, ref right);
    }




}
