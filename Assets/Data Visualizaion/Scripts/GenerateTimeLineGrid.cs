using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateTimeLineGrid : MonoBehaviour {

	public GameObject prefab;
	public Vector3 position = new Vector3 (0,0,0);

	int counterY = 0;

	int counterZ = 0;
	public List<Vector3> pointList;

	// Use this for initialization
	void Start () {

	}

	public void MakeGridPoints(int gPoints) {

		int c = 0;

		for(int z = 0; c < gPoints; z++){

			for (int x = 0; x < (int)Random.Range(4,6) && c < gPoints; x++) {

				for (int y = 0; y < (int)Random.Range(3,4) && c < gPoints; y++) {

					c++;

					position.x = x * 1.5f;
					position.y = y * 1.5f;
					position.z = z * 1.5f;

					//	yield return null;
					Instantiate (prefab, position, Quaternion.identity);
					pointList.Add (position);
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
