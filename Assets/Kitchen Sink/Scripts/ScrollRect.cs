using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class ScrollRect : VRObject {

 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     public override void AfterUpdate() {
        verticalScroll(transform.gameObject, 2f, false); //scroll vertically

    }



}
