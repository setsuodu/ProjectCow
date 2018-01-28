using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMilk : MonoBehaviour
{
	public static SoundMilk instance;

	private AudioSource audioSource;
	public AudioClip clip;

	void Awake()
	{
		instance = this;
		audioSource = GetComponent<AudioSource> ();
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void PlaySound()
	{
		audioSource.clip = clip;
		audioSource.Play();
	}


	public void StopSound()
	{
		audioSource.Stop();
	}
}
