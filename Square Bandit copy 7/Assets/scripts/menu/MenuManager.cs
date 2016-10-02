using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public RectTransform currentPanel;
	public CanvasGroup currentCanvasGroup;
	public CanvasGroup mainMenuGroup;
	public CanvasGroup characterSelectMenuGroup;
	public CanvasGroup InGameGroup;
	public CanvasGroup gameOverGroup;

	public GameObject mainMenuPanel;
	public GameObject gameOverPanel;
	Vector2 onScreenPos = Vector2.zero;
	Vector2 offScreenPos = Vector2.zero;

	float fadeTime = 2;
	public pcControl pcScript;
	public bool gameStarted = false;

	public SpriteRenderer pcBody;
	public Text coinsText;
	public Text coinsTextStore;
	public Text highScoreText;

	void Start () 
	{
		transistionCanvas.instance.StartTransitionOut();
		currentCanvasGroup = mainMenuGroup;
		currentPanel = mainMenuPanel.GetComponent<RectTransform>();
		SetCoinsDisplay(PlayerPrefs.GetInt("coins",0));
		SetHighScoreDisplay(PlayerPrefs.GetInt("highscore",0));
	}
	
	public void GainCoins(int amount)
	{
		int c = PlayerPrefs.GetInt("coins",0);
		c += amount;
		PlayerPrefs.SetInt("coins",c);
		SetCoinsDisplay(c);
	}

	public void LoseCoins(int amount)
	{
		int c = PlayerPrefs.GetInt("coins",0);
		c -= amount;
		PlayerPrefs.SetInt("coins",c);
		SetCoinsDisplay(c);
	}

	void SetCoinsDisplay(int amount)
	{
		coinsText.text = amount.ToString();
		coinsTextStore.text = coinsText.text;
	}

	void SetHighScoreDisplay(int amount)
	{
		highScoreText.text = amount.ToString();
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
		currentCanvasGroup.interactable = false;
		canvasToFadeIn.interactable = true;
		while(canvasToFadeIn.alpha < 1 && currentCanvasGroup.alpha > 0) 
		{
			if(currentCanvasGroup.alpha > 0)
			{
				currentCanvasGroup.alpha -=Time.deltaTime*fadeTime;
				if(currentCanvasGroup.alpha <= 0) 
				{
//					currentCanvasGroup.interactable = false;
					currentCanvasGroup.blocksRaycasts = false;

				}
			}

			if(canvasToFadeIn.alpha < 1)
			{
				canvasToFadeIn.alpha +=Time.deltaTime*fadeTime;
				if(canvasToFadeIn.alpha >= 1) 
				{
//					canvasToFadeIn.interactable = true;
					canvasToFadeIn.blocksRaycasts = true;

				}
			}
			yield return null;
		}
		currentCanvasGroup = canvasToFadeIn;
		CancelInvoke("PanelToPanelFade");
	}

	public void PanelSwap(RectTransform panelToSwapIn)
	{
		StartCoroutine( StartPanelSwap(panelToSwapIn) );

	}
	 
	IEnumerator StartPanelSwap(RectTransform panelToSwapIn)
	{
//		while(panelToSwapIn.anchoredPosition != onScreenPos)  
//		{
//			panelToSwapIn.anchoredPosition += Vector2.up*2500*Time.deltaTime;
//			currentPanel.anchoredPosition -= Vector2.up*2500*Time.deltaTime;
//
//			if(Vector2.Distance(panelToSwapIn.anchoredPosition, onScreenPos) <= 100)
//			{
//				panelToSwapIn.anchoredPosition = onScreenPos;
//			}
//
//			yield return null;
//		}
//		currentPanel = panelToSwapIn;

		while(panelToSwapIn.anchoredPosition != onScreenPos)  
		{
			panelToSwapIn.anchoredPosition += Vector2.up*2500*Time.deltaTime;
			currentPanel.anchoredPosition -= Vector2.up*2500*Time.deltaTime;

			if(Vector2.Distance(panelToSwapIn.anchoredPosition, onScreenPos) <= 100)
			{
				panelToSwapIn.anchoredPosition = onScreenPos;
			}

			yield return null;
		}
		currentPanel = panelToSwapIn;
	}

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

	//extrabits
	public void PlayVideoAd()
	{
		AdManager.AdColony_PlayVideoAd();
	}
}
