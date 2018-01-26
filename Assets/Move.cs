using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour 
{
	public static Move instance;

	public float timer = 10;
	public string status;

	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
		
	}
	
	void Update ()
	{
		timer -= MoveManager.instance.speed;
		transform.position = new Vector3 (timer, 0, 0);
		if (timer < -20f) 
		{
			Destroy (this.gameObject);
		}
	}
}
