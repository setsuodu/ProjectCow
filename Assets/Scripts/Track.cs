using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	public static Track instance;

	private MeshRenderer render;
	private Vector2 offset = Vector2.zero;
	public float speed = 1.0f;
	public bool active;

	void Awake () 
	{
		instance = this;
		render = GetComponent<MeshRenderer> ();
	}

	void Start()
	{
		active = true;
	}
	
	void Update () 
	{
		if (active) 
		{
			offset -= new Vector2 (Time.deltaTime * speed, 0);
			render.material.SetTextureOffset ("_MainTex", offset);
		}
	}
}
