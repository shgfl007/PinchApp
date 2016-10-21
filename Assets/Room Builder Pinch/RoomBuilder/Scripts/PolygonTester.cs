using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PolygonTester : MonoBehaviour {



	public GameObject getPoints;
	private Vector3[] points;
	private Vector2[] vertices2D ;
		void Start () {
				// Create Vector2 vertices

		}


	void Update() {

		if (Input.GetMouseButtonUp (1)) {



		}

	}


	public void startMesh() {

		//Debug.Log ("Point Length is " + getPoints.gameObject.GetComponent<StoreMouseClicks> ().mouseClicks.Count);
		List<Vector3> temp = getPoints.gameObject.GetComponent<StoreMouseClicks> ().mouseClicks;

		//temp.RemoveAt (temp.Count-1);
		points = temp.ToArray ();
		vertices2D = new Vector2[points.Length];

		for (int i=0; i<vertices2D.Length;i++){
			vertices2D[i].x =points [i].x;
			vertices2D[i].y =points [i].z;

		}


		//System.Array.Reverse (points);
		createMesh ();
	}

	void createMesh() {


		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices2D.Length];
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		Vector3[] normals = new Vector3[vertices2D.Length];

		for (int i = 0; i < normals.Length; i++) {
		
			normals [i] = Vector3.up;
		}

		// Set up game object with mesh;
		//gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.GetComponent<MeshFilter>();
		filter.mesh.Clear ();
		msh.normals = normals;
		filter.mesh = msh;


	}
}