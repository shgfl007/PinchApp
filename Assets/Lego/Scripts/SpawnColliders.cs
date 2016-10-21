using UnityEngine;
using System.Collections;

public class SpawnColliders : MonoBehaviour {


	public GameObject baseC;
	public GameObject colliderParent;
	public int gridSize = 7;

	public float transformIncrement = 10;
	// Use this for initialization
	void Start () {
		StartCoroutine (createBaseColliders());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator createBaseColliders() {

		for (int x = 0; x < gridSize; x++) {
		
			for (int z = 0; z < gridSize; z++) {

				GameObject block = Instantiate (baseC, Vector3.zero, Quaternion.identity) as GameObject;
				block.transform.parent = colliderParent.transform;
				Vector3 tempPos = block.transform.localPosition;
				tempPos = Vector3.zero;
		//		print (tempPos);

				tempPos.x += transformIncrement * x;
				tempPos.z += transformIncrement * z;

				block.transform.localPosition = tempPos;
			}

		}

		yield return null;

	}
}
