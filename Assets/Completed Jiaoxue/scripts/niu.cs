using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class niu : MonoBehaviour {
	public static niu instance;
	private AudioSource audiosource;
	public AudioClip yes;
	public AudioClip no;
	public Sprite[] sparray;
	private SpriteRenderer sprender;
	void Awake(){
		if (instance == null) {
			instance = this;
		}
		audiosource = GetComponent<AudioSource> ();
		sprender = GetComponent<SpriteRenderer> ();
	}

	public void NMovement(float speed)
	{
		gameObject.transform.position += new Vector3(speed * Time.deltaTime,0,0);
	}

	public void AudioSucc(){
		sprender.sprite = sparray [0];
		audiosource.clip = yes;
		audiosource.Play ();
		Invoke ("reshape", 2f);
	}
	public void AudioFail(){
		sprender.sprite = sparray [1];
		audiosource.clip = no;
		audiosource.Play ();
		Invoke ("reshape", 2f);
	}
	public void jinai(){
		sprender.sprite = sparray [2];
	}
	void reshape(){
		sprender.sprite = sparray [3];

	}
}
