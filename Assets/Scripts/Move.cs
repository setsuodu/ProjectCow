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
	public int bucket;
	public int milk;

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
				AddScore ();
				Destroy (this.gameObject);
			}
		}
	}

	public void Stop()
	{
		active = false;
        //Track.instance.active = false; //履带、滚轮停
        Tracks.instance.active = false; //履带、滚轮停
    }

    public void Continue()
	{
		active = true;
        //Track.instance.active = true; //履带、滚轮走
        Tracks.instance.active = true; //履带、滚轮走
    }

    public void ChangeStatus(int value)
	{
		render.sprite = spArray[value];
	}

	void AddScore()
	{
		MoveManager.instance.bucketSuccess += bucket;
		MoveManager.instance.milkSuccess += milk;
		MoveManager.instance.Score();
	}
}
