using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class PointData : MonoBehaviour {

	public enum Nationality
	{
		Italian,
		Mexican,
		Indian,
		Chinese,
		Japanese,
		American
	}

	public GameObject year;


	public Nationality nationality; // nationality

	float xMin, xMax, yMin, yMax, zMin, zMax, sizeMin, sizeMax;// max and min data values

//	public Text xAxis, yAxis, zAxis, sizeAxis, nAxis;
	public Image toChange;
	string[] nationString = {"Italian", "Mexican", "Indian", "Chinese", "Japanese", "American"};

	[Range(0.0f, 100.0f)]
	public float xValue; // Prices
	[Range(0.0f, 700.0f)]
	public float yValue; // Calories
	[Range(0.0f, 3.0f)]
	public float zValue; // Taste
	[Range(0.0f, 100.0f)]
	public float size;

	public bool collected = false;

	public int selectedNationality; //nationality integer
	Material mat;

	public VectorLine pathLine;


	Color lineColor = Color.white;
	public Texture lineTex ;

	public List<Vector3> overtimePoints;

	public bool canDraw = false;

	public Vector3 initPos, startPos;
	int c = 0;
	public int moveSpeed;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
		MapPoints ();
	}


	// Update is called once per frame
	void FixedUpdate () {
	
		if (canDraw) {


		//	drawPath();
		//	movePointOverTime ();
			moveToNextPoint();

		} else {
		//	transform.position = initPos;
			c = 0;
		}

	}

	void drawPath() {
		pathLine.points3.Add (this.transform.position);
		pathLine.Draw3D ();
	}


	public void movePointOverTime() {

		if (c < overtimePoints.Count) {
			if (Vector3.Distance (transform.position, overtimePoints [c]) > 0.2f) {
				transform.position = Vector3.MoveTowards(transform.position, overtimePoints [c], Time.deltaTime * 10);

			} else {
				c++;
			}
		} else {
			canDraw = false;

		}
	}



	public Vector3 targetPosition;

	public void moveToNextPoint() {

		if (Vector3.Distance (transform.localPosition, targetPosition) > 0.2f) {
			float speed = Vector3.Distance(initPos, targetPosition)*4;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);

		}
		else {
			canDraw = false;


		}

	}

		
	public void MapPoints() {

		//Setting Mins and Maxes
		xMin = PointManager.instance.xMin;
		xMax = PointManager.instance.xMax;
		yMin = PointManager.instance.yMin;
		yMax = PointManager.instance.yMax;
		zMin = PointManager.instance.zMin;
		zMax = PointManager.instance.zMax;
		sizeMin = PointManager.instance.sizeMin;
		sizeMax = PointManager.instance.sizeMax;

		xValue = Random.Range (xMin, xMax);
		yValue = Random.Range (yMin, yMax);
		zValue = Random.Range (zMin, zMax);
		size = Random.Range (sizeMin, sizeMax);

		//Nationality Assigning
		//selectedNationality = (int) nationality;
		selectedNationality = (int)Random.Range(0,3);


		//Remapping values to mins and Maxes
		xValue = Remap (xValue, xMin, xMax, 0, 40);
		yValue = Remap (yValue, yMin, yMax, 0, 40);
		zValue = Remap (zValue, zMin, zMax, 0, 40);
		size = Remap (size, sizeMin, sizeMax, 0.3f, 1);

		//print(selectedNationality);
		mat.color = PointManager.instance.nationalityColors[selectedNationality];
		GetComponent<Renderer>().material.color = mat.color;
		//Changing Sizes
		gameObject.transform.localScale = new Vector3 (size,size,size);
		//Changing Positions
		transform.localPosition = new Vector3(xValue,yValue,zValue);
		initPos = transform.position;
		startPos = transform.localPosition;

		if (selectedNationality == 0) {

			PointManager.instance.c1.Add (this.gameObject);

		}
		else if(selectedNationality == 1) {
			PointManager.instance.c2.Add (this.gameObject);
		}
		else if(selectedNationality == 2) {
			PointManager.instance.c3.Add (this.gameObject);
		}


		updateText ();


	}



	public void changeColor() {
		Material tempMat;
		tempMat = GetComponent<Renderer>().material;

		tempMat.color = Color.white;
		//mat.color = Color.white;
		GetComponent<Renderer>().material.color = tempMat.color;

	}

	public void resetColor() {
		mat.color = PointManager.instance.nationalityColors[selectedNationality];

		GetComponent<Renderer>().material.color = mat.color;

	}

	public void ReMapPoints() {

		float reX, reY, reZ, reSize;
		reX = xValue;
		reY = yValue;

		xValue = reY;
		yValue = reX;
		transform.position = new Vector3(xValue,yValue,zValue);
		initPos = transform.position;

	}


	public  float Remap(this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}


	public void updateText() {

		//xAxis.text = "X: " + xValue.ToString();
		//yAxis.text = "Y: " + yValue.ToString();
		//zAxis.text = "Z: " + zValue.ToString();
		//sizeAxis.text = "Size: " + size.ToString();
		//nAxis.text = "N: " + nationString[selectedNationality];
		Color32 temp = PointManager.instance.nationalityColors[selectedNationality];
		temp.a = 1;
		toChange.color = temp ;

	}



	public void overTimeData() {

		if (overtimePoints.Count < 2) {
			for (int i = 0; i < 4; i++) {

				if (i != 3) {
					Vector3 temp = new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), Random.Range (zMin, zMax));
					//Remapping values to mins and Maxes
					temp.x = Remap (temp.x, xMin, xMax, 0, 10);
					temp.y = Remap (temp.y, yMin, yMax, 0, 10);
					temp.z = Remap (temp.z, zMin, zMax, 0, 10);
					overtimePoints.Add (temp);
				} 
				else {
					overtimePoints.Add (new Vector3(xValue, yValue, zValue));	
				}
			}
		}

		canDraw = true;

		VectorLine.Destroy (ref pathLine);
		pathLine = new VectorLine("Path", new List<Vector3>(), null, 1f, LineType.Continuous);
		pathLine.color = new Color(Random.Range(0,1.0f),Random.Range(0,1.0f),Random.Range(0,1.0f));
		drawPath ();
	}



}
