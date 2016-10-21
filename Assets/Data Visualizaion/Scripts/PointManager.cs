using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using System.Linq;

public class PointManager : MonoBehaviour {

	private static PointManager mInstance;
	public static PointManager instance {get{return mInstance;}}

	public float xMin, xMax, yMin, yMax, zMin, zMax, sizeMin, sizeMax;

	public Color32 italian, mexican, indian, chinese, japanese, american;
//	[SerializeField]
	public Color32[] nationalityColors = new Color32 [6];

	private List<GameObject> dataPoints = new List<GameObject>();
	public GameObject dataPointPrefab;
	public int numOfDataPoints;
	public GameObject dataParent;
	public List<bool> nationalities;

	//public  Cursor right, left;


	int segments= 250;

	List<Vector3> splinePoints= new List<Vector3>();


	RaycastHit hitL, hitR, hit ;
	Ray ray;
	public bool mouseDown = false;
	public List<GameObject> collected;


	public GameObject LRay, RRay;
	public bool checkL, checkR;

	public GameObject d1, d2, d3;
	public List<GameObject> c1, c2, c3;
	int x, y, z = 0;
	public GameObject timeLine;


	public void stopCheckRayCast() {

		LRay.SetActive (false);
		RRay.SetActive (false);
	}

	public void checkRayCast() {
	
		if(checkL) {
		LRay.SetActive (true);
		}
		if (checkR) {
			RRay.SetActive (true);
		}
		print ("Can Collect");
	}


	void Awake() {

		if (mInstance == null) {

			mInstance = this;
		}
	}
	// Use this for initialization
	void Start () {

		nationalityColors [0] = italian;
		nationalityColors [1] = mexican;
		nationalityColors [2] = indian;
		nationalityColors [3] = chinese;
		nationalityColors [4] = japanese;
		nationalityColors [5] = american;

		for (int i = 0; i < 6; i++) {
			nationalities.Add (true);
		}
			
		generateData ();
		//stopCheckRayCast ();
		Invoke ("reSortList", 0.3f);



	}


	public void generateData() {

		for (int i = 0; i < numOfDataPoints; i++) {
			GameObject clone = (GameObject)Instantiate(dataPointPrefab, transform.position, Quaternion.identity); 
			clone.transform.parent = dataParent.transform;
			dataPoints.Add(clone);
		}
		Invoke ("reSortList", 0.3f);

	}


	public void startTheMove() {

		if(collected.Count > 0){
			resetAll ();
		foreach (GameObject g in dataPoints) {
			g.GetComponent<PointData> ().canDraw = false;
			g.SetActive (false);

		}

		foreach (GameObject g in collected) {

			g.SetActive (true);
			g.GetComponent<PointData> ().overTimeData ();

		}
		//	print ("lets see how many time this prints");
		}
	}


	public void changeToTimeline () {
		VectorLine.Destroy (ref spline);

		foreach (GameObject g in dataPoints) {
			//	g.SetActive (false);
			g.gameObject.transform.parent = timeLine.transform;

			g.gameObject.GetComponent<PointData>().targetPosition = timeLine.transform.gameObject.GetComponent<GenerateTimeLineGrid> ().pointList [z];
			g.gameObject.GetComponent<PointData> ().canDraw = true;
			g.gameObject.GetComponent<PointData> ().moveSpeed = 30;

			z++;
		}
		z = 0;

	}


	public void reSortList() {

		dataPoints = dataPoints.OrderBy( x => x.gameObject.GetComponent<PointData>().size ).ToList();
		c1 = c1.OrderBy( x => x.gameObject.GetComponent<PointData>().selectedNationality ).ToList();
		c2 = c2.OrderBy( x => x.gameObject.GetComponent<PointData>().selectedNationality ).ToList();
		c3 = c3.OrderBy( x => x.gameObject.GetComponent<PointData>().selectedNationality ).ToList();


		d1.gameObject.GetComponent<GenerateGrid> ().MakeGridPoints (c1.Count);
		d2.gameObject.GetComponent<GenerateGrid> ().MakeGridPoints (c2.Count);
		d3.gameObject.GetComponent<GenerateGrid> ().MakeGridPoints (c3.Count);
		timeLine.gameObject.GetComponent<GenerateTimeLineGrid> ().MakeGridPoints (dataPoints.Count);


		//		foreach (GameObject g in dataPoints) {
		//			print(g.gameObject.GetComponent<PointData> ().size);
		//		}

	}
	public void moveToNormal() {
		VectorLine.Destroy (ref spline);

		foreach (GameObject g in dataPoints) {

			g.GetComponent<PointData> ().targetPosition = g.GetComponent<PointData> ().startPos;
			g.gameObject.GetComponent<PointData> ().canDraw = true;
			g.gameObject.transform.parent = dataParent.transform;
			g.gameObject.GetComponent<PointData> ().moveSpeed = 24;

		}

	}


	public void changeToBunch() {

		VectorLine.Destroy (ref spline);

		foreach (GameObject g in c1) {
			g.gameObject.transform.parent = d1.transform;

			g.gameObject.GetComponent<PointData>().targetPosition= d1.transform.gameObject.GetComponent<GenerateGrid> ().pointList [x];
			g.gameObject.GetComponent<PointData> ().canDraw = true;
			g.gameObject.GetComponent<PointData> ().moveSpeed = 20;

			x++;
		}

		foreach (GameObject g in c2) {
			//	g.SetActive (false);
			g.gameObject.transform.parent = d2.transform;

			g.gameObject.GetComponent<PointData>().targetPosition = d2.transform.gameObject.GetComponent<GenerateGrid> ().pointList [y];
			g.gameObject.GetComponent<PointData> ().canDraw = true;
			g.gameObject.GetComponent<PointData> ().moveSpeed = 20;

			y++;
		}

		foreach (GameObject g in c3) {
			//	g.SetActive (false);
			g.gameObject.transform.parent = d3.transform;

			g.gameObject.GetComponent<PointData>().targetPosition = d3.transform.gameObject.GetComponent<GenerateGrid> ().pointList [z];
			g.gameObject.GetComponent<PointData> ().canDraw = true;
			g.gameObject.GetComponent<PointData> ().moveSpeed = 20;

			z++;
		}


		x = 0; y = 0; z = 0;

	}


	public void resetAll() {
		VectorLine.Destroy (ref spline);


		foreach (GameObject g in dataPoints) {
			g.SetActive (true);
			VectorLine.Destroy( ref g.GetComponent<PointData> ().pathLine);
			g.GetComponent<PointData> ().resetColor ();
			g.GetComponent<PointData> ().collected = false;
		}

	}



	public void reMapPoints() {

		VectorLine.Destroy (ref spline);


		foreach (GameObject g in dataPoints) {
			g.SetActive (false);
		}

		foreach(GameObject g in collected){
			g.SetActive (true);
			g.GetComponent<PointData>().ReMapPoints();
		}

	}


	public void initSelecting() {

		VectorLine.Destroy (ref spline);

		foreach(GameObject g in collected){

			g.GetComponent<PointData> ().resetColor ();
			g.GetComponent<PointData> ().collected = false;

		}
		collected.Clear ();
		//check Shoot Ray Cast script to see where the data gets stored
	}

	public void endSelecting() {
		if(collected.Count > 5 ){
			DrawSelectedLine ();
		

		foreach (GameObject g in dataPoints) {
			g.gameObject.SetActive (false);
		}

		foreach(GameObject g in collected){
			g.gameObject.SetActive (true);

			g.GetComponent<PointData> ().resetColor ();
		}
		}


	}
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (KeyCode.Y)) {
			changeToBunch ();
			
		}

		if(Input.GetKeyDown(KeyCode.L)) {

			changeToTimeline ();
		}

 
		if(Input.GetKeyDown(KeyCode.G)) {

			startTheMove ();
		}


		if (Input.GetKeyDown (KeyCode.M)) {

		//	resetAll ();
			moveToNormal();
				
		}


		if (Input.GetKeyDown (KeyCode.Space)) {
			reMapPoints ();
		}



	}



	public void Categorize (int nationality) {
	
		VectorLine.Destroy (ref spline);
		nationalities [nationality] = !nationalities [nationality];

		foreach (GameObject g in dataPoints) {

			if (nationalities [g.GetComponent<PointData> ().selectedNationality] == false) {
				g.gameObject.SetActive (false);
			} else {
				g.gameObject.SetActive (true);
			}

		}
	

	}
	VectorLine spline;
	public Texture lineTexture ;

	Color lineColor = Color.white;
	public Texture lineTex ;


	public void DrawSelectedLine() {
		VectorLine.Destroy (ref spline);
		splinePoints.Clear ();

		foreach(GameObject g in collected){
			splinePoints.Add (g.transform.position);

		}

		spline= new VectorLine("Line", new List<Vector3>(splinePoints.Count), null, 1f, LineType.Continuous);
		spline.drawTransform = dataParent.gameObject.transform;

		for(int i=0; i < splinePoints.Count; i++){
			spline.points3 [i] = new Vector3 (splinePoints[i].x, splinePoints[i].y, splinePoints[i].z);
		}
		spline.Draw3D ();
	}


	public void DrawRelationLine(int nationality) {

		VectorLine.Destroy (ref spline);
		splinePoints.Clear ();
		if(nationalities [nationality]) {

			foreach (GameObject g in dataPoints) {
				if(nationality == g.GetComponent<PointData> ().selectedNationality) {

					splinePoints.Add (g.transform.position);

				}
			}

			spline= new VectorLine("Spline", new List<Vector3>(segments+1), null, 	1.0f, LineType.Continuous);
			spline.MakeSpline (splinePoints.ToArray(), segments, false);
			spline.Draw3D();

		}
		
	}






}
