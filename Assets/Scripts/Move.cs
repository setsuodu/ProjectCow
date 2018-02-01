using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour 
{
	public static Move instance;

    public float speed = 0.1f;
    public string status;
    [SerializeField] private float timer = 15; //初始
    [SerializeField] private bool active;
	[SerializeField] private Sprite[] spArray;
    private SpriteRenderer render;
    public float holdTime = 1.8f; //需要挤奶时长
    public int bSuccess; //伸桶成功
    public int mCount;   //尝试挤奶
    public int mSuccess; //挤奶成功

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
				AddScore (); //销毁前计分
				Destroy (this.gameObject);
			}
		}
	}

	public void Stop()
	{
		active = false;
        Tracks.instance.active = false; //履带、滚轮停
    }

    public void Continue()
	{
		active = true;
        Tracks.instance.active = true; //履带、滚轮走
    }

    //换表情
    public void ChangeExpression(int value)
	{
		render.sprite = spArray[value];
	}

	void AddScore()
	{
        if(MoveManager.instance != null)
        {
            MoveManager.instance.current = null;
            MoveManager.instance.cowCount += 1;
            MoveManager.instance.bSuccess += bSuccess;

            MoveManager.instance.mCount += mCount;

            MoveManager.instance.mSuccess += mSuccess;
            MoveManager.instance.Score();
        }
    }
}
