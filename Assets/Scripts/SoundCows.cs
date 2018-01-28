using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCows : MonoBehaviour {

	public static SoundCows instance;

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
		audioSource.clip = clipList [id];
		audioSource.Play ();
	}
}
