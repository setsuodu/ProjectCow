using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	private MeshRenderer render;
	private Vector2 offset = Vector2.zero;
	public float speed = 1.0f;

	void Awake () 
	{
		render = GetComponent<MeshRenderer> ();
	}
	
	void Update () 
	{
		offset -= new Vector2 (Time.deltaTime * speed, 0);
		render.material.SetTextureOffset ("_MainTex", offset);
	}
}
