//Kitchen Sink
using UnityEngine;
using System.Collections;

public class DropTrigger : MonoBehaviour {
    //This script handles which drop target to assign on release

    private GameObject otherObject ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {

		//if its in the drop trigger
        if (col.gameObject.name == "DropObject") {
            otherObject = col.gameObject.GetComponent<DragDrop>().currentAssign;
            col.gameObject.GetComponent<DragDrop>().currentAssign = this.gameObject;
        }
    }

	//exited the trigger
    void OnTriggerExit(Collider col) {
        if (col.gameObject.name == "DropObject") {
            col.gameObject.GetComponent<DragDrop>().currentAssign = otherObject;

        }
    }

}
