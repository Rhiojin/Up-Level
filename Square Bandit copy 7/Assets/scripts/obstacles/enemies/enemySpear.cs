using UnityEngine;
using System.Collections;

public class enemySpear : MonoBehaviour {


	float travelDirection = 8;

	levelManager levelScript;

	Vector3 leftBounds;
	Vector3 rightBounds;
	float boundsEase = 1.5f;

	void Start () 
	{
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();

		if(Random.value > 0.5f)
		{
			travelDirection *= -1;
			Vector3 s = transform.localScale;
			s.x *= -1;
			transform.localScale = s;
		}



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

		transform.position += Vector3.right*travelDirection*Time.deltaTime;

		if(transform.position.x > rightBounds.x+boundsEase) 
		{
			transform.position = new Vector3(leftBounds.x, transform.position.y, transform.position.z);
		}

		if(transform.position.x < leftBounds.x-boundsEase) 
		{
			transform.position = new Vector3(rightBounds.x+boundsEase, transform.position.y, transform.position.z);
		}
	}
}
