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
	public Sprite[] bgWalls;

	public int difficulty = 0;

	bool dead = false;

	public GameObject gameOverPanel;

	public SpriteRenderer Sky;
	public Color[] skyColors;
	int skyColorIndex = 0;

	public int score;
	public Text scoreDisplay;

	void Start () 
	{
//		InvokeRepeating( "ColorCheck", 0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
//		UpdateSky();
	}

	public void Pause()
	{
		if(!paused)
		{
			if(_didPause != null)
			{
				_didPause();
			}
		}
		else
		{
			if(_didUnpause != null)
			{
				_didUnpause();
			}
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

	public void FCT(Vector3 position)
	{
		
	}

	public void Scored(int scoreAmount)
	{
		score += scoreAmount;
		scoreDisplay.text = score.ToString();
	}

	public void GameOver()
	{
		dead = true;
		gameOverPanel.SetActive(true);
	}

	public void StartOver()
	{
		SceneManager.LoadScene("levelScene");
	}
}
