using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour 
{
	public static Move instance;

	public float timer = 10; //初始
	public string status;
	public bool active;

	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
		active = true;
	}
	
	void Update ()
	{
		if (active) 
		{
			timer -= MoveManager.instance.speed;
			transform.position = new Vector3 (-timer, 0, 2);
			if (timer < -15f) 
			{
				Destroy (this.gameObject);
			}
		}
	}

	public void Stop()
	{
		active = false;
	}

	public void Continue()
	{
		active = true;
	}
}
