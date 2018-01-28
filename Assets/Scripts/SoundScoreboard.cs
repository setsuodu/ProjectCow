using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScoreboard : MonoBehaviour {

	public static SoundScoreboard instance;

	public AudioSource audioSource;
	public List<AudioClip> clipList;

	void Awake()
	{
		instance = this;
	}

	void Start () 
	{

	}

	public void PlayClip(int id)
	{
		Debug.Log ("结果" + id);
		audioSource.clip = clipList [id];
		audioSource.Play ();
	}
}
