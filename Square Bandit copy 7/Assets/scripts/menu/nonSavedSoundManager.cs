using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonSavedSoundManager : MonoBehaviour {

	AudioSource SFXsource;
	public AudioClip click;
	void Start () {

		SFXsource = GetComponent<AudioSource>();
	}
	
	public void PlayClick()
	{
		//		SFXsource.pitch = Random.Range(0.95f,1f);
		SFXsource.PlayOneShot(click, 0.5f);
	}
}
