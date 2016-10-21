using UnityEngine;
using System.Collections;

public class Tweener : MonoBehaviour {
	
	public void tween(GameObject obj, string opt, Vector3 value, float duration, float delay, Ease ease_opt, bool keepPrevious) {
		
		UnityTween v_tween;
		
		if (obj.GetComponents<UnityTween>().Length > 2 && !keepPrevious)
		{
			for (int i = 0; i < obj.GetComponents<UnityTween>().Length-1; i++)
			{
				if (obj.GetComponents<UnityTween>()[i].rotations.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
				if (obj.GetComponents<UnityTween>()[i].positions.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
				if (obj.GetComponents<UnityTween>()[i].localPositions.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
				if (obj.GetComponents<UnityTween>()[i].alphas.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
			}
			
		};
		
		v_tween = obj.AddComponent<UnityTween>();
		
		if (opt == "rotation"){
			v_tween.rotations = new TweenRotationObject[1];
			TweenRotationObject[] valor_tween = new TweenRotationObject[1];
			valor_tween[0] = new TweenRotationObject();
			valor_tween[0].delay = delay;
			valor_tween[0].totalTime = duration;
			valor_tween[0].ease = ease_opt;
			valor_tween[0].tweenValue = value;
			v_tween.rotations[0] = valor_tween[0];
		}
		
		if (opt == "position") {
			v_tween.positions = new TweenPositionObject[1];
			TweenPositionObject[] valor_tween = new TweenPositionObject[1];
			valor_tween[0] = new TweenPositionObject();
			valor_tween[0].delay = delay;
			valor_tween[0].totalTime = duration;
			valor_tween[0].ease = ease_opt;
			valor_tween[0].tweenValue = value;
			v_tween.positions[0] = valor_tween[0];        
		}

		if (opt == "local position") {
			v_tween.localPositions = new TweenLocalPositionObject[1];
			TweenLocalPositionObject[] valor_tween = new TweenLocalPositionObject[1];
			valor_tween[0] = new TweenLocalPositionObject();
			valor_tween[0].delay = delay;
			valor_tween[0].totalTime = duration;
			valor_tween[0].ease = ease_opt;
			valor_tween[0].tweenValue = value;
			v_tween.localPositions[0] = valor_tween[0];        
		}
	}

	// For alpha (special case because of float input)
	public void tween(GameObject obj, string opt, float value, float duration, float delay, Ease ease_opt, bool keepPrevious) {
		
		UnityTween v_tween;
		
		if (obj.GetComponents<UnityTween>().Length > 2 && !keepPrevious)
		{
			for (int i = 0; i < obj.GetComponents<UnityTween>().Length-1; i++)
			{
				if (obj.GetComponents<UnityTween>()[i].rotations.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
				if (obj.GetComponents<UnityTween>()[i].positions.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
				if (obj.GetComponents<UnityTween>()[i].localPositions.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
				if (obj.GetComponents<UnityTween>()[i].alphas.Length > 0)
				{
					Destroy(obj.GetComponents<UnityTween>()[i]);
				}
			}
			
		};
		
		v_tween = obj.AddComponent<UnityTween>();

		if (opt == "alpha") {
			v_tween.alphas = new TweenAlphaObject[1];
			TweenAlphaObject[] valor_tween = new TweenAlphaObject[1];
			valor_tween[0] = new TweenAlphaObject();
			valor_tween[0].delay = delay;
			valor_tween[0].totalTime = duration;
			valor_tween[0].ease = ease_opt;
			valor_tween[0].tweenValue = value;
			v_tween.alphas[0] = valor_tween[0];        
		}
	}

}