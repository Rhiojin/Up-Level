using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class enemyCannon : MonoBehaviour {

	public Transform nozzle;
	public GameObject bullet;
	GameObject bulletHolder;
	int bulletIndex = 0;

	float shotTimer = 2;
	float manualTimer = 0;
	float rate = 0.015f;

	levelManager levelScript;
	public bool leftCannon = true;
	Animator anim;
	public ParticleSystem particles;
	bool checkGap = true;

	void Start () {
	
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();

		//reposition to more middle
		transform.position = new Vector3(Random.Range(-10,10), transform.position.y, transform.position.z);
		anim = GetComponent<Animator>();


	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(!levelScript.paused)
		{
			Shoot();
			if(checkGap)GapCheck();
		}
	}

	void Shoot()
	{
		manualTimer+= rate;
		if(shotTimer < manualTimer)
		{
			anim.Play("enemyCannon_shoot");
			particles.Emit(5);
			manualTimer = 0;

			bulletHolder = Instantiate(bullet, nozzle.position, nozzle.rotation) as GameObject;
			if(!leftCannon) 
			{
				bulletHolder.GetComponent<bullet>().movespeed *= -1;
			}
			Destroy(bulletHolder, 5);

		}
	}

	void GapCheck()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position-(transform.up*5),-(transform.up*10),0.5f);
//		Debug.DrawRay(transform.position-(transform.up*5),-(transform.up*10),Color.cyan,50);

		if (hit.collider != null && hit.collider.CompareTag("gapCheck")) 
		{
			print("qwer");
			if(transform.position.x < 0)
			{
				transform.position += transform.right*10;
			}
			else
			{
				transform.position -= transform.right*10;
			}

			checkGap = false;
		}
	}
		

}
