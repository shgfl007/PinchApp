#undef DEBUG
//#define DEBUG

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackerScript : MonoBehaviour
{

	public MeshFilter rightMesh, leftMesh;
	public Mesh pressedMesh, regularMesh;


	// Settings
	bool hasMirror = true;
	bool showVideoFeed = false;
	bool yzSwitch = true;
	bool tweening = true;
	const bool usingFishEye = false;

	#if DEBUG
	public bool showHSV = false;
	public bool showSearchBox = false;
	public bool showDistBetweenOuterBlues = false;
	public bool showClickROI = false;
	public bool showCentreArea = false;

	public bool showContours = false;
	public bool showProximity = false;
	public bool showAngle = false;
	public bool showROIBetweenBlue = false;

	public bool showBlueThreshold = false;
	public bool showRedThreshold = false;
	public bool showGreenThreshold = false;
	public bool showYellowThreshold = false;
	#endif

	// Tunables
	#if (UNITY_WP_8 || UNITY_WP_8_1 || UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
	float distBetweenOuterBlues_max = 50;
	float distBetweenOuterBlues_min = 20;
	#else
	float distBetweenOuterBlues_max = 70;
	float distBetweenOuterBlues_min = 20;
	#endif
	int erodeSize = 1;
	int dilateSize = 3;
	int maxArea = 900;

	int debugCircleRadius = 3;
	static int maxFrameSkip = 10;

	double searchBoxScalingValue = 0.4;
	int searchBoxOffsetValue = 25;

	int angleThreshold = 160;
	double radiusMultiplier = 5.5;

	// Thresholding values
	int hMin_blue = 1;
	int hMax_blue = 29;
	int sMin_blue = 80;
	int sMax_blue = 255;
	int vMin_blue = 80;
	int vMax_blue = 255;

	int hMin_red = 120;
	int hMax_red = 160;
	int sMin_red = 90;
	int sMax_red = 255;
	int vMin_red = 90;
	int vMax_red = 255;

	int hMin_green = 30;
	int hMax_green = 89;
	int sMin_green = 0;
	int sMax_green = 255;
	int vMin_green = 75;
	int vMax_green = 255;

	int hMin_yellow = 69;
	int hMax_yellow = 115;
	int sMin_yellow = 30;
	int sMax_yellow = 255;
	int vMin_yellow = 100;
	int vMax_yellow = 255;

	// Declarations
	float corrected_depth_left, corrected_depth_right;

	float depth_left = 0.5f;
	float depth_right = 0.5f;

	double distBetweenOuterBlues_left = 0;
	double distBetweenOuterBlues_right = 0;

	Point croppedOffset;
	Mat imgThresholded_blue;
	double centreRadius_left = 0, centreRadius_right = 0;
	double centreArea_left = 0, centreArea_right = 0;

	Mat imgTempConvert, imgTempThreshold;
	int initPhase;

	private static TrackerScript mInstance;
	public static TrackerScript instance { get { return mInstance; } }

	public GameObject rightCursorObject, leftCursorObject, imagePlane;
	public string leftCursorName = "LeftCursorObject";
	public string rightCursorName = "RightCursorObject";
	public bool outputCam = false;

	bool clicked = false;

	List<int> adjacentBlues_indices = new List<int>();
	List<Point> adjacentBlues_locations = new List<Point>();

	Scalar minThreshold_blue, maxThreshold_blue, minThreshold_red, maxThreshold_red, minThreshold_green, maxThreshold_green, minThreshold_yellow, maxThreshold_yellow;

	WebCamTexture webCamTexture;
	WebCamDevice webCamDevice;
	int width, height, halfWidth, halfHeight;

	OpenCvSharp.Rect cropped;
	Mat imgOriginal, cameraPreview;                     //imgOriginal for processing, cameraPreview for viewing
	Texture2D texture;
	int searchTimer = 0;
	Mat erodeElement, dilateElement;
	int waitTime = 0;
	public Cursor leftCursor, rightCursor;
	public Color regularColor, holdColor;

	public int phoneID;

	double max(double a, double b)
	{
		return a > b ? a : b;
	}

	double min(double a, double b)
	{
		return a < b ? a : b;
	}

	double[] BGRtoHSV(double[] BGR)
	{
		double rPrime = BGR[2];
		double gPrime = BGR[1];
		double bPrime = BGR[0];

		double cMax = max(rPrime, max(gPrime, bPrime));
		double cMin = min(rPrime, min(gPrime, bPrime));

		double delta = cMax - cMin;

		double hue = (delta == 0) ? (double)0 :                                                 // Delta is 0
			(cMax == rPrime) ? (double)60 * ((gPrime - bPrime) / delta) : 				// cMax is rPrime
			(cMax == gPrime) ? (double)60 * ((bPrime - rPrime) / delta + (double)2) :  // cMax is gPrime
			(double)60 * ((rPrime - gPrime) / delta + (double)4);   // cMax is bPrime
		if (hue < 0)
			hue += 360;

		double saturation = (cMax == 0) ? (double)0 : // cMax is 0
			(delta / cMax); // cMax is non-zero

		// Value == cMax
		double[] result = { hue / 2, saturation * (double)255, cMax };

		return result;
	}

	bool isInRange(double[] HSV, ref Scalar minThreshold, ref Scalar maxThreshold)
	{
		bool hInRange = HSV[0] >= minThreshold.Val0 && HSV[0] <= maxThreshold.Val0;
		bool sInRange = HSV[1] >= minThreshold.Val1 && HSV[1] <= maxThreshold.Val1;
		bool vInRange = HSV[2] >= minThreshold.Val2 && HSV[2] <= maxThreshold.Val2;

		return hInRange && sInRange && vInRange;
	}

	void Start()
	{
		phoneID = PlayerPrefs.GetInt("phoneID");
		if (phoneID == 0)
		{
			// default tunables
		}
		else
		{
			// alternatives
		}

		initPhase = 0;
		imgTempConvert = new Mat(1, 1, MatType.CV_8UC3);
		imgTempThreshold = new Mat();

		if (mInstance == null)
			mInstance = this;

		// Setup image preview
		if (imagePlane == null)
			imagePlane = GameObject.Find("CameraPreview");

		// Setup game objects
		rightCursorObject = GameObject.Find(rightCursorName);
		leftCursorObject = GameObject.Find(leftCursorName);

		// Initializes elements
		erodeElement = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(erodeSize, erodeSize));
		dilateElement = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(dilateSize, dilateSize));

		if (webCamTexture != null)
			webCamTexture.Stop();

		// Select camera to activate; choose non-front-facing camera if possible
		for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
		{
			if (WebCamTexture.devices[cameraIndex].isFrontFacing == false)
			{
				webCamDevice = WebCamTexture.devices[cameraIndex];
				webCamTexture = new WebCamTexture(webCamDevice.name);
				break;
			}
		}
		if (webCamTexture == null)
		{
			webCamDevice = WebCamTexture.devices[0];
			webCamTexture = new WebCamTexture(webCamDevice.name);
		}

		webCamTexture.requestedFPS = 60;

		// iPhones will only support 640x480
		#if (UNITY_WP_8 || UNITY_WP_8_1 || UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
		webCamTexture.requestedHeight = 480;
		webCamTexture.requestedWidth = 640;
		#else
		webCamTexture.requestedHeight = 240;
		webCamTexture.requestedWidth = 320;
		#endif
		width = 320;
		height = 240;
		halfWidth = width / 2;
		halfHeight = height / 2;

		webCamTexture.Play();
		if (showVideoFeed)
			imagePlane.gameObject.SetActive(true);
		else
			imagePlane.gameObject.SetActive(false);
	}

	void Update()
	{
		#if DEBUG
		if (showVideoFeed)
			imagePlane.gameObject.SetActive(true);
		else
			imagePlane.gameObject.SetActive(false);
		#endif
		switch (initPhase)
		{
		case 0:
			{
				//If you want to use webcamTexture.width and webcamTexture.height on iOS, you have to wait until webcamTexture.didUpdateThisFrame == 1, otherwise these two values will be equal to 16. (http://forum.unity3d.com/threads/webcamtexture-and-error-0x0502.123922/)
				#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
				if (webCamTexture.height > 16 && webCamTexture.width > 16) {
				#else
				if (webCamTexture.didUpdateThisFrame)
				{
				#endif
					texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
					initPhase++;
				}
				return;
			}
		case 1:
			{
				cropped = new OpenCvSharp.Rect(0, 0, 0, 0);

				// Sets up thresholding values
				minThreshold_blue = new Scalar(hMin_blue, sMin_blue, vMin_blue);
				maxThreshold_blue = new Scalar(hMax_blue, sMax_blue, vMax_blue);
				minThreshold_red = new Scalar(hMin_red, sMin_red, vMin_red);
				maxThreshold_red = new Scalar(hMax_red, sMax_red, vMax_red);
				minThreshold_green = new Scalar(hMin_green, sMin_green, vMin_green);
				maxThreshold_green = new Scalar(hMax_green, sMax_green, vMax_green);
				minThreshold_yellow = new Scalar(hMin_yellow, sMin_yellow, vMin_yellow);
				maxThreshold_yellow = new Scalar(hMax_yellow, sMax_yellow, vMax_yellow);

				// Creates cursors with proper image width and height
				if (LeftCursor.instance)
					leftCursor = LeftCursor.instance.Init(width, height, usingFishEye);
				if (RightCursor.instance)
					rightCursor = RightCursor.instance.Init(width, height, usingFishEye);
				if (RayCast.instance)
					RayCast.instance.Init(leftCursor, rightCursor);
				initPhase++;
			}
			break;
		default:
			break;
		}

		#if (UNITY_WP_8 || UNITY_WP_8_1 || UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
		if (webCamTexture.width > 16 && webCamTexture.height > 16) { 
		#else
		if (webCamTexture.didUpdateThisFrame)
		{
		#endif
			imgOriginal = getFrame(webCamTexture, height, width, hasMirror);

			// Sets up preview image
			if (outputCam)
			{
				cameraPreview = new Mat();
				imgOriginal.CopyTo(cameraPreview);
			}

			#if DEBUG
			Mat thresholdPreview_blue = new Mat();
			Mat thresholdPreview_red = new Mat();
			Mat thresholdPreview_green = new Mat();
			Mat thresholdPreview_yellow = new Mat();
			getThresholdImage(imgOriginal, ref thresholdPreview_blue, minThreshold_blue, maxThreshold_blue, dilateElement, erodeElement);
			getThresholdImage(imgOriginal, ref thresholdPreview_red, minThreshold_red, maxThreshold_red);
			getThresholdImage(imgOriginal, ref thresholdPreview_green, minThreshold_green, maxThreshold_green);
			getThresholdImage(imgOriginal, ref thresholdPreview_yellow, minThreshold_yellow, maxThreshold_yellow);
			#endif

			#if !DEBUG
			// If cursors not found for maxFrameSkip frames, activate lazy mode
			if (rightCursor.skippedFrames >= maxFrameSkip && leftCursor.skippedFrames >= maxFrameSkip)
			{
			waitTime++;
			if (waitTime > 5)
			waitTime = 0;
			else
			{
			if (outputCam && showVideoFeed)
			{
			#if DEBUG
			displayOutput(ref cameraPreview, ref thresholdPreview_blue, ref thresholdPreview_red, ref thresholdPreview_green, ref thresholdPreview_yellow, ref imagePlane, showBlueThreshold, showRedThreshold, showGreenThreshold, showYellowThreshold, ref texture);
			#else
			displayOutput(ref cameraPreview, ref imagePlane, ref texture);
			#endif
			}
			return;
			}
			}
			#endif

			// Crops image to save computational time
			croppedOffset = new Point(0, 0);
			if (cropped.Width > 1 && cropped.Height > 1 && false) // turned off because this causes horizontal cursor jitter due to yZSwitch
			{
				imgOriginal = new Mat(imgOriginal, cropped);
				#if DEBUG
				if (showSearchBox)
					Cv2.Rectangle(cameraPreview, new Point(cropped.X, cropped.Y), new Point(cropped.X + cropped.Width, cropped.Y + cropped.Height), new Scalar(0, 255, 255), 1, LineTypes.Link8, 0);
				#endif
				croppedOffset.X = cropped.X;
				croppedOffset.Y = cropped.Y;
			}

			imgThresholded_blue = new Mat();
			getThresholdImage(imgOriginal, ref imgThresholded_blue, minThreshold_blue, maxThreshold_blue, dilateElement, erodeElement);
			#if DEBUG
			if (showHSV)
				enableHSVRetriever(ref cameraPreview, width, height, debugCircleRadius);
			#endif
			// Bool variables for cursor finding
			bool rightCursorFound = false;
			bool leftCursorFound = false;

			// Finds and stores blue contours
			Point[][] contours_blue = new Point[imgThresholded_blue.Rows][];
			HierarchyIndex[] cHierarchy_blue = new HierarchyIndex[imgThresholded_blue.Rows];
			Cv2.FindContours(imgThresholded_blue, out contours_blue, out cHierarchy_blue, RetrievalModes.List, ContourApproximationModes.ApproxSimple, null);

			// Finds potential cursors
			List<Point> leftCursor_centre = new List<Point>();
			List<Point> rightCursor_centre = new List<Point>();
			List<List<Point>> leftCursor_outer = new List<List<Point>>();
			List<List<Point>> rightCursor_outer = new List<List<Point>>();
			leftCursor_centre.Clear();
			rightCursor_centre.Clear();
			leftCursor_outer.Clear();
			rightCursor_outer.Clear();

			// Obtain bounding rectangles to get centroid and area
			OpenCvSharp.Rect[] bRect_blue = new OpenCvSharp.Rect[contours_blue.Length];
			double[] area_blue = new double[contours_blue.Length];
			for (int i = 0; i < contours_blue.Length; i++)
			{
				bRect_blue[i] = Cv2.BoundingRect(contours_blue[i]);
				area_blue[i] = Cv2.ContourArea(contours_blue[i]);
			}

			centreRadius_left = 0;
			centreRadius_right = 0;
			centreArea_left = 0;
			centreArea_right = 0;
			// Search each blue contour to find centre blue of Pincher
			for (int i = 0; i < contours_blue.Length; i++)
			{
				double area_centre = area_blue[i];
				#if DEBUG
				double radius_centre = Math.Sqrt(area_centre / Math.PI);
				OpenCvSharp.Rect bRect = bRect_blue[i];
				Point point_centre = new Point(bRect.X + (bRect.Width / 2), bRect.Y + (bRect.Height / 2));
				if (showContours)
					Cv2.Circle(cameraPreview, new Point(point_centre.X + croppedOffset.X, point_centre.Y + croppedOffset.Y), (int)radius_centre, new Scalar(255, 0, 0), 2);
				#endif
				if (area_centre <= maxArea)
				{
					#if !DEBUG
					double radius_centre = Math.Sqrt(area_centre / Math.PI);
					OpenCvSharp.Rect bRect = bRect_blue[i];
					Point point_centre = new Point(bRect.X + (bRect.Width / 2), bRect.Y + (bRect.Height / 2));
					#endif
					// Count Adjacent Blue Dots
					int adjacentBlues_amount = 0;
					adjacentBlues_indices.Clear();
					adjacentBlues_locations.Clear();

					#if DEBUG
					double radiusAverage = Math.Sqrt(area_centre / Math.PI);
					double radiusOfProximity = radiusAverage * radiusMultiplier;
					if (showProximity)
						Cv2.Circle(cameraPreview, new Point(point_centre.X + croppedOffset.X, point_centre.Y + croppedOffset.Y), (int)radiusOfProximity, new Scalar(255, 255, 0), 2);
					#endif

					// Store all adjacent blues
					for (int j = 0; j < contours_blue.Length; j++)
					{
						if (j != i)
						{
							double area_otherBlue = area_blue[j];

							if (area_otherBlue <= maxArea)
							{
								bool blueIsAdjacent = false;
								Point point_otherBlue = new Point(0, 0);
								double radius_otherBlue = Math.Sqrt(area_otherBlue / Math.PI);
								bRect = bRect_blue[j];
								point_otherBlue = new Point(bRect.X + (bRect.Width / 2), bRect.Y + (bRect.Height / 2));

								blueIsAdjacent = getBlueIsAdjacent(area_otherBlue, point_otherBlue, radius_otherBlue, point_centre, area_centre, radiusMultiplier, ref cameraPreview, croppedOffset, width, height);

								if (blueIsAdjacent)
								{
									adjacentBlues_amount++;
									adjacentBlues_indices.Add(j);
									adjacentBlues_locations.Add(point_otherBlue);
								}
							}
						}
					}

					double angle = 0;
					if (adjacentBlues_amount >= 2 && adjacentBlues_amount <= 7)
					{
						bool bluesAreInLine = getBluesAreInLine(ref angle, ref adjacentBlues_locations, point_centre, angleThreshold);
						#if DEBUG
						if (showAngle)
						{
							showDebugText(ref cameraPreview, point_centre, croppedOffset, width, height, "Angle: " + ((int)angle).ToString());
							Cv2.Circle(cameraPreview, new Point(point_centre.X + croppedOffset.X, point_centre.Y + croppedOffset.Y), (int)radius_centre, new Scalar(255, 0, 0), 2);
						}
						#endif
						if (bluesAreInLine)
						{
							#if DEBUG
							if (showROIBetweenBlue)
								drawCursorDesignationROI(ref cameraPreview, point_centre, croppedOffset, radius_centre, debugCircleRadius, imgOriginal, adjacentBlues_locations);
							#endif
							// designates the centre blue contour as left cursor (value = 1), right cursor (value = 2), or neither (value = 0)
							int cursorDesignation = getCursorDesignation(imgOriginal, point_centre, radius_centre, minThreshold_red, maxThreshold_red, minThreshold_yellow, maxThreshold_yellow, adjacentBlues_locations);

							bool hasRedBetweenBlue = false;
							bool hasRedYellowBetweenBlue = false;

							if (cursorDesignation == 1)
								hasRedBetweenBlue = true;
							else if (cursorDesignation == 2)
								hasRedYellowBetweenBlue = true;

							// Left
							if (hasRedBetweenBlue)
							{
								adjacentBlues_locations[0] = new Point(adjacentBlues_locations[0].X + croppedOffset.X, adjacentBlues_locations[0].Y + croppedOffset.Y);
								adjacentBlues_locations[1] = new Point(adjacentBlues_locations[1].X + croppedOffset.X, adjacentBlues_locations[1].Y + croppedOffset.Y);
								List<Point> adjacentDotLocationsCopy = new List<Point>(adjacentBlues_locations); // List is a reference type

								leftCursor_centre.Insert(0, new Point(point_centre.X + croppedOffset.X, point_centre.Y + croppedOffset.Y));
								leftCursor_outer.Insert(0, adjacentDotLocationsCopy);
								centreRadius_left = radius_centre;
								centreArea_left = area_centre;
								leftCursorFound = true;
							}
							// Right
							if (hasRedYellowBetweenBlue)
							{
								adjacentBlues_locations[0] = new Point(adjacentBlues_locations[0].X + croppedOffset.X, adjacentBlues_locations[0].Y + croppedOffset.Y);
								adjacentBlues_locations[1] = new Point(adjacentBlues_locations[1].X + croppedOffset.X, adjacentBlues_locations[1].Y + croppedOffset.Y);
								List<Point> adjacentDotLocationsCopy = new List<Point>(adjacentBlues_locations); // List is a reference type

								rightCursor_centre.Insert(0, new Point(point_centre.X + croppedOffset.X, point_centre.Y + croppedOffset.Y));
								rightCursor_outer.Insert(0, adjacentDotLocationsCopy);
								centreRadius_right = radius_centre;
								centreArea_right = area_centre;
								rightCursorFound = true;
							}                            
						}
					}
				}
			}

			#if DEBUG
			if (showCentreArea)
			{
				if (leftCursorFound)
					showDebugText(ref cameraPreview, leftCursor_centre[0], new Point(0, 0), width, height, "Area: " + ((int)centreArea_left).ToString());
				if (rightCursorFound)
					showDebugText(ref cameraPreview, rightCursor_centre[0], new Point(0, 0), width, height, "Area: " + ((int)centreArea_right).ToString());
			}
			#endif

			// Obtain depth
			if (leftCursor_outer.Count != 0)
			{
				distBetweenOuterBlues_left = getDistBetweenOuterBlues(leftCursor_outer[0], leftCursor_centre[0]);
				depth_left = map((float)distBetweenOuterBlues_left, distBetweenOuterBlues_max, distBetweenOuterBlues_min, 0, 1);
			}
			if (rightCursor_outer.Count != 0)
			{
				distBetweenOuterBlues_right = getDistBetweenOuterBlues(rightCursor_outer[0], rightCursor_centre[0]);
				depth_right = map((float)distBetweenOuterBlues_right, distBetweenOuterBlues_max, distBetweenOuterBlues_min, 0, 1);
			}

			//-------------------------------------------------
			// HANDLING WHETHER OR NOT CURSORS ARE FOUND
			//-------------------------------------------------

			clicked = false;
			if (leftCursorFound)
			{
				Point vector = leftCursor_outer[0][0] - leftCursor_outer[0][1];
				double magnitude = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
				double unitVector_X = vector.X / magnitude;
				double unitVector_Y = vector.Y / magnitude;
				double unitNormal_X = -unitVector_Y;
				double unitNormal_Y = unitVector_X;

				clicked = getClickState(leftCursor_centre[0], leftCursor_outer, centreRadius_left, unitVector_X, unitVector_Y, unitNormal_X, unitNormal_Y, imgOriginal);

				if (yzSwitch)
				{
					// Left
					float backupY = leftCursor_centre[0].Y;
					leftCursor_centre[0] = new Point(leftCursor_centre[0].X, height - depth_left * height);
					depth_left = backupY / height;
				}

				//Point cursorPoint = findNearestPoint(ref leftCursor, ref leftCursor_centre);

				leftCursor.addCoordinate(leftCursor_centre[0], true);

				#if DEBUG
				if (showClickROI)
					drawClickSearchArea(leftCursor_centre[0], leftCursor_outer, centreRadius_left, unitVector_X, unitVector_Y, unitNormal_X, unitNormal_Y, ref cameraPreview);
				#endif
				leftCursor.addClickState(clicked);
				leftCursor.skippedFrames = 0;
			}
			else
			{ // If not found
				if (leftCursor.skippedFrames > maxFrameSkip)
				{ // voidInit isn't found for maxFrameSkip frames
					leftCursor.addCoordinate(leftCursor.previousData[0], false); // LKU:
					leftCursor.addClickState(false);
				}
				else
				{ // If cursor momentarily isn't found
					leftCursor.addCoordinate(leftCursor.previousData[0], true); // LKU:
					leftCursor.skippedFrames++;
				}
			}

			if (rightCursorFound)
			{
				Point vector = rightCursor_outer[0][0] - rightCursor_outer[0][1];
				double magnitude = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
				double unitVector_X = vector.X / magnitude;
				double unitVector_Y = vector.Y / magnitude;
				double unitNormal_X = -unitVector_Y;
				double unitNormal_Y = unitVector_X;

				clicked = getClickState(rightCursor_centre[0], rightCursor_outer, centreRadius_right, unitVector_X, unitVector_Y, unitNormal_X, unitNormal_Y, imgOriginal);

				if (yzSwitch)
				{
					// Right
					float backupY = rightCursor_centre[0].Y;
					rightCursor_centre[0] = new Point(rightCursor_centre[0].X, height - depth_right * height);
					depth_right = backupY / height;
				}

				//Point cursorPoint = findNearestPoint(ref rightCursor, ref rightCursor_centre);

				rightCursor.addCoordinate(rightCursor_centre[0], true);

				#if DEBUG
				if (showClickROI)
					drawClickSearchArea(rightCursor_centre[0], rightCursor_outer, centreRadius_right, unitVector_X, unitVector_Y, unitNormal_X, unitNormal_Y, ref cameraPreview);
				#endif
				rightCursor.addClickState(clicked);
				rightCursor.skippedFrames = 0;
			}
			else
			{ // If not found
				if (rightCursor.skippedFrames > maxFrameSkip)
				{
					rightCursor.addCoordinate(rightCursor.previousData[0], false); // Turns off prediction after cursor isn't found for maxFrameSkip frames
					rightCursor.addClickState(false);
				}
				else
				{
					rightCursor.addCoordinate(rightCursor.previousData[0], true); // Turns on prediction if cursor momentarily isn't found
					rightCursor.skippedFrames++;
				}
			}

			//-------------------------------------------------
			// UPDATE SEARCH AREA
			//-------------------------------------------------

			// If cursors found, then crop search area to reduce computations
			if (leftCursorFound && rightCursorFound)
			{
				cropped = getSearchArea(width, height, ref leftCursor, ref rightCursor, centreArea_left, centreArea_right);
				searchTimer = 0;
			}
			// If only one cursor found, search entire image every 7 frames
			else if (leftCursorFound || rightCursorFound)
			{
				if (searchTimer > 7)
				{
					searchTimer = 0;
					cropped.X = 0;
					cropped.Y = 0;
					cropped.Width = 0;
					cropped.Height = 0;
				}
				else
				{
					cropped = getSearchArea(width, height, ref leftCursor, ref rightCursor, centreArea_left, centreArea_right);
					searchTimer++;
				}
			}
			else
			{
				cropped.X = 0;
				cropped.Y = 0;
				cropped.Width = 0;
				cropped.Height = 0;
			}

			//-------------------------------------------------
			// SETS IN-APP CURSOR POSITIONS
			//-------------------------------------------------

			// Convert points for Unity integration
			Point leftPoint = new Point(width - leftCursor.getPosition().X, leftCursor.getPosition().Y);
			Point rightPoint = new Point(width - rightCursor.getPosition().X, rightCursor.getPosition().Y);

			//Correct coordinates according to mirror angle
			if (hasMirror)
			{
				Quaternion rotate = Quaternion.AngleAxis(-30, Vector3.right);

				corrected_depth_left = depth_left * 240;
				Vector3 coordinate = new Vector3(leftPoint.X, leftPoint.Y, corrected_depth_left);
				Vector3 correctedCoordinate = rotate * coordinate;
				leftPoint.X = (int)correctedCoordinate.x;
				leftPoint.Y = (int)correctedCoordinate.y;
				corrected_depth_left = correctedCoordinate.z;
				corrected_depth_left /= 240;

				corrected_depth_right = depth_right * 240;
				coordinate = new Vector3(rightPoint.X, rightPoint.Y, corrected_depth_right);
				correctedCoordinate = rotate * coordinate;
				rightPoint.X = (int)correctedCoordinate.x;
				rightPoint.Y = (int)correctedCoordinate.y;
				corrected_depth_right = correctedCoordinate.z;
				corrected_depth_right /= 240;
			}

			// Convert position coordinates for the screen
			leftPoint.X = (leftPoint.X - halfWidth);
			leftPoint.Y = (halfHeight - leftPoint.Y);
			rightPoint.X = (rightPoint.X - halfWidth);
			rightPoint.Y = (halfHeight - rightPoint.Y);
			float scaleFactor_x = 1.1f;
			float scaleFactor_y = 1.1f;

			if (tweening)
			{
				if (leftCursor.cursorFound())
				{
					leftCursor.x = leftPoint.X * -scaleFactor_x;

					if (hasMirror)
						leftCursor.depth = corrected_depth_left;
					else
						leftCursor.depth = depth_left;
					leftCursor.depthScale = map(leftCursor.depth, 0, 1, 0.9f, 0.1f);
					leftCursor.y = leftPoint.Y * -scaleFactor_y;

					#if DEBUG
					if (showDistBetweenOuterBlues)
						showDebugText(ref cameraPreview, leftCursor.previousData[0], new Point(0, 0), width, height, "Dot Dist: " + ((int)distBetweenOuterBlues_left).ToString());
					#endif
				}
				if (rightCursor.cursorFound())
				{
					rightCursor.x = rightPoint.X * -scaleFactor_x;

					if (hasMirror)
						rightCursor.depth = corrected_depth_right;
					else
						rightCursor.depth = depth_right;
					rightCursor.depthScale = map(rightCursor.depth, 0, 1, 0.9f, 0.1f);
					rightCursor.y = rightPoint.Y * -scaleFactor_y;

					#if DEBUG
					if (showDistBetweenOuterBlues)
						showDebugText(ref cameraPreview, rightCursor.previousData[0], new Point(0, 0), width, height, "Dot Dist: " + ((int)distBetweenOuterBlues_right).ToString());
					#endif
				}

				// Offset to bring cursors closer together
				if (leftCursorFound && rightCursorFound)
				{
					leftCursor.x -= 15;
					rightCursor.x += 15;
				}
			}
			else
			{
				leftCursorObject.transform.localPosition = new Vector3(leftPoint.X * scaleFactor_x, leftPoint.Y * scaleFactor_y, 1f);
				rightCursorObject.transform.localPosition = new Vector3(rightPoint.X * scaleFactor_x, rightPoint.Y * scaleFactor_y, 1f);
			}
			Renderer leftRender = null;
			if (leftCursorObject)
			{
				leftRender = leftCursorObject.GetComponent<Renderer>();
			}
			Renderer rightRender = null;
			if (rightCursorObject)
			{
				rightRender = rightCursorObject.GetComponent<Renderer>();
			}

			// Click states
			if (!leftCursor.getClickState() && leftRender != null && rightRender != null){
				leftRender.material.color = regularColor;
				leftMesh.mesh = regularMesh;
			}
			else{
				leftRender.material.color = holdColor;
				leftMesh.mesh = pressedMesh;
			}
			if (!rightCursor.getClickState()){
				rightMesh.mesh = regularMesh;
				rightRender.material.color = regularColor;}
			else{
				rightMesh.mesh = pressedMesh;
				rightRender.material.color = holdColor;
			}

			// Display preview image
			if (outputCam && showVideoFeed)
			{
				#if DEBUG
				displayOutput(ref cameraPreview, ref thresholdPreview_blue, ref thresholdPreview_red, ref thresholdPreview_green, ref thresholdPreview_yellow, ref imagePlane, showBlueThreshold, showRedThreshold, showGreenThreshold, showYellowThreshold, ref texture);
				#else
				displayOutput(ref cameraPreview, ref imagePlane, ref texture);
				#endif
			}
		}
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("phoneID", 0);
	}

	// get nearest potential cursor from last known
	Point findNearestPoint(ref Cursor cursor, ref List<Point> potentialCursors_centre)
	{
		Point prediction = cursor.getPrediction();
		Point closest = new Point();
		float lowestDistance = 100000;
		for (int i = 0; i < potentialCursors_centre.Count; i++)
		{
			float xDiff = Mathf.Abs((float)(potentialCursors_centre[i].X - prediction.X));
			float yDiff = Mathf.Abs((float)(potentialCursors_centre[i].Y - prediction.Y));
			float totalDistance = Mathf.Pow(xDiff * xDiff + yDiff * yDiff, 0.5f);  // find hypotenuse
			if (totalDistance < lowestDistance)
			{
				closest = potentialCursors_centre[i];
				lowestDistance = totalDistance;
			}
		}

		return closest;
	}

	OpenCvSharp.Rect getSearchArea(int width, int height, ref Cursor leftCursor, ref Cursor rightCursor, double centreArea_left, double centreArea_right)
	{
		double centreArea_max = Math.Max(centreArea_left, centreArea_right);
		Point leftCursorOffset = leftCursor.getPredictiveOffset();
		Point leftCursorCoord = leftCursor.previousCoordinates[0];
		Point rightCursorOffset = rightCursor.getPredictiveOffset();
		Point rightCursorCoord = rightCursor.previousCoordinates[0];
		int minimum = 5;
		int offsetAmount = (int)(8 * Mathf.Max(leftCursorOffset.X, Mathf.Max(leftCursorOffset.Y, Mathf.Max(rightCursorOffset.X, Mathf.Max((float)rightCursorOffset.Y, minimum)))) + centreArea_max * searchBoxScalingValue - searchBoxOffsetValue);

		// get top left and bottom right points
		Point topLeft = new Point();
		Point bottomRight = new Point();

		//-------------------------
		// SET CORNERS
		//-------------------------
		if (leftCursor.cursorFound() && !rightCursor.cursorFound())
		{
			topLeft.X = leftCursorCoord.X;
			topLeft.Y = leftCursorCoord.Y;
			bottomRight.X = leftCursorCoord.X;
			bottomRight.Y = leftCursorCoord.Y;
		}
		else if (!leftCursor.cursorFound() && rightCursor.cursorFound())
		{
			topLeft.X = rightCursorCoord.X;
			topLeft.Y = rightCursorCoord.Y;
			bottomRight.X = rightCursorCoord.X;
			bottomRight.Y = rightCursorCoord.Y;
		}
		else
		{
			if (rightCursorCoord.X <= leftCursorCoord.X)
			{
				topLeft.X = rightCursorCoord.X;
				bottomRight.X = leftCursorCoord.X;
			}
			else
			{
				topLeft.X = leftCursorCoord.X;
				bottomRight.X = rightCursorCoord.X;
			}
			if (rightCursorCoord.Y <= leftCursorCoord.Y)
			{
				topLeft.Y = rightCursorCoord.Y;
				bottomRight.Y = leftCursorCoord.Y;
			}
			else
			{
				topLeft.Y = leftCursorCoord.Y;
				bottomRight.Y = rightCursorCoord.Y;
			}
		}

		//-------------------------
		// SET OFFSET
		//-------------------------
		// TOP LEFT
		if (topLeft.X - offsetAmount >= 0)
			topLeft.X = topLeft.X - offsetAmount;
		else
			topLeft.X = 0;

		if (topLeft.Y - offsetAmount >= 0)
			topLeft.Y = topLeft.Y - offsetAmount;
		else
			topLeft.Y = 0;

		// BOTTOM RIGHT
		if (bottomRight.X + offsetAmount < width)
			bottomRight.X = bottomRight.X + offsetAmount;
		else
			bottomRight.X = width;

		if (bottomRight.Y + offsetAmount < height) // height/2 because only half the camera feed is taken
			bottomRight.Y = bottomRight.Y + offsetAmount;
		else
			bottomRight.Y = height;

		OpenCvSharp.Rect area = new OpenCvSharp.Rect(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

		return area;
	}

	Mat getFrame(WebCamTexture webCamTexture, int height, int width, bool hasMirror)
	{
		Mat frame = new Mat(height, width, MatType.CV_8UC3);
		#if (UNITY_WP_8 || UNITY_WP_8_1 || UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
		Mat sizePlaceHolder = new Mat (webCamTexture.height, webCamTexture.width, MatType.CV_8UC3);;
		OpencvSharpUtils.webCamTextureToMat(webCamTexture, sizePlaceHolder);		
		Cv2.Resize (sizePlaceHolder, frame, new Size(width, height));
		#else
		OpencvSharpUtils.webCamTextureToMat(webCamTexture, frame);
		#endif

		#if (UNITY_WP_8 || UNITY_WP_8_1 || UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
		if (hasMirror)
		Cv2.Flip(frame, frame, FlipMode.X);
		#else
		Cv2.Flip(frame, frame, FlipMode.X);
		#endif
		return frame;
	}

	void getThresholdImage(Mat imgOriginal, ref Mat thresholdPreview, Scalar minThreshold, Scalar maxThreshold, Mat dilateElement, Mat erodeElement)
	{
		Mat imgHSV = new Mat();
		Cv2.CvtColor(imgOriginal, imgHSV, ColorConversionCodes.BGR2HSV);
		Cv2.InRange(imgHSV, minThreshold, maxThreshold, thresholdPreview);
		Cv2.Erode(thresholdPreview, thresholdPreview, erodeElement);
		Cv2.Dilate(thresholdPreview, thresholdPreview, dilateElement);
	}

	void getThresholdImage(Mat imgOriginal, ref Mat thresholdPreview, Scalar minThreshold, Scalar maxThreshold)
	{
		Mat imgHSV = new Mat();
		Cv2.CvtColor(imgOriginal, imgHSV, ColorConversionCodes.BGR2HSV);
		Cv2.InRange(imgHSV, minThreshold, maxThreshold, thresholdPreview);
	}

	#if DEBUG
	void displayOutput(ref Mat cameraPreview, ref Mat thresholdPreview_blue, ref Mat thresholdPreview_red, ref Mat thresholdPreview_green, ref Mat thresholdPreview_yellow, ref GameObject imagePlane, bool showBlueThreshold, bool showRedThreshold, bool showGreenThreshold, bool showYellowThreshold, ref Texture2D texture)
	{
		if (imagePlane == null)
			imagePlane = GameObject.Find("CameraPreview");
		if (showBlueThreshold)
			OpencvSharpUtils.matToTexture2D(thresholdPreview_blue, texture);
		else if (showRedThreshold)
			OpencvSharpUtils.matToTexture2D(thresholdPreview_red, texture);
		else if (showGreenThreshold)
			OpencvSharpUtils.matToTexture2D(thresholdPreview_green, texture);
		else if (showYellowThreshold)
			OpencvSharpUtils.matToTexture2D(thresholdPreview_yellow, texture);
		else
			OpencvSharpUtils.matToTexture2D(cameraPreview, texture);
		imagePlane.GetComponent<Renderer>().material.mainTexture = texture;
	}
	#else
	void displayOutput(ref Mat cameraPreview, ref GameObject imagePlane, ref Texture2D texture)
	{
	if (imagePlane == null)
	imagePlane = GameObject.Find("CameraPreview");
	OpencvSharpUtils.matToTexture2D(cameraPreview, texture);
	imagePlane.GetComponent<Renderer>().material.mainTexture = texture;
	}
	#endif
	#if DEBUG
	void enableHSVRetriever(ref Mat cameraPreview, int width, int height, int debugCircleRadius)
	{
		Cv2.Circle(cameraPreview, new Point(width / 2, height / 2), debugCircleRadius, new Scalar(255, 0, 0));
		Mat cameraHSV = new Mat();
		Cv2.CvtColor(cameraPreview, cameraHSV, ColorConversionCodes.BGR2HSV);
		Vec3b HSV = cameraHSV.At<Vec3b>(height / 2, width / 2);
		double H = HSV[0];
		double S = HSV[1];
		double V = HSV[2];

		Cv2.Flip(cameraPreview, cameraPreview, FlipMode.X);
		Cv2.Flip(cameraPreview, cameraPreview, FlipMode.Y);
		Cv2.PutText(cameraPreview, "H: " + H.ToString(), new Point(width - 220, height - 175), HersheyFonts.HersheySimplex, 0.3, new Scalar(255, 0, 0));
		Cv2.PutText(cameraPreview, "S: " + S.ToString(), new Point(width - 220, height - 165), HersheyFonts.HersheySimplex, 0.3, new Scalar(255, 0, 0));
		Cv2.PutText(cameraPreview, "V: " + V.ToString(), new Point(width - 220, height - 155), HersheyFonts.HersheySimplex, 0.3, new Scalar(255, 0, 0));
		Cv2.Flip(cameraPreview, cameraPreview, FlipMode.Y);
		Cv2.Flip(cameraPreview, cameraPreview, FlipMode.X);
	}

	void showDebugText(ref Mat img, Point point, Point offset, int width, int height, string text)
	{
		Point correctedPoint = new Point(width - (point.X + offset.X), height - (point.Y + offset.Y));
		Cv2.Flip(img, img, FlipMode.X);
		Cv2.Flip(img, img, FlipMode.Y);
		Cv2.PutText(img, text, correctedPoint, HersheyFonts.HersheySimplex, 0.3, new Scalar(255, 0, 0));
		Cv2.Flip(img, img, FlipMode.Y);
		Cv2.Flip(img, img, FlipMode.X);
	}
	#endif

	int getCursorDesignation(Mat img, Point point_centre, double radius_centre, Scalar minThreshold_red, Scalar maxThreshold_red, Scalar minThreshold_yellow, Scalar maxThreshold_yellow, List<Point> adjacentBlues_locations)
	{
		int redPixels = 0;
		int yellowPixels = 0;
		int redBetweenBlue = 0;
		int yellowBetweenBlue = 0;
		Point midpoint;

		foreach (Point point_adjacent in adjacentBlues_locations)
		{
			redPixels = 0;
			yellowPixels = 0;
			midpoint = getMidpoint(point_centre, point_adjacent);
			for (int y = (int)(midpoint.Y - radius_centre * 0.6); y < midpoint.Y + radius_centre * 0.6 && redPixels < 3 && yellowPixels < 3; ++y)
			{
				for (int x = (int)(midpoint.X - radius_centre * 0.6); x < midpoint.X + radius_centre * 0.6 && redPixels < 3 && yellowPixels < 3; ++x)
				{
					if (y >= 0 && y < img.Rows && x >= 0 && x < img.Cols)
					{
						Vec3b BGR_Vec3b = img.At<Vec3b>(y, x);
						double[] BGR = { BGR_Vec3b[0], BGR_Vec3b[1], BGR_Vec3b[2] };
						double[] HSV = BGRtoHSV(BGR);
						bool isRed = isInRange(HSV, ref minThreshold_red, ref maxThreshold_red);

						if (isRed)
							redPixels++;

						bool isYellow = isInRange(HSV, ref minThreshold_yellow, ref maxThreshold_yellow);

						if (isYellow)
							yellowPixels++;
					}
				}
			}
			if (redPixels >= 3)
				redBetweenBlue++;
			if (yellowPixels >= 3)
				yellowBetweenBlue++;
		}
		if (redBetweenBlue == 2)
			return 1;
		else if (redBetweenBlue == 1 && yellowBetweenBlue == 1)
			return 2;
		else
			return 0;
	}

	void drawCursorDesignationROI(ref Mat cameraPreview, Point point_centre, Point croppedOffset, double radius_centre, int debugCircleRadius, Mat img, List<Point> adjacentBlues_locations)
	{
		Point midpoint;

		foreach (Point point_adjacent in adjacentBlues_locations)
		{
			midpoint = getMidpoint(point_centre, point_adjacent);
			for (int y = (int)(midpoint.Y - radius_centre * 0.6); y < midpoint.Y + radius_centre * 0.6; y += 2)
			{
				for (int x = (int)(midpoint.X - radius_centre * 0.6); x < midpoint.X + radius_centre * 0.6; x += 2)
				{
					if (y >= 0 && y < img.Rows && x >= 0 && x < img.Cols)
					{
						Point searchPoint = new Point(x + croppedOffset.X, y + croppedOffset.Y);
						Cv2.Circle(cameraPreview, searchPoint, debugCircleRadius, new Scalar(0, 0, 0));
					}
				}
			}
		}
	}

	Point getMidpoint(Point pointA, Point pointB)
	{
		return new Point((pointA.X + pointB.X) * 0.5, (pointA.Y + pointB.Y) * 0.5);
	}

	double getDistBetweenOuterBlues(List<Point> adjacentBlues_locations, Point cursor_centre)
	{
		Point pointA = adjacentBlues_locations[0];
		Point pointB = adjacentBlues_locations[1];

		double distBetweenOuterBlues = distanceBetween(pointA, pointB);
		double distBetweenUpperBlues = distanceBetween(pointA, cursor_centre);
		double distBetweenLowerBlues = distanceBetween(pointB, cursor_centre);

		double distBetweenOuterBlues_Average = (distBetweenUpperBlues + distBetweenLowerBlues + distBetweenOuterBlues / 2) / 3 * 2;
		return distBetweenOuterBlues_Average;
	}

	bool getBlueIsAdjacent(double area, Point point, double radius, Point point_centre, double area_centre, double radiusMultiplier, ref Mat cameraPreview, Point croppedOffset, int width, int height)
	{
		double distanceFromCentre = Math.Sqrt(Math.Pow((point.X - point_centre.X), 2) + Math.Pow((point.Y - point_centre.Y), 2));
		double radiusAverage = Math.Sqrt(area_centre / Math.PI);
		double radiusOfProximity = radiusAverage * radiusMultiplier;
		double distanceRatio = Math.Round(Math.Abs((distanceFromCentre)) / radiusOfProximity, 2);

		if (distanceRatio < 1)
			return true;
		return false;
	}

	bool getBluesAreInLine(ref double angle, ref List<Point> adjacentBlues_locations, Point point_centre, int angleThreshold)
	{
		List<Point> savedLocations = new List<Point>();
		for (int a = 0; a < adjacentBlues_locations.Count; a++)
		{
			for (int c = a + 1; c < adjacentBlues_locations.Count; c++)
			{
				if (c != a)
				{
					Point pointA = adjacentBlues_locations[a];
					Point pointB = point_centre;
					Point pointC = adjacentBlues_locations[c];

					double dotProduct = (pointA.X - pointB.X) * (pointC.X - pointB.X) + (pointA.Y - pointB.Y) * (pointC.Y - pointB.Y);
					double magnitudeProduct = (Math.Sqrt((pointA.X - pointB.X) * (pointA.X - pointB.X) + (pointA.Y - pointB.Y) * (pointA.Y - pointB.Y))) * (Math.Sqrt((pointC.X - pointB.X) * (pointC.X - pointB.X) + (pointC.Y - pointB.Y) * (pointC.Y - pointB.Y)));
					angle = Math.Acos(Math.Round(dotProduct / magnitudeProduct, 2)) * 180 / Math.PI;

					if (angle > angleThreshold)
					{
						if (savedLocations.Count == 0)
						{
							savedLocations.Insert(0, pointA);
							savedLocations.Insert(1, pointC);
						}
						else
						{
							double previousDifference = Math.Abs(distanceBetween(savedLocations[0], point_centre) - distanceBetween(savedLocations[1], point_centre));
							double currentDifference = Math.Abs(distanceBetween(pointA, point_centre) - distanceBetween(pointC, point_centre));
							if (currentDifference <= previousDifference)
							{
								savedLocations.RemoveAt(1);
								savedLocations.RemoveAt(0);
								savedLocations.Insert(0, pointA);
								savedLocations.Insert(1, pointC);
							}
						}
					}
				}
			}
		}
		if (savedLocations.Count != 0)
		{
			foreach (Point location in adjacentBlues_locations.ToArray())
			{
				if (!location.Equals(savedLocations[0]) && !location.Equals(savedLocations[1]))
					adjacentBlues_locations.Remove(location);
			}
			return true;
		}
		return false;
	}

	double distanceBetween(Point pointA, Point pointB)
	{
		double xDiff = pointA.X - pointB.X;
		double yDiff = pointA.Y - pointB.Y;
		return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
	}

	float map(float value, float low1, float high1, float low2, float high2)
	{
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
	}

	bool getClickState(Point cursorPoint, List<List<Point>> potential_outer, double centreRadius, double unitVector_X, double unitVector_Y, double unitNormal_X, double unitNormal_Y, Mat img)
	{
		int pixelValue1 = 0, pixelValue2 = 0;
		for (int vectorMultiplier = -(int)(distanceBetween(cursorPoint, potential_outer[0][1]) * 0.8); vectorMultiplier < -(int)(distanceBetween(cursorPoint, potential_outer[0][0]) * 0.2) && pixelValue1 < 3 && pixelValue2 < 3; vectorMultiplier += 2)
		{
			for (int distanceAway = (int)(centreRadius * 1.5); distanceAway < (int)(centreRadius * 2.1) && pixelValue1 < 3 && pixelValue2 < 3; distanceAway++)
			{
				Point searchPoint1 = new Point(cursorPoint.X - croppedOffset.X + unitVector_X * vectorMultiplier + unitNormal_X * distanceAway, cursorPoint.Y - croppedOffset.Y + unitVector_Y * vectorMultiplier + unitNormal_Y * distanceAway);
				Point searchPoint2 = new Point(cursorPoint.X - croppedOffset.X + unitVector_X * vectorMultiplier - unitNormal_X * distanceAway, cursorPoint.Y - croppedOffset.Y + unitVector_Y * vectorMultiplier - unitNormal_Y * distanceAway);

				if (searchPoint1.Y >= 0 && searchPoint1.Y < imgOriginal.Rows && searchPoint1.X >= 0 && searchPoint1.X < imgOriginal.Cols)
				{
					Vec3b BGR_Vec3b = imgOriginal.At<Vec3b>(searchPoint1.Y, searchPoint1.X);
					double[] BGR = { BGR_Vec3b[0], BGR_Vec3b[1], BGR_Vec3b[2] };
					double[] HSV = BGRtoHSV(BGR);

					bool pixelIsGreen = isInRange(HSV, ref minThreshold_green, ref maxThreshold_green);

					Scalar minThreshold_bright = new Scalar(0, 0, 0);
					Scalar maxThreshold_bright = new Scalar(0, 255, 255);

					bool pixelIsBrightGreen = isInRange(HSV, ref minThreshold_bright, ref maxThreshold_bright);

					if (pixelIsGreen || pixelIsBrightGreen)
						pixelValue1++;
				}
				if (searchPoint2.Y >= 0 && searchPoint2.Y < imgOriginal.Rows && searchPoint2.X >= 0 && searchPoint2.X < imgOriginal.Cols)
				{
					Vec3b BGR_Vec3b = imgOriginal.At<Vec3b>(searchPoint2.Y, searchPoint2.X);
					double[] BGR = { BGR_Vec3b[0], BGR_Vec3b[1], BGR_Vec3b[2] };
					double[] HSV = BGRtoHSV(BGR);

					bool pixelIsGreen = isInRange(HSV, ref minThreshold_green, ref maxThreshold_green);

					Scalar minThreshold_bright = new Scalar(0, 0, 0);
					Scalar maxThreshold_bright = new Scalar(0, 255, 255);

					bool pixelIsBrightGreen = isInRange(HSV, ref minThreshold_bright, ref maxThreshold_bright);

					if (pixelIsGreen || pixelIsBrightGreen)
						pixelValue2++;
				}
			}
		}

		if (pixelValue1 >= 3 || pixelValue2 >= 3)
			return true;
		return false;
	}

	void drawClickSearchArea(Point cursorPoint, List<List<Point>> potential_outer, double centreRadius, double unitVector_X, double unitVector_Y, double unitNormal_X, double unitNormal_Y, ref Mat cameraPreview)
	{
		for (int vectorMultiplier = -(int)(distanceBetween(cursorPoint, potential_outer[0][1]) * 0.8); vectorMultiplier < -(int)(distanceBetween(cursorPoint, potential_outer[0][0]) * 0.2); vectorMultiplier += 2)
		{
			for (int distanceAway = (int)(centreRadius * 1.5); distanceAway < (int)(centreRadius * 2.1); distanceAway++)
			{
				Point searchPoint1 = new Point(cursorPoint.X + unitVector_X * vectorMultiplier + unitNormal_X * distanceAway, cursorPoint.Y + unitVector_Y * vectorMultiplier + unitNormal_Y * distanceAway);
				Point searchPoint2 = new Point(cursorPoint.X + unitVector_X * vectorMultiplier - unitNormal_X * distanceAway, cursorPoint.Y + unitVector_Y * vectorMultiplier - unitNormal_Y * distanceAway);

				if (searchPoint1.Y >= 0 && searchPoint1.Y < cameraPreview.Rows && searchPoint1.X >= 0 && searchPoint1.X < cameraPreview.Cols)
					Cv2.Circle(cameraPreview, searchPoint1, debugCircleRadius, new Scalar(0, 0, 0));
				if (searchPoint2.Y >= 0 && searchPoint2.Y < cameraPreview.Rows && searchPoint2.X >= 0 && searchPoint2.X < cameraPreview.Cols)
					Cv2.Circle(cameraPreview, searchPoint2, debugCircleRadius, new Scalar(0, 0, 0));
			}
		}
	}

	public void changeScene ()
	{
		webCamTexture.Stop();
	}

	#if (UNITY_WP_8 || UNITY_WP_8_1 || UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR && DEBUG
	void OnGUI()
	{
	if (GUI.Button(new UnityEngine.Rect(80, 70, 400, 250), "Video Feed Toggle"))
	{
	showVideoFeed = !showVideoFeed;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(580, 70, 400, 250), "Show Blue Threshold"))
	{
	showVideoFeed = true;
	showBlueThreshold = !showBlueThreshold;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(1080, 70, 400, 250), "Show Red Threshold"))
	{
	showRedThreshold = !showRedThreshold;
	showVideoFeed = true;
	showBlueThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(1580, 70, 400, 250), "Show Green Threshold"))
	{
	showGreenThreshold = !showGreenThreshold;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(2080, 70, 400, 250), "Show Yellow Threshold"))
	{
	showYellowThreshold = !showYellowThreshold;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(80, 350, 400, 250), "Mirror Toggle"))
	{
	hasMirror = !hasMirror;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(580, 350, 400, 250), "YZ Switch Toggle"))
	{
	yzSwitch = !yzSwitch;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(1080, 350, 400, 250), "HSV Retriever"))
	{
	showHSV = !showHSV;
	showDistBetweenOuterBlues = false;
	showClickROI = false;
	showCentreArea = false;
	showContours = false;
	showProximity = false;
	showAngle = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(1580, 350, 400, 250), "Show Dist Between Outer Blues"))
	{
	showDistBetweenOuterBlues = !showDistBetweenOuterBlues;
	showHSV = false;    
	showClickROI = false;
	showCentreArea = false;
	showContours = false;
	showProximity = false;
	showAngle = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(2080, 350, 400, 250), "Show Click ROI"))
	{
	showClickROI = !showClickROI;
	showHSV = false;    
	showDistBetweenOuterBlues = false;
	showCentreArea = false;
	showContours = false;
	showProximity = false;
	showAngle = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}

	if (GUI.Button(new UnityEngine.Rect(80, 630, 400, 250), "Show Area of Centre Blue"))
	{
	showCentreArea = !showCentreArea;
	showHSV = false;
	showDistBetweenOuterBlues = false;
	showClickROI = false;
	showContours = false;
	showProximity = false;
	showAngle = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(80, 1150, 400, 250), "Show Blue Contours"))
	{
	showContours = !showContours;
	showHSV = false;    
	showDistBetweenOuterBlues = false;
	showClickROI = false;
	showCentreArea = false;
	showProximity = false;
	showAngle = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(580, 1150, 400, 250), "Show Radius of Proximity"))
	{
	showProximity = !showProximity;
	showHSV = false;
	showDistBetweenOuterBlues = false;
	showClickROI = false;
	showCentreArea = false;
	showContours = false;
	showAngle = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(1580, 1150, 400, 250), "Show Angle"))
	{
	showAngle = !showAngle;
	showHSV = false;    
	showDistBetweenOuterBlues = false;
	showClickROI = false;
	showCentreArea = false;
	showContours = false;
	showProximity = false;
	showROIBetweenBlue = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	if (GUI.Button(new UnityEngine.Rect(2080, 1150, 400, 250), "Show ROI Between Blue"))
	{
	showROIBetweenBlue = !showROIBetweenBlue;
	showHSV = false;    
	showDistBetweenOuterBlues = false;
	showClickROI = false;
	showCentreArea = false;
	showContours = false;
	showProximity = false;
	showAngle = false;
	showVideoFeed = true;
	showBlueThreshold = false;
	showRedThreshold = false;
	showGreenThreshold = false;
	showYellowThreshold = false;
	}
	}
	#endif
}