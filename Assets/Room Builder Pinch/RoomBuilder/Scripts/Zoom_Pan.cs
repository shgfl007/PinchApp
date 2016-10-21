using UnityEngine;
using System.Collections;

public class Zoom_Pan : MonoBehaviour {
	float minFov = 3f;
	float maxFov = 10f;
	float sensitivity = 2f;

	public float mouseSensitivity  = 50f;
	private Vector3 lastPosition ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

			float fov = Camera.main.orthographicSize;
			fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity * -1;
			fov = Mathf.Clamp(fov, minFov, maxFov);
		Camera.main.orthographicSize = fov;




	
			if (Input.GetMouseButtonDown(2))
			{
				lastPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton(2))
			{
				Vector3 delta  = Input.mousePosition - lastPosition;
			transform.Translate(delta.x * mouseSensitivity,delta.y * mouseSensitivity, 0 );
				lastPosition = Input.mousePosition;
			}



	}
}
