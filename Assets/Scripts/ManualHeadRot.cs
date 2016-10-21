using UnityEngine;
using System.Collections;

public class ManualHeadRot : MonoBehaviour {

	private static ManualHeadRot mInstance;
	public static ManualHeadRot instance { get { return mInstance; } }

	//used to set a game-object, such as a parent, as a point reference for resetting view
	public Transform target;
	public Transform target2;

	private Quaternion prevRotation;
	Vector3 prevRotationEuler;

	bool isMoving = false;
	void Awake () {

		if (mInstance == null)
		{
			mInstance = this;
		}
		//set your first rotation as starting head rotation
		//prevRotation = Cardboard.SDK.HeadPose.Orientation;

	}

	public void reCenterCardboard () {

		transform.rotation = target.rotation;

	}
	public void zeroCardboard () {

		transform.localEulerAngles = new Vector3 (0,0,0);
	}

	public void secondaryReCenter() {
		transform.rotation = target2.rotation;


	}


	public void translateView() {
		isMoving = true;

	}

	void Update () {

		if (isMoving) {


			Quaternion fromRot = transform.rotation;
			if (Quaternion.Angle(fromRot, target2.rotation) > 2 ) {
				fromRot = Quaternion.Lerp (fromRot, target2.rotation, Time.deltaTime / 0.8f);
				transform.rotation = fromRot;
			} else {
				isMoving = false;
			}
		}

		if (!isMoving) {


				Quaternion cHeadRot = Cardboard.SDK.HeadPose.Orientation;
				Vector3 cHeadRotEuler = cHeadRot.eulerAngles;
				Quaternion rot = Quaternion.Euler (cHeadRotEuler - prevRotationEuler);
				Quaternion temp = transform.rotation;
				temp *= rot;
				Vector3 angies = temp.eulerAngles;
				transform.rotation = Quaternion.Euler (new Vector3 (angies.x, angies.y, 0));

			prevRotation = Cardboard.SDK.HeadRotation;
			prevRotationEuler = prevRotation.eulerAngles;


		}
	}
}
