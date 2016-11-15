using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class soundManager : MonoBehaviour {

	public static soundManager instance;
	public AudioSource BGMsource;
	public AudioSource SFXsource;
	public AudioSource AmbientSource;
	public AudioSource FootStepSource;

	public AudioClip[] clips;
	public AudioClip[] BGMs;

	Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		SoundCheck();
	}

	IEnumerator Start () 
	{

		soundLibrary.Add("footstep", clips[0]);
		soundLibrary.Add("coinSound", clips[1]);
		soundLibrary.Add("buttStomp", clips[2]);
//		soundLibrary.Add("tackle", clips[3]);
//		soundLibrary.Add("footstep", clips[4]);
//		soundLibrary.Add("goalMissed", clips[5]);
//		soundLibrary.Add("goalMade", clips[6]);
//		soundLibrary.Add("goalOutOfPitch", clips[7]);
//		soundLibrary.Add("refereeWhistle", clips[8]);
//		soundLibrary.Add("coinEcho", clips[9]);
//		soundLibrary.Add("coinSound", clips[10]);
//		soundLibrary.Add("coinSplash", clips[11]);
//		soundLibrary.Add("bgmDeath", clips[12]);
//		soundLibrary.Add("bgmLive", clips[13]);
//		soundLibrary.Add("buttonPress", clips[14]);
//		soundLibrary.Add("ambienceQuiet", clips[15]);
//		soundLibrary.Add("ambienceChampions", clips[16]);
//		soundLibrary.Add("fireworks", clips[17]);
//		soundLibrary.Add("shing", clips[18]);
//		soundLibrary.Add("ambiencePremier", clips[19]);
//		soundLibrary.Add("ambienceWorldCup", clips[20]);

		yield return new WaitForSeconds(2);
//		SetBgm("bgmLive");

	}



	void SoundCheck()
	{
		if(PlayerPrefs.GetInt("soundOn",1) == 1)
		{
			SFXsource.mute = false;
		}
		else
		{
			SFXsource.mute = true;
		}

//		if(PlayerPrefs.GetInt("musicOn",1) == 1)
//		{
//			BGMsource.mute = false;
//		}
//		else
//		{
//			BGMsource.mute = true;
//		}
	}

	public void ToggleBGM()
	{
		BGMsource.mute = !BGMsource.mute;
		if(BGMsource.mute == false)
		{
			PlayerPrefs.SetInt("musicOn",1);
		}

		else
		{
			PlayerPrefs.SetInt("musicOn",0);
		}
	}

	public void ToggleSFX()
	{
		SFXsource.mute = !SFXsource.mute;
//		AmbientSource.mute = !AmbientSource.mute;
		FootStepSource.mute = !FootStepSource.mute;
		if(SFXsource.mute == false)
		{
			PlayerPrefs.SetInt("soundOn",1);
		}

		else
		{
			PlayerPrefs.SetInt("soundOn",0);
		}
	}

	public void PlayClip(string clip, float vol)
	{
		SFXsource.pitch = Random.Range(0.9f,1f);
		SFXsource.PlayOneShot(soundLibrary[clip], vol);
	}
	public void PlayStep()
	{
		FootStepSource.pitch = Random.Range(0.95f,1);
		FootStepSource.PlayOneShot(soundLibrary["footstep"], 0.2f);
	}

	public void SetBgm(string clip)
	{
		BGMsource.clip = soundLibrary[clip];
		BGMsource.Play();
	}

	public void StopBGM()
	{
		BGMsource.Stop();
	}
	public void ToggleAmbientSource()
	{
		AmbientSource.mute = !AmbientSource.mute;
	}

	public void SetAmbience(string clip)
	{
		AmbientSource.clip = soundLibrary[clip];
		AmbientSource.Play();
	}

	public void StopAmbience()
	{
		AmbientSource.Stop();
	}

	public void StartDribble()
	{
		FootStepSource.Play();
	}

	public void StopDribble()
	{
		FootStepSource.Play();
	}
}

