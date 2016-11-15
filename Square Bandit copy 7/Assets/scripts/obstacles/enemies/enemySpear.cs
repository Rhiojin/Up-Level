using UnityEngine;
using System.Collections;

public class enemySpear : MonoBehaviour {


	float travelDirection = 7;
	float fastTravelDirection = 16;
	bool travelingRight = false;

	levelManager levelScript;

	Vector3 leftBounds;
	Vector3 rightBounds;
	float boundsEase = 1.5f;

	RaycastHit2D hit;
	bool hasTarget = false;
	public Animator anim;

	void Start () 
	{
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();

		if(Random.value > 0.5f)
		{
			travelDirection *= -1;
			fastTravelDirection *= -1;
			Vector3 s = transform.localScale;
			s.x *= -1;
			transform.localScale = s;
			travelingRight = false;
		}
		else
		{
			travelingRight = true;
		}



		leftBounds = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
		rightBounds = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));

		InvokeRepeating("RaycastForTarget",0.1f, 1);
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

		if(!hasTarget)transform.position += Vector3.right*travelDirection*Time.deltaTime;
		else transform.position += Vector3.right*fastTravelDirection*Time.deltaTime;

		if(transform.position.x > rightBounds.x+boundsEase) 
		{
			transform.position = new Vector3(leftBounds.x, transform.position.y, transform.position.z);
		}

		if(transform.position.x < leftBounds.x-boundsEase) 
		{
			transform.position = new Vector3(rightBounds.x+boundsEase, transform.position.y, transform.position.z);
		}
	}

	void RaycastForTarget()
	{
		if(!hasTarget)
		{
			if(travelingRight)
			{
				hit = Physics2D.Raycast(transform.position+Vector3.right*2,Vector2.right,200,1<<LayerMask.NameToLayer("player"));
			}
			else
			{
				hit = Physics2D.Raycast(transform.position+Vector3.right*2,Vector2.right,200,1<<LayerMask.NameToLayer("player"));
			}

			if(hit.collider != null)
			{
				hasTarget = true;
				anim.speed *= 2;
				anim.Play("npcSpearRun");
			}
		}
	}
}
