using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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

	public Dictionary<string, int> shopItems = new Dictionary<string, int>();
	public Transform shopShelf;
	bool shaking = false;
	public Text fctText;
	public CanvasGroup fctTextGroup;
	public RectTransform fctTextTransform;

	void Awake()
	{
		PlayerPrefs.SetInt("coins",10000);
		
		// build shop dict modularly. All that is required for adding a new skin is making the button in the content holder;
		int childCount = shopShelf.transform.childCount;
		int price = 75;
		Transform child;
		Transform priceText;
		for(int i = 0; i < childCount; i++)
		{
			child = shopShelf.GetChild(i);
			priceText = child.transform.Find("priceBlocker/priceHolder/price");
			if(priceText != null)
			{
				shopItems.Add(child.name, price);
				priceText.GetComponent<Text>().text = price.ToString();;
				if(PlayerPrefs.GetInt(child.name+"Unlocked",0) == 1)
				{
					child.transform.Find("priceBlocker").gameObject.SetActive(false);
				}
			}
			else
			{
				Debug.LogWarning(child.name+" does not have a priceblocker");
			}
			if(1>0 && i%5 == 0) price+=25;


		}

	}

	void Start () 
	{
		transistionCanvas.instance.StartTransitionOut();
		currentCanvasGroup = mainMenuGroup;
		currentPanel = mainMenuPanel.GetComponent<RectTransform>();
		SetCoinsDisplay(PlayerPrefs.GetInt("coins",0));
		SetHighScoreDisplay(PlayerPrefs.GetInt("highscore",0));

		AdManager._Adcolony_didFinishVideo += GetAdReward;
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
		int c = PlayerPrefs.GetInt("coins",0);
		if(shopItems.ContainsKey(name))
		{
			if(c >= shopItems[name])
			{
				c -= shopItems[name];
				PlayerPrefs.SetInt("coins",c);
				PlayerPrefs.SetInt(name+"Unlocked",1);

				SetCoinsDisplay(c);
				pcScript.UpdateSkin(name);
				shopShelf.transform.Find(name+"/priceBlocker").gameObject.SetActive(false);
				SetMenuFCT(shopShelf.transform.Find(name+"/priceBlocker").position,shopItems[name]);
			}
			else
			{
				//shake
				if(!shaking)StartCoroutine( Shake(coinsTextStore.transform) );
			}
		}
		else
		{
			print("priceblocker not targeting correct skin");
		}

	}

	IEnumerator Shake(Transform obj)
	{
		shaking = true;
		float x = 8.5f;
		float y = x;
		float settleSpeed = 0.4f;
		Vector3 originalPos = obj.transform.position;
		Vector3 targetPos = originalPos;

		do
		{
			targetPos.x = originalPos.x+Random.Range(-x,x);
			targetPos.y = originalPos.y+Random.Range(-y,y);
			obj.position = targetPos;
			x -= settleSpeed;
			y -= settleSpeed;
			yield return null;
		}
		while(x > 0);
		obj.position = originalPos;
		shaking = false;


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
		yield return new WaitForSeconds(0.15f);
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

				//gameOverGroup.blocksRaycasts = true;
				CancelInvoke("GameOverUIFadeIn");
				StartCoroutine( Restart() );
			}
		}
	}

	IEnumerator Restart()
	{
		yield return new WaitForSeconds(1.25f);
		StartOver();
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

	public void GetAdReward(int amount)
	{
		GainCoins(amount);
	}

	public void SetMenuFCT(Vector3 pos,int p)
	{
		fctTextTransform.transform.position = pos;
		StartCoroutine( StartMenuFCT(p) );
	}


	IEnumerator StartMenuFCT (int points) 
	{
		fctText.text = "-"+points.ToString();
		fctTextGroup.alpha = 1;
//		fctTextTransform.anchoredPosition = Vector3.zero;
		while(fctTextGroup.alpha > 0)
		{
			fctTextGroup.alpha -= 0.5f*Time.deltaTime;
			fctTextTransform.anchoredPosition += 100*Vector2.up*Time.deltaTime;
			yield return null;
		}
	}
}
