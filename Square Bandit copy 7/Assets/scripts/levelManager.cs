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

	public SpriteRenderer Sky;
	public Color[] skyColors;
	int skyColorIndex = 0;

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
	public MenuManager menuScript;
	public FCTCanvas fctCanvasScript;
	public Text coinsText;
	public Text coinsTextStore;
	public GameObject singleCoin;
	public CanvasGroup ingameCoinDisplayGroup;
	public Text ingameCoinDisplay;

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

	void UpdateSky()
	{
//		Sky.color = Color.Lerp(Sky.color, skyColors[skyColorIndex], 0.02f*Time.deltaTime);

	}

	void ColorCheck()
	{
		if(Mathf.Approximately(Sky.color.r, skyColors[skyColorIndex].r)) skyColorIndex++;
		if(skyColorIndex >= skyColors.Length) skyColorIndex = 0;
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
		coinsText.text = amount.ToString();
		coinsTextStore.text = coinsText.text;
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


//	public void Play()
//	{
//		if(gameStarted)
//		{
//			StartOver();
//		}
//		else
//		{
//			NewStart();
//		}
//	}
//
//	public void NewStart()
//	{
//		pcScript.gameStarted = true;
//		mainMenuGroup.interactable = false;
//		InvokeRepeating("FadeOut",0.01f, 0.02f);
//		InvokeRepeating("InGameUIFadeIn",0.01f, 0.02f);
//
//		/*
//		 * logic:
//		 * animate buttons out
//		 * enable input + pause button
//		 * 
//		 */
//	}
//
//	public void StartOver()
//	{
//		transistionCanvas.instance.StartTransitionIn(Load);
//	}
//
	public void GameOver()
	{
		dead = true;
		pc.SetActive(false);
		lastPcPoint = pc.transform.position;
		StartCoroutine( menuScript.FadeDelay() );

		int hs = PlayerPrefs.GetInt("highscore",0);
		if(hs < score)
		{
			PlayerPrefs.SetInt("highscore",score);
		}

		PlayerPrefs.SetInt("lastscore", score);

	}
//
//	IEnumerator FadeDelay()
//	{
//		yield return new WaitForSeconds(0.5f);
//		InvokeRepeating("InGameUIFadeOut",0.01f, 0.02f);
//		InvokeRepeating("GameOverUIFadeIn",0.01f, 0.02f);
//
//	}
//
//	public void Load()
//	{
//		SceneManager.LoadScene("levelScene");
//	}
//
//	void FadeOut()
//	{
//		if(mainMenuGroup.alpha > 0)
//		{
//			mainMenuGroup.alpha -=Time.deltaTime*fadeTime;
//			if(mainMenuGroup.alpha <= 0) 
//			{
//				mainMenuPanel.SetActive(false);
//				CancelInvoke("FadeOut");
//			}
//		}
//	}
//
//	void GameOverUIFadeIn()
//	{
//		if(gameOverGroup.interactable == false)gameOverGroup.interactable = true;
//		if(gameOverGroup.alpha < 1)
//		{
//			gameOverGroup.alpha +=Time.deltaTime*fadeTime;
//			if(gameOverGroup.alpha >= 1) 
//			{
//				
//				gameOverGroup.blocksRaycasts = true;
//				CancelInvoke("GameOverUIFadeIn");
//			}
//		}
//	}
//
//	void InGameUIFadeOut()
//	{
//		if(InGameGroup.alpha > 0)
//		{
//			InGameGroup.alpha -=Time.deltaTime*fadeTime;
//			if(InGameGroup.alpha <= 0) 
//			{
//				print("fac");
//				InGameGroup.interactable = false;
//				InGameGroup.blocksRaycasts = false;
//				CancelInvoke("InGameUIFadeOut");
//			}
//		}
//	}
//
//	void InGameUIFadeIn()
//	{
//		if(InGameGroup.alpha < 1)
//		{
//			InGameGroup.alpha +=Time.deltaTime*fadeTime;
//			if(InGameGroup.alpha >= 1) 
//			{
//				print("fac1");
//				InGameGroup.interactable = true;
//				InGameGroup.blocksRaycasts = true;
//				CancelInvoke("InGameUIFadeIn");
//			}
//		}
//	}

//	IEnumerator Resets()
//	{
//		RunEraser();
//		yield return new WaitForSeconds(2);
//		startPlatformHolder = Instantiate(startPlatform, startPlatformStartPos, startPlatform.transform.rotation) as GameObject;
//		stageMakerScript.Reset();
//
//		pc.transform.position = pcStartPos;
//		pc.SetActive(true);
//		camScript.targetPos = cameraStartPos;
//	}

//	void RunEraser()
//	{
//		print("aw");
//		eraser.position = new Vector3(0, -15, 0);
//		eraserTargetPos.y = lastPcPoint.y + 200;
//		eraser.gameObject.SetActive(true);
//		InvokeRepeating("MoveEraser", 0.1f, 0.02f);
//	}
//
//	void MoveEraser()
//	{
//		if(eraser.gameObject.activeSelf)
//			eraser.position = Vector3.MoveTowards(eraser.position, eraserTargetPos, eraserSpeed*Time.deltaTime);
//		if(eraserTargetPos.y - eraser.position.y < 10)
//		{
//			CancelInvoke("MoveEraser");
//			eraser.gameObject.SetActive(false); 
//		}
//	}


}
