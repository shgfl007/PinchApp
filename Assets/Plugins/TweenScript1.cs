using UnityEngine;
using System.Collections;

//Tween position object class
[System.Serializable]
public class TweenPositionObject : BaseTweenObject
{
	public Vector3 tweenValue;
	
	private Vector3 _startValue;
	public Vector3 startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
	
	public TweenPositionObject ()
	{
		this.tweenType = TweenType.TweenPosition;
	}	
}

//Tween local position object class
[System.Serializable]
public class TweenLocalPositionObject : BaseTweenObject
{
	public Vector3 tweenValue;
	
	private Vector3 _startValue;
	public Vector3 startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
	
	public TweenLocalPositionObject ()
	{
		this.tweenType = TweenType.TweenLocalPosition;
	}	
}

//Tween rotation object class
[System.Serializable]
public class TweenRotationObject : BaseTweenObject
{
	public Vector3 tweenValue;
	
	private Vector3 _startValue;
	public Vector3 startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
	
	public TweenRotationObject ()
	{
		this.tweenType = TweenType.TweenRotation;
	}	
}

//Tween rotation object class
[System.Serializable]
public class TweenAlphaObject : BaseTweenObject
{
	public float tweenValue;
	
	private float _startValue;
	public float startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
	
	public TweenAlphaObject ()
	{
		this.tweenType = TweenType.TweenAlpha;
	}	
}



//Engine class
public class UnityTween : MonoBehaviour {
	
	
	public TweenPositionObject[] positions =  new TweenPositionObject[0];
	public TweenLocalPositionObject[] localPositions =  new TweenLocalPositionObject[0];
	public TweenRotationObject[] rotations =  new TweenRotationObject[0];
	public TweenAlphaObject[] alphas =  new TweenAlphaObject[0];
	
	public bool loopArray;
	
	private ArrayList tweens;
	
	
	void Start () {
		
		this.tweens = new ArrayList();
		this.AddTweens();
	}
	
	private void AddTweens ()
	{
		foreach(TweenPositionObject tween in positions)
		{
			TweenPosition(tween);
		}
		foreach(TweenLocalPositionObject tween in localPositions)
		{
			TweenLocalPosition(tween);
		}
		foreach(TweenRotationObject tween in rotations)
		{
			TweenRotation(tween);
		}
		foreach(TweenAlphaObject tween in alphas)
		{
			TweenAlpha(tween);
		}
	}
	//Add Tween in arrayList
	//Tween position
	public void TweenPosition (TweenPositionObject obj)
	{
		TweenPositionObject tween = new TweenPositionObject();
		
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
		
		this.tweens.Add(tween);
	}
	//Tween local position
	public void TweenLocalPosition (TweenLocalPositionObject obj)
	{
		TweenLocalPositionObject tween = new TweenLocalPositionObject();
		
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
		
		this.tweens.Add(tween);
	}
	//Tween rotation
	public void TweenRotation (TweenRotationObject obj)
	{
		TweenRotationObject tween = new TweenRotationObject();
		
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
		
		this.tweens.Add(tween);
	}
	//Tween alpha
	public void TweenAlpha (TweenAlphaObject obj)
	{
		TweenAlphaObject tween = new TweenAlphaObject();
		
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
		
		this.tweens.Add(tween);
	}
	
	//Clear Tweens with the same type
	private void ClearTweensSameType (BaseTweenObject obj)
	{
		foreach (BaseTweenObject tween in tweens)
		{
			if(tween.id != obj.id && tween.tweenType == obj.tweenType)
				tween.ended = true;
		}
	}
	
	//Updates
	void Update ()
	{
		this.DetectDelay();
		this.UpdateTween();
	}
	//Detect when delay was passed
	private void DetectDelay ()
	{
		foreach (BaseTweenObject tween in tweens)
		{
			if(Time.time > tween.startTime + tween.delay && !tween.canStart)
			{
				if(tween.tweenType == TweenType.TweenPosition)
				{
					TweenPositionObject tweenPos = tween as TweenPositionObject;
					tweenPos.startValue = this.transform.position; // POSITION
				}
				if(tween.tweenType == TweenType.TweenLocalPosition)
				{
					TweenLocalPositionObject tweenPos = tween as TweenLocalPositionObject;
					tweenPos.startValue = this.transform.localPosition; // LOCAL POSITION
				}
				else if(tween.tweenType == TweenType.TweenRotation)
				{
					TweenRotationObject tweenRot = tween as TweenRotationObject;
					tweenRot.startValue = this.transform.rotation.eulerAngles;
				}
				else if(tween.tweenType == TweenType.TweenAlpha)
				{
					TweenAlphaObject tweenAlpha = tween as TweenAlphaObject;
					if(GetComponent<GUITexture>() != null)
						tweenAlpha.startValue = GetComponent<GUITexture>().color.a;
					else if (GetComponent<CanvasGroup>() != null)
						tweenAlpha.startValue = gameObject.GetComponent<CanvasGroup>().alpha;
					else if (this.GetComponent<Renderer>() != null)
						tweenAlpha.startValue = this.GetComponent<Renderer>().material.color.a;
				}
				
				this.ClearTweensSameType(tween);
				
				tween.canStart = true;
			}
		}
	}
	//Update tween by type
	private void UpdateTween ()
	{
		
		int tweenCompleted = 0;
		foreach (BaseTweenObject tween in tweens)
		{
			if(tween.canStart && !tween.ended)
			{
				if(tween.tweenType == TweenType.TweenPosition)
					UpdatePosition(tween as TweenPositionObject);
				else if(tween.tweenType == TweenType.TweenLocalPosition)
					UpdateLocalPosition(tween as TweenLocalPositionObject);
				else if(tween.tweenType == TweenType.TweenRotation)
					UpdateRotation(tween as TweenRotationObject);
				else if(tween.tweenType == TweenType.TweenAlpha)
					UpdateAlpha(tween as TweenAlphaObject);	
				
			}
			if(tween.ended)
				tweenCompleted++;
			
			if(tweenCompleted == tweens.Count && loopArray)
				this.MakeLoop ();
			
			
		}
	}
	
	private void MakeLoop ()
	{
		foreach (BaseTweenObject tween in tweens)
		{
			tween.ended = false;
			tween.canStart = false;
			tween.startTime = Time.time;			
		}
	}
	
	
	
	//Update Position
	private void UpdatePosition(TweenPositionObject tween)
	{
		Vector3 begin = tween.startValue;
		Vector3 finish =  tween.tweenValue; 
		Vector3 change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
		
		if(duration == 0)
		{
			this.EndTween(tween);
			this.transform.position = finish; // POSITION
			return;
		}
		
		
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
		
		this.transform.position = Equations.ChangeVector(currentTime, begin, change ,duration, tween.ease); // POSITION
	}
	//Update Local Position
	private void UpdateLocalPosition(TweenLocalPositionObject tween)
	{
		Vector3 begin = tween.startValue;
		Vector3 finish =  tween.tweenValue; 
		Vector3 change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
		
		if(duration == 0)
		{
			this.EndTween(tween);
			this.transform.localPosition = finish; // POSITION
			return;
		}
		
		
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
		
		this.transform.localPosition = Equations.ChangeVector(currentTime, begin, change ,duration, tween.ease); // POSITION
	}
	//Update Rotation
	private void UpdateRotation(TweenRotationObject tween)
	{
		Vector3 begin = tween.startValue;
		Vector3 finish =  tween.tweenValue; 
		Vector3 change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
		
		if(duration == 0)
		{
			this.EndTween(tween);
			this.transform.rotation = Quaternion.Euler (finish); // POSITION
			return;
		}
		
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
		
		this.transform.rotation = Quaternion.Euler(Equations.ChangeVector(currentTime, begin, change ,duration, tween.ease));
	}
	//Update Alpha
	private void UpdateAlpha(TweenAlphaObject tween)
	{
		float begin = tween.startValue;
		float finish =  tween.tweenValue; 
		float change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
		
		float alpha = Equations.ChangeFloat(currentTime, begin, change ,duration, tween.ease);
		float redColor;
		float redGreen;
		float redBlue;
		
		if (GetComponent<GUITexture> () != null) {
			redColor = GetComponent<GUITexture> ().color.r;
			redGreen = GetComponent<GUITexture> ().color.g;
			redBlue = GetComponent<GUITexture> ().color.b;

			GetComponent<GUITexture> ().color = new Color (redColor, redGreen, redBlue, alpha);

			if (duration == 0) {
					this.EndTween (tween);
					GetComponent<GUITexture> ().color = new Color (redColor, redGreen, redBlue, finish);
					return;
			}
		} 
		else if (GetComponent<CanvasGroup> () != null) {
			if (alpha < 0)
				alpha = 0;
			if (alpha > 1)
				alpha  = 1;
			GetComponent<CanvasGroup> ().alpha = alpha;

			if (duration == 0) {
				this.EndTween (tween);
				GetComponent<CanvasGroup> ().alpha = finish;
				return;
			}
		}
		else if (this.GetComponent<Renderer>() != null)
		{
			redColor = this.GetComponent<Renderer>().material.color.r;
			redGreen = this.GetComponent<Renderer>().material.color.g;
			redBlue = this.GetComponent<Renderer>().material.color.b;

			this.GetComponent<Renderer>().material.color = new Color(redColor,redGreen,redBlue,alpha);
			
			if(duration == 0)
			{
				this.EndTween(tween);
				this.GetComponent<Renderer>().material.color = new Color(redColor,redGreen,redBlue,finish);
				return;
			}
		}
		
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
	}	
	
	private void EndTween (BaseTweenObject tween)
	{
		tween.ended = true;
	}
}