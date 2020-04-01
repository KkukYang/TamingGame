 using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour {

	public delegate void Add ();
	public Add add = null;
	public delegate void End (GameObject obj);
	public End end = null;

	void EndEvent()
	{
		if(end != null)
			end(this.gameObject);
	}

	void AddEvent()
	{
		if(add != null)
			add();
	}

}
