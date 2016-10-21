using UnityEngine;
using System.Collections;

public class FilterDataNation : MonoBehaviour {


	public int nation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void categorizeNations () {

		PointManager.instance.Categorize (nation);
	}

	public void drawLines() {

		PointManager.instance.DrawRelationLine (nation);

	}

}
