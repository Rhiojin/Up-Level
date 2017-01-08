using UnityEngine;
using System.Collections;

public class stageMaker : MonoBehaviour {

	public GameObject tutorialCanvasGroup;
	public GameObject tutorialPlatform;
	public GameObject tutorialPlatformButt;
	public GameObject tutorialPlatformPunch;
	public GameObject tutorialPlatformRocket;
	public GameObject platform;
	public GameObject platformLong;
	public GameObject platformLongBreakables;
	public GameObject platformLongDonkeyCannon;
	public GameObject platformLongWater;

	public GameObject[] bonusRooms;

	public GameObject platformBossRoom;
	public GameObject Light2D;

	Vector3 spawnPoint = new Vector3(0, 10, 0);
	float xRange = 20;
	float delta = 45;
	float longPlatformDelta = 90; //100 - 10

	int floorCount = 0;
	int floorMax = 3; //4

	int lightCount = 0;
	int lightCountMax = 1;
	Vector3 lightSpawnpoint = new Vector3(-150,70,0);
	float lightSpawnDelta = 450;

	GameObject platformHolder;
	GameObject hazardHolder;
	int difficulty = 0;


	public GameObject[] groundHazards;
	public GameObject[] airHazards;
	public GameObject[] npcHazards;
	public GameObject[] aerialNpcHazards;
	public GameObject coinBunch;


	Vector3 groundHazardSpawnPoint = new Vector3(0, 10, 0);
	Vector3 airHazardSpawnPoint = new Vector3(0, 40, 0);
	Vector3 midSpawnPoint = new Vector3(0, 40, 0);
	int randomizer = 0;
	bool longSpawn = false;
	bool coinRoomSpawn = false;

	Vector3 originalSpawnPoint;
	Vector3 originalGroundSpawnPoint;
	Vector3 originalAirSpawnPoint;
	Vector3 originalMidSpawnPoint;



	void Start () 
	{
		//spawnPoint.x = Random.Range(-xRange, xRange);
//		Instantiate(platform, spawnPoint, platform.transform.rotation);

		originalSpawnPoint = spawnPoint;
		originalGroundSpawnPoint = groundHazardSpawnPoint;
		originalAirSpawnPoint = airHazardSpawnPoint;

		int p = PlayerPrefs.GetInt("playCount",0);
		if(p <= 5)//sync with menumanager
		{
			p++;
			PlayerPrefs.SetInt("playCount",p);
			InvokeRepeating("SpawnTutorialPlatform", 0.1f, 0.1f);
			Instantiate(tutorialCanvasGroup, Vector3.zero, tutorialCanvasGroup.transform.rotation);
		}
		else
		{
			difficulty = 1;
			InvokeRepeating("SpawnPlatform", 0.1f, 0.1f);
		}
		SpawnLight();
	}

	public void Reset()
	{
		spawnPoint = originalSpawnPoint;
		groundHazardSpawnPoint = originalGroundSpawnPoint;
		airHazardSpawnPoint = originalAirSpawnPoint;
		lightCount = 0;
		floorCount = 0;
	}

	void SpawnTutorialPlatform()
	{
		if(difficulty >= 5)
		{
			CancelInvoke("SpawnTutorialPlatform");
			InvokeRepeating("SpawnPlatform", 0.1f, 0.1f);
			return;
		}

		if(floorCount < floorMax)
		{
			longSpawn = false;
			spawnPoint.y += delta;

			switch(difficulty)
			{
			case(0):
				{
					platformHolder = Instantiate(tutorialPlatformRocket, spawnPoint, platform.transform.rotation) as GameObject;
				}
				break;
			case(1):
				{
					platformHolder = Instantiate(tutorialPlatform, spawnPoint, platform.transform.rotation) as GameObject;
				}
				break;
			case(2):
				{
					platformHolder = Instantiate(tutorialPlatformPunch, spawnPoint, platform.transform.rotation) as GameObject;
				}
				break;
			case(3):
				{
					platformHolder = Instantiate(tutorialPlatformButt, spawnPoint, platform.transform.rotation) as GameObject;
				}
				break;
			default:
				{
					platformHolder = Instantiate(platform, spawnPoint, platform.transform.rotation) as GameObject;
				}
				break;
			}



			floorCount++;
			difficulty++;

			groundHazardSpawnPoint.y += delta;
			airHazardSpawnPoint.y += delta;
		}



	}

	void SpawnPlatform()
	{

		if(floorCount < floorMax)
		{
			longSpawn = false;
			coinRoomSpawn = false;
			spawnPoint.y += delta;

			//decide and spawn platform based on difficulty

			int p = 0;
			if(difficulty%5 == 0)
			{
				// select a platform other than the first every 5 levels
				p = Random.Range(1,6);
			}
			else p = 0;

			switch(p)
			{
			case(0):
				{
					platformHolder = Instantiate(platform, spawnPoint, platform.transform.rotation) as GameObject;
					floorCount++;
					difficulty++;
					if(Random.value >= 0.3f)
					{
						Vector3 random = new Vector3(Random.Range(-30, 30),0,0);
						Instantiate(coinBunch,airHazardSpawnPoint+random,coinBunch.transform.rotation);
					}
				}
				break;

			case(1):
				{
					spawnPoint.y += longPlatformDelta;
					platformHolder = Instantiate(platformLong, spawnPoint, platform.transform.rotation) as GameObject;
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

			case(3): //donkey cannons
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

			case(4): //water room
				{
					spawnPoint.y += longPlatformDelta;
					platformHolder = Instantiate(platformLongWater, spawnPoint, platform.transform.rotation) as GameObject;
					floorCount++;

					difficulty++;
					spawnPoint.y += delta;
					groundHazardSpawnPoint.y += longPlatformDelta+delta;
					airHazardSpawnPoint.y += longPlatformDelta+delta;
					longSpawn = true;
				}
				break;

			case(5): //coin room
				{
					if(Random.value > 0.5f)
					{
						platformHolder = Instantiate(bonusRooms[Random.Range(0,bonusRooms.Length)], spawnPoint, platform.transform.rotation) as GameObject;
						floorCount++;
						difficulty++;
						coinRoomSpawn = true;
					}
					else
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

			if(!coinRoomSpawn)
			{
				//add npcs and hazards

				//Hazards
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

				}

				//NPCS

				int count = 1;
				if(difficulty<10)
				{
					count = 1;
				}
				else if(difficulty>11 && difficulty<20)
				{
					count = 2;
				}
				else if(difficulty>21)
				{
					count = 3;
				}
				SpawnNpc(count);

				groundHazardSpawnPoint.y += delta;
				airHazardSpawnPoint.y += delta;
			}

		}


	}

	void SpawnNpc(int count)
	{
		for(int i = 1; i <= count; i++)
		{
			if(Random.value > 0.75f)
			{
				//aerial npc
				randomizer = Random.Range(0,aerialNpcHazards.Length);
				Instantiate(aerialNpcHazards[randomizer], airHazardSpawnPoint+aerialNpcHazards[randomizer].transform.right, new Quaternion(0,0,0,0));
			}
			else
			{
				//ground npc
				randomizer = Random.Range(0,npcHazards.Length);
				airHazardSpawnPoint.x = Random.Range(-25, 25);
				hazardHolder = Instantiate(npcHazards[randomizer], airHazardSpawnPoint-Vector3.up, new Quaternion(0,0,0,0)) as GameObject;
			}

		}
	}

	void SpawnLight()
	{
		if(lightCount < lightCountMax)
		{
			print("spawning light");
			Instantiate(Light2D,lightSpawnpoint, Light2D.transform.rotation);
			lightSpawnpoint.x *= -1;
			lightSpawnpoint.y += lightSpawnDelta;
			lightCount++;

		}
	}

	public void MoveLight(Transform light)
	{
		print("moving light");
		light.transform.position = lightSpawnpoint;
		lightSpawnpoint.x *= -1;
		lightSpawnpoint.y += lightSpawnDelta;


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
			MoveLight(trig.transform.parent);
//			lightCount--;
		}
//		if(trig.CompareTag("longTrashTrigger"))
//		{
//			Destroy(trig.transform.parent.gameObject);
//			floorCount-=2;
//		}
		else if(!trig.CompareTag("eraser") && !trig.CompareTag("weightless"))
		{
			Destroy(trig.transform.gameObject);
		}


	}

	void OnCollisionEnter2D(Collision2D col)
	{
//		if(col.collider.CompareTag("trashTrigger"))
//		{
		if(!col.collider.CompareTag("eraser"))
			Destroy(col.gameObject);

//		}
	}
}
