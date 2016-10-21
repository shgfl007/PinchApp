using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class AssignColor : ButtonBehaviour {

	public Material mat;
	public Color col;

	public Image img;


	public override void end ()
	{
		BlockUI.instance.changeColor (mat);
		img.color = col;
	}

}
