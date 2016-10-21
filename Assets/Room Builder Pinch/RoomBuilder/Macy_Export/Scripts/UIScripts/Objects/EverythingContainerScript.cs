using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EverythingContainerScript : MonoBehaviour {

	public Tweener mTween;
	public GameObject current;
	public bool isAnimating = false;
	public bool contentOpen = false;
	public List<GameObject> managedInterfaces = new List<GameObject> ();
	List<GameObject> previousInterfaces = new List<GameObject> ();
	GameObject cameraObject;

	// Use this for initialization
	void Start () {
		mTween = gameObject.AddComponent<Tweener>();
		previousInterfaces.Insert(0, GameObject.Find("Home Interface")); // Default start location
		managedInterfaces.Insert(0, GameObject.Find("Home Interface")); // Home is a default managed interface
		current = previousInterfaces [0];
	}

    public void goToNearestManagedInterface() {
        float absDistance = -1;
        GameObject closestInterface;
        closestInterface = managedInterfaces[0];
        if (VRCamera.instance)
        { 
            absDistance = (managedInterfaces[0].transform.position - VRCamera.instance.gameObject.transform.position).magnitude;
        
		    for (int i = 0; i < managedInterfaces.Count; i++) {
			    float newDist = (managedInterfaces [i].transform.position - VRCamera.instance.gameObject.transform.position).magnitude;
			    //Debug.Log(managedInterfaces[i].name);
			    if (newDist < absDistance && managedInterfaces[i].activeSelf) {
				    absDistance = newDist;
				    closestInterface = managedInterfaces[i];
			    }
		    }
        }
        goToGameObject (closestInterface);
	}

	public void goToGameObject(GameObject to)
	{
		if (!isAnimating) {
			disableAll ();
			to.SetActive (true);

			// Add previous such that there can only be 3
			if (current != null) {
				previousInterfaces.Insert (0, current);
			}
			while (previousInterfaces.Count > 3) {
				previousInterfaces.RemoveAt(previousInterfaces.Count - 1);
			}

			// Find vector from everything container to to
			Vector3 vectorTo = new Vector3(transform.position.x - to.transform.position.x,
			                               transform.position.y - to.transform.position.y,
			                               transform.position.z - to.transform.position.z);
			moveTo (gameObject, vectorTo);

			// Set previous objects to be faded out
			for (int i = 0; i < previousInterfaces.Count; i++) {
				if (previousInterfaces[i] != null && previousInterfaces[i].name != "Home Interface") {
					mTween.tween (previousInterfaces[i], "alpha", 0.35f, 0.35f, 0, Ease.Linear, false);
				}
			}
			// Set the current alpha to be opaque
			mTween.tween (to, "alpha", 1, 0.35f, 0, Ease.Linear, false);
			//to.GetComponent<CanvasGroup> ().alpha = 1;

			current = to;
		}
	}

	public void goBack()
	{
		goToGameObject (previousInterfaces[0]);
	}

	public void moveTo(GameObject objectToMove, Vector3 position) {
		if (!isAnimating) {
			StartCoroutine (move (objectToMove, position));
		}
	}

	public void simultaneousMoveTo(GameObject objectToMove, Vector3 position) {
		StartCoroutine (move (objectToMove, position));
	}

	IEnumerator move(GameObject objectToMove, Vector3 position) {
		isAnimating = true;
		int increments = 35;
		Vector3 initialPosition = objectToMove.transform.position;
		Vector3 vectorToMoveBy = position - initialPosition;

		for (int i = 1; i <= increments; i++)
		{
			Vector3 updatedPosition = initialPosition + easeInOut (vectorToMoveBy, increments, i);
			objectToMove.transform.position = updatedPosition;
			//Debug.Log(objectToMove.transform.position.z);
			yield return new WaitForSeconds(0.008f);
		}
		isAnimating = false;
	}

	Vector3 easeInOut(Vector3 totalLength, int increments, float incrementNum) {
		return totalLength * (-0.5f * Mathf.Cos(Mathf.PI * incrementNum / (float) increments) + 0.5f);
	}

	public void addManagedInterface(GameObject managedInterface) {
		managedInterfaces.Add (managedInterface);
		managedInterface.SetActive (false);
		//Debug.Log (managedInterface.name);
		//Debug.Log (managedInterfaces.Count);
	}

	public void disableAll() {
		//Debug.Log (managedInterfaces.Count);
		for (int i = 0; i < managedInterfaces.Count; i++)
		{
			bool isPrevious = false;
			for (int j = 0; j < previousInterfaces.Count && !isPrevious; j++)
			{
				isPrevious = previousInterfaces[j] == managedInterfaces[i];

			}
			if (managedInterfaces[i].name != "Home Interface" && managedInterfaces[i] != current && !isPrevious)
			{
				managedInterfaces[i].SetActive(false);
				//Debug.Log("disabled");
			}
		}
	}
}
