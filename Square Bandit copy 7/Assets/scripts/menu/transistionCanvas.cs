using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class transistionCanvas : MonoBehaviour {

	public static transistionCanvas instance;

	public RectTransform ballImage;
	public Image fade;
	Color fadeColor = Color.white;

	bool fadingIn = false;
	bool fadingOut = false;
	float fadeSpeed = 2.3f; //higher is faster
	System.Action currentAction;

	Vector2 ballOffScreen = new Vector2(-420,100);
	Vector2 ballTravelArc = new Vector2(1300, 1300);
	float ballRotateSpeed = -1080;
	float gravity = 80;

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
	}

	void Start () 
	{
		fadeColor.a = 0;
		ballOffScreen.y = Random.Range(100, -300);
		ballImage.anchoredPosition = ballOffScreen;
	}

	void Update () 
	{
		if(fadingIn) TransitionIn();
		if(fadingOut) TransitionOut();
	}

	public void StartTransitionIn(System.Action callback)
	{
		fadingIn = true;
		fadingOut = false;

		currentAction = callback;
		ballImage.anchoredPosition = ballOffScreen;
	}

	public void StartTransitionOut()
	{
		fadingOut = true;
		fadingIn = false;
	}

	public void TransitionOut()
	{
		fadeColor.a -= fadeSpeed*2f*Time.deltaTime;
		fade.color = fadeColor;
		if(fadeColor.a <= 0)
		{
			fadeColor.a = 0;
			fadingOut = false;
			ballOffScreen.y = Random.Range(100, -300);
			if(Random.value >= 0.5f)
			{
				ballOffScreen.x *= -1;
				ballTravelArc.x *= -1; 
				ballRotateSpeed *= -1;
			}

			ballImage.anchoredPosition = ballOffScreen;
			ballTravelArc.y = 1300;

		}
	}

	public void TransitionIn()
	{
		//print(fadeColor.a);
		fadeColor.a += fadeSpeed*Time.deltaTime;
		fade.color = fadeColor;
		if(fadeColor.a >= 1.5f)
		{
			fadeColor.a = 1;
			fadingIn = false;
			currentAction();
		}

		ballImage.Rotate(0,0,ballRotateSpeed*Time.deltaTime);
		ballImage.anchoredPosition +=ballTravelArc*Time.deltaTime;
		ballTravelArc.y -= gravity;
	}

	public void ChangeImage(Sprite newImage)
	{
		ballImage.GetComponent<Image>().sprite = newImage;
	}
}
