using UnityEngine;
using System.Collections;

public class CarouselManager : MonoBehaviour {

    public GameObject[] icons;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].transform.RotateAround(transform.position, Vector3.up, 0.5f);
            icons[i].transform.LookAt(transform.position);
        }
	
	}
}
