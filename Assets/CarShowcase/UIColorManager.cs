using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class UIColorManager : ButtonBehaviour {


	public GameObject colOptions;

	void OnDisable() {

		colOptions.gameObject.SetActive (false);
	}

	public override void end() {

		colOptions.gameObject.SetActive (true);

	}





}
