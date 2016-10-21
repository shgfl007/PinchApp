using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThereAndBack {

	public float duration = 0.8f;
	Tweener tweener = new Tweener();
	public List<GameObjectInfo> gameObjectInfos = new List<GameObjectInfo> ();
	public bool isThere = false;

	public void go() {
		if (!isThere)
			there ();
		else
			back ();
	}

	public void go(float durationAlteration) {
		duration = durationAlteration;
		if (!isThere)
			there ();
		else
			back ();
	}

	void there() {
		for (int i = 0; i < gameObjectInfos.Count; i++) {
			if (gameObjectInfos[i].finalParameters.rotation != new Vector3(-1000, -1000, -1000))
				tweener.tween (gameObjectInfos[i].gameObject, "rotation", gameObjectInfos[i].finalParameters.rotation, duration, 0f, Ease.EaseInOutQuad, true);
			if (gameObjectInfos[i].finalParameters.position != new Vector3(-1000, -1000, -1000))
				tweener.tween (gameObjectInfos[i].gameObject, "position", gameObjectInfos[i].finalParameters.position, duration, 0f, Ease.EaseInOutQuad, true);
			if (gameObjectInfos[i].finalParameters.alpha != -1000)
				tweener.tween (gameObjectInfos[i].gameObject, "alpha", gameObjectInfos[i].finalParameters.alpha, duration, 0f, Ease.Linear, true);
		}
		isThere = true;
	}

	void back() {
		for (int i = 0; i < gameObjectInfos.Count; i++) {
			//Debug.Log (gameObjectInfos[i].gameObject.name + " to rotationX " + gameObjectInfos[i].originalParameters.rotation.x);
			tweener.tween (gameObjectInfos[i].gameObject, "rotation", gameObjectInfos[i].originalParameters.rotation, duration, 0f, Ease.EaseInOutQuad, true);
			tweener.tween (gameObjectInfos[i].gameObject, "position", gameObjectInfos[i].originalParameters.position, duration, 0f, Ease.EaseInOutQuad, true);
			if (gameObjectInfos[i].originalParameters.alpha != -1000)
				tweener.tween (gameObjectInfos [i].gameObject, "alpha", gameObjectInfos [i].originalParameters.alpha, duration, 0f, Ease.Linear, true);
		}
		isThere = false;
	}

	public void add(string name, ObjectParameters parameters) {
		GameObject theObject = GameObject.Find (name);
		GameObjectInfo theInfo = new GameObjectInfo (theObject, parameters);
		gameObjectInfos.Insert (0, theInfo);
	}

	public void add(GameObject theObject, ObjectParameters parameters) {
		GameObjectInfo theInfo = new GameObjectInfo (theObject, parameters);
		gameObjectInfos.Insert (0, theInfo);
	}
}

// Pass in game object and final parameters
public class GameObjectInfo {
	public GameObject gameObject;

	public ObjectParameters originalParameters = new ObjectParameters();
	public ObjectParameters finalParameters = new ObjectParameters();

	public GameObjectInfo(GameObject gameObjectInput, ObjectParameters parameters) {
		gameObject = gameObjectInput;
		originalParameters.position = new Vector3(gameObject.transform.position.x,
		                                          gameObject.transform.position.y,
		                                          gameObject.transform.position.z);
		originalParameters.alpha = getAlphaOfGameObject();
		originalParameters.rotation = new Vector3 (gameObject.transform.rotation.eulerAngles.x,
		                                           gameObject.transform.rotation.eulerAngles.y,
		                                           gameObject.transform.rotation.eulerAngles.z);
		finalParameters = parameters;
		//Debug.Log (gameObject.name + ": " + originalParameters.position.x + ", " + originalParameters.alpha + ", " + originalParameters.rotation.x);
	}

	public float getAlphaOfGameObject() {
		float tempAlpha = 0;
		if(gameObject.GetComponent<GUITexture>() != null)
			tempAlpha = gameObject.GetComponent<GUITexture>().color.a;
		else if (gameObject.GetComponent<Renderer>() != null)
			tempAlpha = gameObject.GetComponent<Renderer>().material.color.a;
		else if (gameObject.GetComponent<CanvasGroup>() != null)
			tempAlpha = gameObject.GetComponent<CanvasGroup>().alpha;
		return tempAlpha;
	}
}

public class ObjectParameters {
	public Vector3 position = new Vector3(-1000, -1000, -1000);
	public float alpha = -1000;
	public Vector3 rotation = new Vector3(-1000, -1000, -1000);
}