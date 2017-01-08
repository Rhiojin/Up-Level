using UnityEngine;
using System.Collections;

public class spikeHazard : MonoBehaviour {

	float rotationDirection = -5;
	float travelDirection = 12;

	levelManager levelScript;

	Vector3 leftBounds;
	Vector3 rightBounds;
	float boundsEase = 1.5f;

	void Start () 
	{
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();
		
		if(Random.value > 0.5f)
		{
			rotationDirection *= -1;
			travelDirection *= -1;
		}

//		leftBounds = Camera.main.ScreenToWorldPoint(Screen.width-Screen.width); //i suppose this could just be '0'
//		rightBounds = Camera.main.ScreenToWorldPoint(Screen.width);

		leftBounds = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
		rightBounds = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!levelScript.paused)
		{
			Move();
		}
	}

	void Move()
	{
		transform.Rotate(0,0,rotationDirection);
		transform.position += Vector3.right*travelDirection*Time.deltaTime;

		if(transform.position.x > rightBounds.x+boundsEase) 
		{
			transform.position = new Vector3(leftBounds.x-boundsEase, transform.position.y, transform.position.z);
		}

		if(transform.position.x < leftBounds.x-boundsEase) 
		{
			transform.position = new Vector3(rightBounds.x+boundsEase, transform.position.y, transform.position.z);
		}
	}
}
