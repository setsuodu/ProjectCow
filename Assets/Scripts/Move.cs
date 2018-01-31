using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour 
{
	public static Move instance;

    public float speed = 0.1f;
    [SerializeField] private float timer = 15; //初始
    public string status;
    [SerializeField] private bool active;
	[SerializeField] private Sprite[] spArray;
    private SpriteRenderer render;
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
            timer -= speed;
			transform.position = new Vector3 (-timer, 0, 2);
			if (timer < -16f)
			{
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

    //换表情
    public void ChangeStatus(int value)
	{
		render.sprite = spArray[value];
	}

	void AddScore()
	{
        if(MoveManager.instance != null)
        {
            MoveManager.instance.current = null;
            MoveManager.instance.bucketSuccess += bucket;
            MoveManager.instance.milkSuccess += milk;
            MoveManager.instance.Score();
        }
	}
}
