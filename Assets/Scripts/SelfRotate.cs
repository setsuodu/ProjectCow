using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour 
{
	public static SelfRotate instance;

	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
	}
	
	void Update () 
	{
		if (Move.instance != null && Move.instance.active) 
		{
			transform.Rotate (-Vector3.forward * 10f, Space.Self);
		}
	}
}
