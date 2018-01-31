using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeColMove : MonoBehaviour
{
    public float speed = 0.1f;
	[SerializeField] private float timer = 15; //初始
    [SerializeField] private string status;
    [SerializeField] private bool active;

	void Start () 
	{
		active = true;
	}

	void Update ()
	{
		if (active) 
		{
			timer -= speed;
			transform.position = new Vector3 (-timer, 3, 3);
			if (timer < -16f) 
			{
				Destroy (this.gameObject);
			}
		}
	}
}
