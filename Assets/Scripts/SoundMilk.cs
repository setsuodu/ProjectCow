using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMilk : MonoBehaviour
{
	public static SoundMilk instance;

	public AudioSource audioSource;
	[SerializeField] private AudioClip[] clips;

    void Awake()
	{
		instance = this;
		audioSource = GetComponent<AudioSource> ();
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void PlaySound(int times)
	{
        audioSource.clip = clips[times - 1];
        audioSource.Play();
	}

	public void StopSound()
	{
		audioSource.Stop();
	}
}
