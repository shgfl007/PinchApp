using UnityEngine;
using System.Collections;

public class ChangeParents : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (SpaceManager.instance.isSpaceBuilder) {
		
			transform.parent = null;
		}

	}
}
