using UnityEngine;
using System.Collections;

public class stageMaker : MonoBehaviour {

	public GameObject platform;
	public GameObject platformLong;
	public GameObject platformLongBreakables;
	public GameObject platformLongDonkeyCannon;
	public GameObject platformBossRoom;
	public GameObject Light2D;

	Vector3 spawnPoint = new Vector3(0, 10, 0);
	float xRange = 20;
	float delta = 45;
	float longPlatformDelta = 90; //100 - 10

	int floorCount = 0;
	int floorMax = 4; //4

	int lightCount = 0;
	int lightCountMax = 2;
	Vector3 lightSpawnpoint = new Vector3(-150,70,0);
	float lightSpawnDelta = 300;

	GameObject platformHolder;
	GameObject hazardHolder;
	int difficulty = 1;


	public GameObject[] groundHazards;
	public GameObject[] airHazards;
	public GameObject[] npcHazards;


	Vector3 groundHazardSpawnPoint = new Vector3(0, 10, 0);
	Vector3 airHazardSpawnPoint = new Vector3(0, 40, 0);
	int randomizer = 0;
	bool longSpawn = false;





	void Start () 
	{
		//spawnPoint.x = Random.Range(-xRange, xRange);
//		Instantiate(platform, spawnPoint, platform.transform.rotation);

		InvokeRepeating("SpawnPlatform", 0.1f, 0.1f);
		InvokeRepeating("SpawnLight", 0.1f, 0.1f);
	}
	

	void Update () {
	
	
	}

	void SpawnPlatform()
	{
		//spawnPoint.x = Random.Range(-xRange, xRange);

		if(floorCount < floorMax)
		{
			longSpawn = false;
			spawnPoint.y += delta;

			//decide platform based on difficulty

			int p = 0;
			if(difficulty%5 == 0)
			{
				p = Random.Range(1,4); // rework to take difficuly into consideration
			}
			else p = 0;

			switch(p)
			{
			case(0):
				{
					platformHolder = Instantiate(platform, spawnPoint, platform.transform.rotation) as GameObject;
					floorCount++;
					difficulty++;
				}
				break;

			case(1):
				{
					spawnPoint.y += longPlatformDelta;
					platformHolder = Instantiate(platformLong, spawnPoint, platform.transform.rotation) as GameObject;
//					floorCount+=2;
					floorCount++;

					difficulty++;
					spawnPoint.y += delta;
					groundHazardSpawnPoint.y += longPlatformDelta+delta;
					airHazardSpawnPoint.y += longPlatformDelta+delta;
					longSpawn = true;
				}
				break;

			case(2):
				{
					spawnPoint.y += longPlatformDelta;
					platformHolder = Instantiate(platformLongBreakables, spawnPoint, platform.transform.rotation) as GameObject;
					floorCount++;

					difficulty++;
					spawnPoint.y += delta;
					groundHazardSpawnPoint.y += longPlatformDelta+delta;
					airHazardSpawnPoint.y += longPlatformDelta+delta;
					longSpawn = true;
				}
				break;

			case(3):
				{
					spawnPoint.y += longPlatformDelta;
					platformHolder = Instantiate(platformLongDonkeyCannon, spawnPoint, platform.transform.rotation) as GameObject;
					floorCount++;

					difficulty++;
					spawnPoint.y += delta;
					groundHazardSpawnPoint.y += longPlatformDelta+delta;
					airHazardSpawnPoint.y += longPlatformDelta+delta;
					longSpawn = true;
				}
				break;

				default:
				{
					platformHolder = Instantiate(platform, spawnPoint, platform.transform.rotation) as GameObject;
					floorCount++;
					difficulty++;
				}
				break;
			}

//			platformHolder = Instantiate(platform, spawnPoint, platform.transform.rotation) as GameObject;
//			floorCount++;
//			difficulty++;

			//add hazards and such

			// need to decide how to add multiple hazards
			int c = Random.Range(0,3);
			if(longSpawn)
			{
				c = 1;
//				if(c = 0)
//				{
//					c++;
//				}
//
//				if(c = 2)
//				{
//					c++;
//				}
			}
			switch(c)
			{

			case(0):
				{
					randomizer = Random.Range(0,groundHazards.Length);
					groundHazardSpawnPoint.x = Random.Range(-25, 25);
					hazardHolder = Instantiate(groundHazards[randomizer], groundHazardSpawnPoint, new Quaternion(0,0,0,0)) as GameObject;

				}
				break;

			case(1):
				{
					randomizer = Random.Range(0,airHazards.Length);
					airHazardSpawnPoint.x = Random.Range(-25, 25);
					hazardHolder = Instantiate(airHazards[randomizer], airHazardSpawnPoint, new Quaternion(0,0,0,0)) as GameObject;
				}
				break;

			case(2):
				{
					randomizer = Random.Range(0,npcHazards.Length);
					airHazardSpawnPoint.x = Random.Range(-25, 25);
					hazardHolder = Instantiate(npcHazards[randomizer], airHazardSpawnPoint, new Quaternion(0,0,0,0)) as GameObject;
				}
				break;
			}

//			randomizer = Random.Range(0,groundHazards.Length);
//			groundHazardSpawnPoint.x = Random.Range(-25, 25);
//			hazardHolder = Instantiate(groundHazards[randomizer], groundHazardSpawnPoint, new Quaternion(0,0,0,0)) as GameObject;
		

			groundHazardSpawnPoint.y += delta;
			airHazardSpawnPoint.y += delta;

		}


	}

	void SpawnLight()
	{
		if(lightCount < lightCountMax)
		{
			Instantiate(Light2D,lightSpawnpoint, Light2D.transform.rotation);
			lightSpawnpoint.x *= -1;
			lightSpawnpoint.y += lightSpawnDelta;
			lightCount++;

		}
	}

	void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.CompareTag("trashTrigger"))
		{
			Destroy(trig.transform.parent.gameObject);
			floorCount--;
		}

		if(trig.CompareTag("lightTrashTrigger"))
		{
			Destroy(trig.transform.parent.gameObject);
			lightCount--;
		}
//		if(trig.CompareTag("longTrashTrigger"))
//		{
//			Destroy(trig.transform.parent.gameObject);
//			floorCount-=2;
//		}
		else
		{
			Destroy(trig.transform.gameObject);
		}


	}

	void OnCollisionEnter2D(Collision2D col)
	{
//		if(col.collider.CompareTag("trashTrigger"))
//		{
			Destroy(col.gameObject);

//		}
	}
}
