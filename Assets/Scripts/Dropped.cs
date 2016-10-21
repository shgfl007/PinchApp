using UnityEngine;
using System.Collections;

public class Dropped : MonoBehaviour {




	public Material dropped, original;
	private Renderer rend;
	void OnDisable () {
		rend = GetComponent<MeshRenderer> ();

		rend.material = original;

	}
	// Use this for initialization
	void Start () {
	
		rend = GetComponent<MeshRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {

		rend.material = dropped;
	}

	void OnTriggerExit() {

		rend.material = original;

	}

}
