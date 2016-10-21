using UnityEngine;
using System.Collections;

public class ColorChanger : ButtonBehaviour {

	public Material mat;



	public override void end() {
		if (RotateSelf.instance.inCar == false) {
			changeMaterials.instance.assignMat (mat);
			changeMaterials.instance.changeMat ();
		
		}
		CarUIManage.instance.turnOffUI ();


	}



}
