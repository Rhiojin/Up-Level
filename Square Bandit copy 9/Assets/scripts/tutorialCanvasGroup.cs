using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialCanvasGroup : MonoBehaviour {

	CanvasGroup thisGroup;
	void Start () 
	{
		thisGroup = GetComponent<CanvasGroup>();
	}


	public void StartFadeIn()
	{
		InvokeRepeating("Fade",0.1f,0.1f);
	}

	void Fade()
	{
		if(thisGroup.alpha < 1)
		{
			thisGroup.alpha +=Time.deltaTime*4;
			if(thisGroup.alpha >= 1) 
			{
				CancelInvoke("Fade");
			}
		}
	}
}
