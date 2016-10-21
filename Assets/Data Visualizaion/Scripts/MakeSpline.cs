using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Vectrosity;

public class MakeSpline : MonoBehaviour {


		int segments= 250;

		void  Start (){
		List<Vector3> splinePoints= new List<Vector3>();
			int i= 1;
			GameObject obj= GameObject.Find ("Sphere"+(i++));
			while (obj != null) {
				splinePoints.Add (obj.transform.position);
				obj = GameObject.Find ("Sphere"+(i++));
			}

				VectorLine spline= new VectorLine("Spline", new List<Vector3>(segments+1), null, 2.0f, LineType.Continuous);
				spline.MakeSpline (splinePoints.ToArray(), segments, false);
				spline.Draw3D();
			
		}	

}
