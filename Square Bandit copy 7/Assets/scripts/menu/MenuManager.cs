using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
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
	Vector2 offScreenPos = new Vector2(0,-1500);

	float fadeTime = 2;
	public pcControl pcScript;
	public bool gameStarted = false;

	public SpriteRenderer pcBody;
	public Text coinsText;
	public Text coinsTextStore;
	public Text coinsTextIAP;
	public Text highScoreText;
	public Text highScoreShareText;

	public Dictionary<string, int> shopItems = new Dictionary<string, int>();
	public Transform shopShelf;
	bool shaking = false;
	public Text fctText;
	public CanvasGroup fctTextGroup;
	public RectTransform fctTextTransform;
	public RectTransform sharePanel;

	public Image hat;
	public Image body;
	public Image face;
	public Image armsL;
	public Image armsR;
	public Image handsL;
	public Image handsR;
	public Image legsL;
	public Image legsR;

	public Text removeAdsPrice;
	public Text gp1price;
	public Text gp2price;
	public Text gp3price;

	List<int> listOfPrices = new List<int>();
	public GameObject theShine;
	public Image storeButtonImage;
	public Color storeButtonShineColorOne;
	public Color storeButtonShineColorTwo;
	float storeButtonTimer = 0;
	float storeButtonLerpSpeed = 2f;

	bool shouldShine = false;
	public GameObject shareButton;
	public GameObject shareCancelButton;

	void Awake()
	{
		Application.targetFrameRate = 60;
		
//		PlayerPrefs.SetInt("coins",3000);
		
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

				priceText.GetComponent<Text>().text = price.ToString();
				if(PlayerPrefs.GetInt(child.name+"Unlocked",0) == 1)
				{
					child.transform.Find("priceBlocker").gameObject.SetActive(false);
				}
				else
				{
					listOfPrices.Add(price);
				}
			}
			else
			{
				Debug.LogWarning(child.name+" does not have a priceblocker");
			}

			if(1>0 && i%5 == 0)
			{
				price+=25;
			}



		}

		if(listOfPrices.Count > 0)
		{
			CheckForPurchaseableSkin();
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
		IAPmanager.instance.didGetPrice += SetPrices;
		IAPmanager.instance.didReadyStore += RequestPrices;	

		StartCoroutine( LoadSocialSkin() );
	
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.R))
		{
			GetAdReward(70);
		}
	}

	void FixedUpdate()
	{
		if(shouldShine)
		{
			ShineButton();
		}

		//debug
//		if(Input.GetKeyUp(KeyCode.R))
//		{
//			GetAdReward(70);
//		}
	}

	public void RequestPrices()
	{
		IAPmanager.instance.RequestPrice("removeads");
		IAPmanager.instance.RequestPrice("goldpack1");
		IAPmanager.instance.RequestPrice("goldpack2");
		IAPmanager.instance.RequestPrice("goldpack3");
	}

	public void SetPrices(string product, string price)
	{
		switch(product)
		{
		case("removeads"):
			{
				removeAdsPrice.text = price;
			}
			break;

		case("goldpack1"):
			{
				gp1price.text = price;
			}
			break;

		case("goldpack2"):
			{
				gp2price.text = price;
			}
			break;

		case("goldpack3"):
			{
				gp3price.text = price;
			}
			break;
		}
	}

	public void GetPurchase(string product)
	{
		switch(product)
		{
		case("removeads"):
			{
				PlayerPrefs.SetInt("removedAds",1);
				AdManager.Admob_HideABannerAd();
			}
			break;

		case("goldpack1"):
			{
				GainCoins(200);
			}
			break;

		case("goldpack2"):
			{
				GainCoins(500);
			}
			break;

		case("goldpack3"):
			{
				GainCoins(900);
			}
			break;
		}
	}

	public void GainCoins(int amount)
	{
		int c = PlayerPrefs.GetInt("coins",0);
		print(c);
		c += amount;
		print(c);
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
		coinsTextIAP.text = coinsText.text;
	}

	void SetHighScoreDisplay(int amount)
	{
		highScoreText.text = amount.ToString();
		highScoreShareText.text = highScoreText.text;
	}

	public void SetSkin(string name)
	{
		int c = PlayerPrefs.GetInt("coins",0);
		if(shopItems.ContainsKey(name))
		{
			if(PlayerPrefs.GetInt(name+"Unlocked",0) == 0)
			{
				//unlock skin if cash is available
				if(c >= shopItems[name])
				{
					c -= shopItems[name];
					PlayerPrefs.SetInt("coins",c);
					PlayerPrefs.SetInt(name+"Unlocked",1);

					SetCoinsDisplay(c);
					pcScript.UpdateSkin(name);
					StartCoroutine( UpdateSocialSkin(name) );
					shopShelf.transform.Find(name+"/priceBlocker").gameObject.SetActive(false);
					SetMenuFCT(shopShelf.transform.Find(name+"/priceBlocker").position,shopItems[name]);

					//pop price from list
					listOfPrices.Remove(shopItems[name]);
				}
				else
				{
					//shake
					if(!shaking)StartCoroutine( Shake(coinsTextStore.transform) );
				}
			}
			else
			{
				pcScript.UpdateSkin(name);
				StartCoroutine( UpdateSocialSkin(name) );
			}
		}
		else
		{
			if(name == "viking")
			{
				pcScript.UpdateSkin(name);
				StartCoroutine( UpdateSocialSkin(name) );
			}
			else print("priceblocker not targeting correct skin");
		}

	}

	void CheckForPurchaseableSkin()
	{
		int coins = PlayerPrefs.GetInt("coins",0);
		if(listOfPrices.Any(o => o <= coins))
		{
			shouldShine = true;
			InvokeRepeating("ShineButton",0.1f,0.05f);
				
		}
	}

	void ShineButton()
	{
		storeButtonTimer = Mathf.PingPong(Time.time * storeButtonLerpSpeed, 1.0f);
		storeButtonImage.color = Color.Lerp(storeButtonShineColorOne, storeButtonShineColorTwo, storeButtonTimer);
	}

	public void HideShine()
	{
		CancelInvoke("ShineButton");
		shouldShine = false;

	}

	IEnumerator LoadSocialSkin()
	{
		//pull character art from sprite sheet. Assigned in order of appearance
		string pathPrefix = "Skins/upLevel-character-";
		string skinName = PlayerPrefs.GetString("selectedSkin","viking");
		string fullPath = pathPrefix+skinName;
		Sprite[] skinData = Resources.LoadAll<Sprite>(fullPath);
		yield return new WaitForSeconds(0.25f);
		body.sprite = skinData[0];
		hat.sprite = skinData[1];
		armsL.sprite = skinData[2];
		armsR.sprite = skinData[2];
		yield return new WaitForSeconds(0.25f);
		legsL.sprite = skinData[4];
		legsR.sprite = skinData[4];
		yield return new WaitForSeconds(0.25f);
		face.sprite = skinData[5];
		handsL.sprite = skinData[6];
		handsR.sprite = skinData[6];
	}

	IEnumerator UpdateSocialSkin(string name)
	{
		//pull character art from sprite sheet. Assigned in order of appearance
		string pathPrefix = "Skins/upLevel-character-";
		string fullPath = pathPrefix+name;
		Sprite[] skinData = Resources.LoadAll<Sprite>(fullPath);
		yield return new WaitForSeconds(0.25f);
		body.sprite = skinData[0];
		hat.sprite = skinData[1];
		armsL.sprite = skinData[2];
		armsR.sprite = skinData[2];
		yield return new WaitForSeconds(0.25f);
		legsL.sprite = skinData[4];
		legsR.sprite = skinData[4];
		yield return new WaitForSeconds(0.25f);
		face.sprite = skinData[5];
		handsL.sprite = skinData[6];
		handsR.sprite = skinData[6];
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

	void FadeIn()
	{
		mainMenuPanel.SetActive(true);
		if(mainMenuGroup.alpha < 1)
		{
			mainMenuGroup.alpha +=Time.deltaTime*fadeTime;
			if(mainMenuGroup.alpha >= 1) 
			{
				CancelInvoke("FadeIn");
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
				GameObject.Find("Optimizer").GetComponent<optimizer>().StartOptimize();
				//call optimizer
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
		AdManager._Adcolony_didFinishVideo -= GetAdReward;
		IAPmanager.instance.didGetPrice -= SetPrices;
		IAPmanager.instance.didReadyStore -= RequestPrices;	

		GameObject.Find("level manager").GetComponent<levelManager>().RegisterForAdWatch();
		
		CancelInvoke("ShineButton");
		shouldShine = false;
		pcScript.gameStarted = true;
		mainMenuGroup.interactable = false;
		InvokeRepeating("FadeOut",0.01f, 0.02f);
		InvokeRepeating("InGameUIFadeIn",0.01f, 0.02f);

		//the LAZY AF way of doing this - create tutorial
		int p = PlayerPrefs.GetInt("playCount",0);
		if(p <= 5) //sync number with stagemaker
		{
			GameObject obj = GameObject.Find("TutorialCanvasGroup(Clone)");
			obj.SendMessage("StartFadeIn",SendMessageOptions.DontRequireReceiver);
		}

	}

	public void StartOver()
	{
		transistionCanvas.instance.StartTransitionIn(Load);

	}

	public void Load()
	{
		SceneManager.LoadScene("levelScene");
	}

	public void ShowShareOverlay()
	{
		StopCoroutine( "MoveShareOverlayOut" );
		InvokeRepeating("FadeOut",0.01f, 0.02f);
		StartCoroutine( "MoveShareOverlayIn" );
	}

	public void HideShareOverlay()
	{
		StopCoroutine( "MoveShareOverlayIn" );
		sharePanel.anchoredPosition = Vector2.zero;
		InvokeRepeating("FadeIn",0.01f, 0.02f);
		StartCoroutine( "MoveShareOverlayOut" );
	}

	IEnumerator MoveShareOverlayIn()
	{
		
		while(sharePanel.anchoredPosition != onScreenPos)
		{
			sharePanel.anchoredPosition = Vector2.Lerp(sharePanel.anchoredPosition, onScreenPos, 10*Time.deltaTime);
			yield return null;
		}
		sharePanel.anchoredPosition = Vector2.zero;
	}

	IEnumerator MoveShareOverlayOut()
	{
		
		while(sharePanel.anchoredPosition != offScreenPos)
		{
			sharePanel.anchoredPosition = Vector2.Lerp(sharePanel.anchoredPosition, offScreenPos, 10*Time.deltaTime);
			yield return null;
		}
		sharePanel.anchoredPosition = offScreenPos;
	}

	public void ShareFB()
	{
	}

	public void ShareTW()
	{
		
	}

	//extrabits
	public void PlayVideoAd(Button button)
	{
		AdManager.AdColony_PlayVideoAd(button);
	}

	public void GetAdReward(int amount)
	{
		GainCoins(amount);
		StartCoroutine( StartMenuFCTForAds(amount) );
	}

	public void SetMenuFCT(Vector3 pos,int p)
	{
		fctTextTransform.transform.position = pos;
		StartCoroutine( StartMenuFCT(p) );
	}

	public void SetMenuFCTForAds(Vector3 pos,int p)
	{
		fctTextTransform.transform.position = pos;
		StartCoroutine( StartMenuFCTForAds(p) );
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

	IEnumerator StartMenuFCTForAds (int points) 
	{
		fctText.text = "+"+points.ToString();
		fctTextGroup.alpha = 1;
		fctTextTransform.anchoredPosition = Vector3.zero;
		while(fctTextGroup.alpha > 0)
		{
			fctTextGroup.alpha -= 0.25f*Time.deltaTime;
			fctTextTransform.anchoredPosition += 50*Vector2.up*Time.deltaTime;
			yield return null;
		}
	}

	public void HideButtons(bool hide)
	{
		if(hide)
		{
			shareButton.SetActive(false);
			shareCancelButton.SetActive(false);
		}
		else
		{
			shareButton.SetActive(true);
			shareCancelButton.SetActive(true);
		}
	}
}
