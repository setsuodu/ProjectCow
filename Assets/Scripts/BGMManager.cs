using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour 
{
	public static BGMManager instance;

	private AudioSource audioSource;
	public AudioClip startClip;
	public AudioClip loopClip;
	public AudioClip endClip;

	void Awake()
	{
		instance = this;
		audioSource = GetComponent<AudioSource> ();
	}

	void Start () 
	{
		if (!audioSource.isPlaying) 
		{
			audioSource.clip = startClip;
			audioSource.Play ();
		}
	}
	
	void Update () 
	{
		if (!audioSource.isPlaying && MoveManager.instance.isPlaying) 
		{
			//Debug.Log ("播完StartClip");

			audioSource.clip = loopClip;
			audioSource.Play ();
		}
	}

	public void EndMusic()
	{
		audioSource.Stop ();
		//audioSource.clip = endClip;
		//audioSource.Play ();
	}
}
