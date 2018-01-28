using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeColMove : MonoBehaviour
{
	public float timer = 15; //初始
	public string status;
	public bool active;

	void Start () 
	{
		active = true;
	}

	void Update ()
	{
		if (active) 
		{
			timer -= MoveManager.instance.speed;
			transform.position = new Vector3 (-timer, 3, 3);
			if (timer < -16f) 
			{
				Destroy (this.gameObject);
			}
		}
	}
}
