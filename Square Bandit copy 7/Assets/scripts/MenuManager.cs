using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	public CanvasGroup currentCanvasGroup;
	public CanvasGroup mainMenuGroup;
	public CanvasGroup characterSelectMenuGroup;
	public CanvasGroup InGameGroup;
	public CanvasGroup gameOverGroup;

	public GameObject mainMenuPanel;
	public GameObject gameOverPanel;

	float fadeTime = 2;
	public pcControl pcScript;
	public bool gameStarted = false;

	public SpriteRenderer pcBody;

	void Start () 
	{
		transistionCanvas.instance.StartTransitionOut();
		currentCanvasGroup = mainMenuGroup;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetSkin(string name)
	{
		pcScript.UpdateSkin(name);
	}

	void FadeOut()
	{
		if(mainMenuGroup.alpha > 0)
		{
			mainMenuGroup.alpha -=Time.deltaTime*fadeTime;
			if(mainMenuGroup.alpha <= 0) 
			{
				mainMenuPanel.SetActive(false);
				CancelInvoke("FadeOut");
			}
		}
	}

	public IEnumerator FadeDelay()
	{
		yield return new WaitForSeconds(0.5f);
		InvokeRepeating("InGameUIFadeOut",0.01f, 0.02f);
		InvokeRepeating("GameOverUIFadeIn",0.01f, 0.02f);

	}

	void GameOverUIFadeIn()
	{
		if(gameOverGroup.interactable == false)gameOverGroup.interactable = true;
		if(gameOverGroup.alpha < 1)
		{
			gameOverGroup.alpha +=Time.deltaTime*fadeTime;
			if(gameOverGroup.alpha >= 1) 
			{

				gameOverGroup.blocksRaycasts = true;
				CancelInvoke("GameOverUIFadeIn");
			}
		}
	}

	void InGameUIFadeOut()
	{
		if(InGameGroup.alpha > 0)
		{
			InGameGroup.alpha -=Time.deltaTime*fadeTime;
			if(InGameGroup.alpha <= 0) 
			{
				InGameGroup.interactable = false;
				InGameGroup.blocksRaycasts = false;
				CancelInvoke("InGameUIFadeOut");
			}
		}
	}

	void InGameUIFadeIn()
	{
		if(InGameGroup.alpha < 1)
		{
			InGameGroup.alpha +=Time.deltaTime*fadeTime;
			if(InGameGroup.alpha >= 1) 
			{
				InGameGroup.interactable = true;
				InGameGroup.blocksRaycasts = true;
				CancelInvoke("InGameUIFadeIn");
			}
		}
	}

	public void StartFadeTransition(CanvasGroup canvasToFadeIn)
	{
		StartCoroutine( PanelToPanelFade(canvasToFadeIn) );
	}

	IEnumerator PanelToPanelFade(CanvasGroup canvasToFadeIn)
	{
		while(canvasToFadeIn.alpha < 1 && currentCanvasGroup.alpha > 0) 
		{
			if(currentCanvasGroup.alpha > 0)
			{
				currentCanvasGroup.alpha -=Time.deltaTime*fadeTime;
				if(currentCanvasGroup.alpha <= 0) 
				{
					currentCanvasGroup.interactable = false;
					currentCanvasGroup.blocksRaycasts = false;

				}
			}

			if(canvasToFadeIn.alpha < 1)
			{
				canvasToFadeIn.alpha +=Time.deltaTime*fadeTime;
				if(canvasToFadeIn.alpha >= 1) 
				{
					canvasToFadeIn.interactable = true;
					canvasToFadeIn.blocksRaycasts = true;

				}
			}
			yield return null;
		}
		currentCanvasGroup = canvasToFadeIn;
		CancelInvoke("PanelToPanelFade");
	}

//	void PanelToPanelFade(CanvasGroup canvasAFadeOut, CanvasGroup canvasBFadeIn)
//	{
//		if(canvasAFadeOut.alpha > 0)
//		{
//			canvasAFadeOut.alpha -=Time.deltaTime*fadeTime;
//			if(canvasAFadeOut.alpha <= 0) 
//			{
//				canvasAFadeOut.interactable = false;
//				canvasAFadeOut.blocksRaycasts = false;
//
//			}
//		}
//
//		if(canvasBFadeIn.alpha < 1)
//		{
//			canvasBFadeIn.alpha +=Time.deltaTime*fadeTime;
//			if(canvasBFadeIn.alpha >= 1) 
//			{
//				canvasBFadeIn.interactable = true;
//				canvasBFadeIn.blocksRaycasts = true;
//
//			}
//		}
//		if(canvasBFadeIn.alpha >= 1 && canvasAFadeOut.alpha <= 0) 
//			CancelInvoke("PanelToPanelFade");
//	}

	public void Play()
	{
		if(gameStarted)
		{
			StartOver();
		}
		else
		{
			NewStart();
		}
	}

	public void NewStart()
	{
		pcScript.gameStarted = true;
		mainMenuGroup.interactable = false;
		InvokeRepeating("FadeOut",0.01f, 0.02f);
		InvokeRepeating("InGameUIFadeIn",0.01f, 0.02f);

		/*
		 * logic:
		 * animate buttons out
		 * enable input + pause button
		 * 
		 */
	}

	public void StartOver()
	{
		transistionCanvas.instance.StartTransitionIn(Load);

	}

	public void Load()
	{
		SceneManager.LoadScene("levelScene");
	}
}
