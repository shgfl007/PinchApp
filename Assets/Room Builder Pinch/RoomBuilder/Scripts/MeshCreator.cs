using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class MeshCreator : MonoBehaviour {
	public GameObject getPoints;
	private Vector3[] points;

	MeshFilter mf;
	Mesh mesh;
	int triCounter;
	int iCounter;
	int numOfTris;
	// Use this for initialization
	void Start () {
	
	//	createMesh ();

	//	mesh.uv = uv;
	//	gameObject.transform.localEulerAngles = new Vector3(90,0,0);
	}

	void createMesh(){
		mf = GetComponent<MeshFilter> ();
		mesh = new Mesh ();
		mf.mesh = mesh;
		mf.mesh.Clear ();
		mesh.Clear ();

//		Vector3[] vertices = new Vector3[4] {
//			new Vector3 (0, 0, 0), new Vector3 (0, 0, 1), new Vector3 (1, 0,1), new Vector3 (1, 0, 0)
//		};
//
		Vector3[] vertices = points;

		numOfTris = (points.Length-2) ;
		int[] tri = new int[numOfTris * 3 ];

		int p1, p2, p3;
		p1 = 0;
		p2 = 1;
		p3 = 2;
		//Debug.Log ("num of Tri vertices is " + numOfTris);
		triCounter = 0;
		iCounter = 0;
		for (int i = 0; i < tri.Length; i++) {


//			if (iCounter == 3) {
//				triCounter++;
//				Debug.Log ("it gets here");
//			} else {
				iCounter++;

		//	}
			//Debug.Log ("the I is " + iCounter);

			//Debug.Log ("the tricount is  " + triCounter);

			if (triCounter != (numOfTris - 1)) {
				if (iCounter == 1) {

					if (triCounter == 0) {
						tri [i] = p1;
						p1 += 2;
					} else {
						tri [i] = 2;
					}
//					tri [i] = p1;
//					p1 += 2;
					//Debug.Log("Current indeces value is " + tri [i]);

				}

				if (iCounter == 2) {

					if (triCounter == 0) {
						tri [i] = p2;
						p2 += 2;
					} else {
						tri [i] = p2;
						p2 += 1;
					}
//					tri [i] = p2;
//					p2 += 2;
					//Debug.Log("Current indeces value is " + tri [i]);


				}
				if (iCounter == 3) {

					if (triCounter == 0) {
						tri [i] = p3;
						p3 += 2;
					} else {
						tri [i] = p2;
						p3 += 1;
					}

//					tri [i] = p3;
//					p3 += 2;
					triCounter++;

					iCounter = 0;
					//Debug.Log("Current indeces value is " + tri [i]);


				}
			} else {
			
				if (iCounter == 1) {
					tri [i] = 2;
					//Debug.Log("Current indeces value is " + tri [i]);

					//p1 += 2;
				}

				if (iCounter == 2) {
					tri [i] = points.Length-1;
				//	Debug.Log("Current indeces value is " + tri [i]);

					//p2 += 2;

				}
				if (iCounter == 3) {
					tri [i] = 0;
					//Debug.Log("Current indeces value is " + tri [i]);

					//p3 += 2;
					iCounter = 0;

				}
			}

			//
//			if (i == tri.Length - 4) {
//				tri [i] = 2;
//				tri [i + 1] = tri [tri.Length - 1];
//				tri [i + 2] = 0;
//			} else {
//				tri [i] = p1;
//				tri [i + 1] = p2;
//				tri [i + 2] = p3;
//				p1 = p3;
//				p2 = p3 + 1;
//				p3 = p3 + 2;
//			}
		}

//		tri [0] = 0;
//		tri [1] = 1;
//		tri [2] = 2;
//		tri [3] = 2;
//		tri [4] = 3;
//		tri [5] = 0;
//
//

		Vector3[] normals = new Vector3[points.Length];

		for (int i = 0; i < points.Length - 1; i++) {
		
			normals [i] = Vector3.up;
		
		}

		//		Vector2[] uv = new Vector2[4];
		//		uv [0] = new Vector2 (0,0);
		//		uv [1] = new Vector2 (1,0);
		//		uv [2] = new Vector2 (0,1);
		//		uv [3] = new Vector2 (1,1);
		//
		mesh.vertices = vertices;

		mesh.triangles = tri;
		mesh.normals = normals;
	//	mesh.RecalculateNormals();

	//	mf.mesh = mesh;
	
	}

	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonUp (1)) {

			//Debug.Log ("Point Length is " + getPoints.gameObject.GetComponent<StoreMouseClicks> ().mouseClicks.Count);
			List<Vector3> temp = getPoints.gameObject.GetComponent<StoreMouseClicks> ().mouseClicks;

			//temp.RemoveAt (temp.Count-1);
			points = temp.ToArray ();
			//System.Array.Reverse (points);
			createMesh ();

		}

	}
}
