using OpenCvSharp;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    //-------------------------------------
    // FIELD
    //-------------------------------------
    public delegate void CursorClicked(Cursor c);
    public CursorClicked cursoHold;
    public bool clicked;
    public bool usingFishEye;
    public Point blankPoint;
    public Point predictiveOffset;
    public Point previousInput;
    public int smoothingPower;
    public int predictionPower;
    public int skippedFrames;
    public float x, y;              // value that TrackerScript passes to Raycast
    public float depth, depthScale;             // value should be between 0 and 1
    public Transform handPuppet;
    public Vector3 floorHitPoint;
    public int amountOfHistory;
    public int fishEyeFocalDistance;
    public int width;
    public int height;

    // per frame basis
    public float speed;
    public float horizontalSpeed;
    public float verticalSpeed;

    public List<Point> previousData;
    public List<bool> previousClickStates;
    public List<Point> previousCoordinates;

    //------------------------------
    // Moved from HeldObjectInfo
    //-------------------------------
    public bool click = false;
    public bool hold = false; // Switches turn on when an object is hit, and turn off when cursor is no longer clicking
    public Vector3 previousCursorPos;
    public Vector3 currentCursorPos;
    public bool objectHitByRay = false;
    public Transform hoveringTransform;
    public Ray ray;
    public bool doRayCast = true;
    public int interactionMode = -1;
    public int previousInteractionMode = -1;
    public bool previousClickState = false;
    public Transform heldTransform;
    public float distMoved = 0;
    public RaycastHit hit;
    public Transform previousHeldTransform;
    public Transform previousHoveringTransform;
    public delegate void RayCastHitObj(GameObject obj, Cursor c);
    public RayCastHitObj rayHitObj;
    public delegate void CursorDeselected();
    public CursorDeselected Deselected;
    public delegate void CursorDeselectedHold(Quaternion angle);
    public CursorDeselectedHold OnCursorDeselectedHold;
    public bool canCallRelease;
    //-------------------------------------
    // NOT FIELD
    //-------------------------------------

    public GameObject GetCursorObject()
    {
        return gameObject;
    }

    public Cursor Init(int w, int h, bool fishEye)
    {
        clicked = false;
        skippedFrames = 0; // Amount of frames since last finding the cursor
        amountOfHistory = 5; // Tunable
        smoothingPower = 8;
        predictionPower = 5;
        blankPoint = new Point();
        blankPoint.X = 0;
        blankPoint.Y = 0;
        fishEyeFocalDistance = 1000;
        width = w;
        height = h;
        usingFishEye = fishEye;
        predictiveOffset = new Point();
        predictiveOffset.X = 0;
        predictiveOffset.Y = 0;

        previousData = new List<Point>();
        previousClickStates = new List<bool>();
        previousCoordinates = new List<Point>();
        previousInput = new Point(0, 0);

        speed = 0;
        horizontalSpeed = 0;
        verticalSpeed = 0;

        reset();
        return this;
    }

    public void setFocalDistance(int fD)
    {
        fishEyeFocalDistance = fD;
    }

    public void setFishEyeMode(bool mode)
    {
        usingFishEye = mode;
    }

    public void setHistoryAmount(int amount)
    {
        amountOfHistory = amount;
    }

    public bool getClickState()
    {
        return clicked;
    }

    public Point getPredictiveOffset()
    {
        return predictiveOffset;
    }

    public Point getPrediction()
    {
        Point prediction = new Point(previousCoordinates[0].X, previousCoordinates[0].Y);
        prediction.X += predictiveOffset.X;
        prediction.Y += predictiveOffset.Y;

        return prediction;
    }

    public bool cursorFound()
    {
        if (skippedFrames < 9)
            return true;
        else
            return false;
    }

    public Point getPosition()
    {
        if (previousCoordinates.Count > 0)
        {
            if (usingFishEye)
                return fisheyeCorrection(width, height, previousCoordinates[0], fishEyeFocalDistance);
            else
                return previousCoordinates[0];
        }
        else
            return blankPoint;
    }

    public Point getPreviousPosition()
    {
        if (previousCoordinates.Count > 1)
        {
            if (usingFishEye)
                return fisheyeCorrection(width, height, previousCoordinates[1], fishEyeFocalDistance);
            else
                return previousCoordinates[1];
        }
        else
            return blankPoint;
    }

    public void reset()
    {
        // Fills lists with initial values
        for (int i = 0; i < amountOfHistory; i++)
        {
            addClickState(false);
            addCoordinate(blankPoint, false);
        }
    }

    public void addClickState(bool state)
    {
        // Insert the raw data
        previousClickStates.Insert(0, state); // Uses an iterator to insert an item at the beginning
        while (previousClickStates.Count > 3)
        { // Ensures that there are only <smoothingPower> items in the list
            previousClickStates.RemoveAt(previousClickStates.Count - 1);
        }

        int counterTrue = 0, counterFalse = 0;
        for (int i = 0; i < previousClickStates.Count; i++)
        {
            if (previousClickStates[i])
            {
                counterTrue++;
            }
            else {
                counterFalse++;
            }
        }

        if (counterTrue >= 2)
        {
            clicked = true;
        }
        if (counterFalse >= 2)
        {
            clicked = false;
        }
    }

    public void addCoordinate(Point coord, bool predictive)
    {
        // Adds new point
        previousData.Insert(0, coord); // Uses an iterator to insert an item at the beginning
        while (previousData.Count > amountOfHistory)
        { // Ensures that there are only <amountOfHistory> items in the list
            previousData.RemoveAt(previousData.Count - 1);
        }

        previousInput.X = coord.X;
        previousInput.Y = coord.Y;

        // Applies predictive tracking
        if (predictive && false) // Turn off predictive tracking
        {
            Point offset = findPredictiveOffset(0.2);
            previousData.Insert(0, new Point(previousData[0].X + offset.X, previousData[0].Y + offset.Y));
            previousData.RemoveAt(1);

            predictiveOffset.X = offset.X;
            predictiveOffset.Y = offset.Y;
        }

        // Applies smoothing
        previousCoordinates.Insert(0, jitterReduction());
        while (previousCoordinates.Count > smoothingPower)
        { // Ensures that there are only <amountOfHistory> items in the list
            previousCoordinates.RemoveAt(previousCoordinates.Count - 1);
        }
    }

    public Point jitterReduction()
    {
        int sumX = 0, sumY = 0, count = 0;

        // Averages the last few coordinates 
        for (int i = 0; i < previousData.Count && i < smoothingPower; i++)
        {
            sumX += (int)previousData[i].X;
            sumY += (int)previousData[i].Y;
            count++;
        }

        if (count == 0)
        {
            count = 1;
        }

        return new Point(sumX / count, sumY / count);
    }

    public Point findPredictiveOffset(double power)
    {
        float dy = 0, dx = 0, count = 0;

        if (previousData.Count > 2)
        {
            for (int i = 1; i < previousData.Count - 1 && i < predictionPower; i++)
            {
                dx += (float)(previousData[i].X - previousData[i + 1].X);
                dy += (float)(previousData[i].Y - previousData[i + 1].Y);
                if (previousData[i].X < 0 || previousData[i].Y < 0 || previousData[i + 1].X < 0 || previousData[i + 1].Y < 0)
                {
                    return blankPoint;
                }
                count++;
            }

            if (count == 0)
            {
                count = 1;
            }

            // Get velocity
            horizontalSpeed = Mathf.Abs(dx / count);
            verticalSpeed = Mathf.Abs(dy / count);
            speed = Mathf.Pow(horizontalSpeed * horizontalSpeed + verticalSpeed * verticalSpeed, 0.5f);

            if (dx > 75 || dy > 75)
            {
                return new Point((double)(dx * .1 / count), (double)(dy * .1 / count));
            }
            else {
                return new Point((double)(dx * power / count), (double)(dy * power / count));
            }
        }
        else
            return blankPoint;
    }

    public Point fisheyeCorrection(int width, int height, Point point, int fD)
    {
        double nX = point.X - (width / 2);
        double nY = point.Y - (height / 2);
        double xS = nX / Mathf.Abs((float)nX);
        double yS = nY / Mathf.Abs((float)nY);
        nX = Mathf.Abs((float)nX);
        nY = Mathf.Abs((float)nY);

        if (fD == 0)
        {
            fD = 1;
        }

        double realDistX = fD * Mathf.Tan(2 * Mathf.Asin((float)(nX / fD)));
        double realDistY = fD * Mathf.Tan(2 * Mathf.Asin((float)(nY / fD)));

        realDistX = xS * realDistX + (width / 2);
        realDistY = yS * realDistY + (height / 2);

        if (point.X != width * 0.5)
        {
            point.X = (int)realDistX;
        }
        if (point.Y != height * 0.5)
        {
            point.Y = (int)realDistY;
        }

        return point;
    }

    public void CursorHold()
    {
        if (cursoHold != null)
        {
            cursoHold(this);
        }
    }
}