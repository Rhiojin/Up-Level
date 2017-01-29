using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class levelManager : MonoBehaviour {

	public delegate void playerDeathDelegate();
	public event playerDeathDelegate _playerDidDie;

	public delegate void gamePauseDelegate();
	public event gamePauseDelegate _didPause;

	public delegate void gameUnpauseDelegate();
	public event gamePauseDelegate _didUnpause;

	public bool paused = false;

	public Color[] BGcolors;
	public Color[] waterColors;
	public Sprite[] bgWalls;

	public int difficulty = 0;

	bool dead = false;

	public GameObject mainMenuPanel;
	public GameObject gameOverPanel;

	public int score;
	public Text scoreDisplay;
	public Text gameOverScoreDisplay;

	public bool gameStarted = false;

	Vector3 cameraStartPos = new Vector3(0, 1,-10);
	Vector3 pcStartPos = new Vector3(0, -23.1f,0);
	Vector3 startPlatformStartPos = new Vector3(0, -35.1f,0);
	public GameObject startPlatform;
	public GameObject startPlatformHolder;
	public GameObject pc;
	public camControl camScript;
	pcControl pcScript;
	public stageMaker stageMakerScript;
	Vector3 lastPcPoint =  new Vector3(0,500,0);
	Vector3 eraserTargetPos = new Vector3(0,500,0);
	public Transform eraser;
	float eraserSpeed = 20;
	public CanvasGroup mainMenuGroup;
	public CanvasGroup InGameGroup;
	public CanvasGroup gameOverGroup;
	public CanvasGroup pauseMenuGroup;
	public CanvasGroup respawnGroup;
	public MenuManager menuScript;
	public FCTCanvas fctCanvasScript;
	public Text coinsText;
	public Text coinsTextStore;
	public GameObject singleCoin;
	public CanvasGroup ingameCoinDisplayGroup;
	public Text ingameCoinDisplay;

	public GameObject vidAdRespawnButton;
	public GameObject coinRespawnButton;

	float fadeTime = 2;

	void Start () 
	{
		pcScript = pc.GetComponent<pcControl>();
		transistionCanvas.instance.StartTransitionOut();
		ingameCoinDisplay.text = PlayerPrefs.GetInt("coins",0).ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		FadeCoin();
	}

	public void Pause()
	{
		
		if(!paused)
		{
			if(_didPause != null)
			{
				_didPause();
			}
			InvokeRepeating("FadeInPauseMenu",0.01f,0.1f);
		}
		else
		{
			if(_didUnpause != null)
			{
				_didUnpause();
			}
			InvokeRepeating("FadeOutPauseMenu",0.01f,0.1f);
		}

		paused = !paused;
	}

	public void WatchVideoAd()
	{
		AdManager.AdColony_PlayVideoAd();
	}

	public IEnumerator Flicker(SpriteRenderer sprite)
	{
		sprite.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = false;
	}

	public void Scored(int scoreAmount)
	{
		score += scoreAmount;
		scoreDisplay.text = score.ToString();
		gameOverScoreDisplay.text = scoreDisplay.text;
		fctCanvasScript.SetFCT(pc.transform.position, scoreAmount);
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
		if(coinsText)coinsText.text = amount.ToString();
		if(coinsTextStore)coinsTextStore.text = coinsText.text;
	}

	public void MakeACoin(Vector3 position)
	{
		Instantiate(singleCoin, position, singleCoin.transform.rotation);
	}

	public void GainCoinsIngame(int amount)
	{
		int c = PlayerPrefs.GetInt("coins",0);
		c += amount;
		PlayerPrefs.SetInt("coins",c);
		ingameCoinDisplay.text = c.ToString();
		ingameCoinDisplayGroup.alpha = 1;
	}

	void FadeCoin()
	{
		if(ingameCoinDisplayGroup.alpha >= 0)
		{
			ingameCoinDisplayGroup.alpha -= 0.2f*Time.deltaTime*fadeTime;
		}
	}

	void FadeInPauseMenu()
	{
		CancelInvoke("FadeOutPauseMenu");
		if(pauseMenuGroup.interactable == false)pauseMenuGroup.interactable = true;
		if(pauseMenuGroup.alpha < 1)
		{
			pauseMenuGroup.alpha += 5*Time.deltaTime*fadeTime;
			if(pauseMenuGroup.alpha >= 1) 
			{
					
				pauseMenuGroup.blocksRaycasts = true;
				CancelInvoke("FadeInPauseMenu");
			}
		}
	}

	void FadeOutPauseMenu()
	{
		CancelInvoke("FadeInPauseMenu");
		if(pauseMenuGroup.interactable == true)pauseMenuGroup.interactable = false;
		if(pauseMenuGroup.alpha > 0)
		{
			pauseMenuGroup.alpha -= 5*Time.deltaTime*fadeTime;
			if(pauseMenuGroup.alpha <= 0) 
			{

				pauseMenuGroup.blocksRaycasts = true;
				CancelInvoke("FadeOutPauseMenu");
			}
		}
	}

	public void GameOver()
	{
		dead = true;
		StartCoroutine( TurnOffPC() );
		lastPcPoint = pc.transform.position;

		// if lastLandSpot is within camera area, give respawn option

		StartCoroutine( menuScript.FadeDelay() );

		int hs = PlayerPrefs.GetInt("highscore",0);
		if(hs < score)
		{
			PlayerPrefs.SetInt("highscore",score);
		}

		PlayerPrefs.SetInt("lastscore", score);
		UnRegisterForAdWatch();
	}

	IEnumerator TurnOffPC()
	{
		yield return new WaitForSeconds(1);
		if(pcScript.dead) pc.SetActive(false);
	}

	public void ShowRespawnMenu()
	{
		if(!AdManager.AdColony_AdAvailable())
		{
			vidAdRespawnButton.SetActive(false);
		}
		if(PlayerPrefs.GetInt("coins",0) < 100)
		{
			coinRespawnButton.SetActive(false);
		}
		InvokeRepeating("FadeInRespawnMenu",0.01f,0.1f);
	}

	public void CloseRespawnMenu()
	{
		InvokeRepeating("FadeOutRespawnMenu",0.01f,0.1f);
	}

	public void OnVideoWatched(int cw)
	{
		pcScript.Respawn();
	}

	void FadeInRespawnMenu()
	{
		CancelInvoke("FadeOutRespawnMenu");
		if(respawnGroup.interactable == false)respawnGroup.interactable = true;
		if(respawnGroup.alpha < 1)
		{
			respawnGroup.alpha += 5*Time.deltaTime*fadeTime;
			if(respawnGroup.alpha >= 1) 
			{

				respawnGroup.blocksRaycasts = true;
				CancelInvoke("FadeInPauseMenu");
			}
		}
	}

	void FadeOutRespawnMenu()
	{
		CancelInvoke("FadeInRespawnMenu");
		if(respawnGroup.interactable == true)respawnGroup.interactable = false;
		if(respawnGroup.alpha > 0)
		{
			respawnGroup.alpha -= 5*Time.deltaTime*fadeTime;
			if(respawnGroup.alpha <= 0) 
			{

				respawnGroup.blocksRaycasts = true;
				CancelInvoke("FadeOutPauseMenu");
			}
		}
	}

	public void PlayButtonClick()
	{
		soundManager.instance.PlayClip("buttonClick", 0.7f);
	}

	public void RegisterForAdWatch()
	{
		AdManager._Adcolony_didFinishVideo += OnVideoWatched;
	}

	public void UnRegisterForAdWatch()
	{
		AdManager._Adcolony_didFinishVideo -= OnVideoWatched;
	}

}
