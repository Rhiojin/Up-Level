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

	void Start () {
	
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();

		//reposition to more middle
		transform.position = new Vector3(Random.Range(-10,10), transform.position.y, transform.position.z);
//		if(Random.value > 0.5f)
//		{
//			transform.localScale = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z);
//			leftCannon = false;
//		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(!levelScript.paused)
		{
			Shoot();
		}
	}

	void Shoot()
	{
		manualTimer+= rate;
		if(shotTimer < manualTimer)
		{
			manualTimer = 0;

			bulletHolder = Instantiate(bullet, nozzle.position, nozzle.rotation) as GameObject;
			if(!leftCannon) 
			{
				bulletHolder.GetComponent<bullet>().movespeed *= -1;
			}
			Destroy(bulletHolder, 5);

		}
	}

}
