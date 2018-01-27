using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour 
{
	public static Move instance;

	public float timer = 15; //初始
	public string status;
	public bool active;
	private SpriteRenderer render;
	[SerializeField] private Sprite[] spArray;

	void Awake()
	{
		instance = this;
		render = GetComponent<SpriteRenderer> ();
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
			if (timer < -16f) 
			{
				MoveManager.instance.current = null;
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

	public void ChangeStatus(int value)
	{
		render.sprite = spArray[value];
	}
}
