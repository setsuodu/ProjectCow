using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : MonoBehaviour {
	public static hand instance;
	public AudioClip bilin;
	private AudioSource audioSource;
	public Sprite[] sparray;
	private SpriteRenderer render;

	// Use this for initialization
	void Awake(){
		if (instance == null) {
			instance = this;
		}
		render = GetComponent<SpriteRenderer> ();
		audioSource = GetComponent<AudioSource> ();

	}

	public void empty () {
		render.sprite = sparray [0];
	}
	
	// Update is called once per frame
	public void milk () {
		render.sprite = sparray [1];
	}
	public void succ(){
		audioSource.clip = bilin;
		audioSource.Play ();
	}
}
