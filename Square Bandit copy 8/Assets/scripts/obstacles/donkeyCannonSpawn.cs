using UnityEngine;
using System.Collections;

public class donkeyCannonSpawn : MonoBehaviour {

	public Transform[] spawnPoints;
	Vector3 adjustedPos = Vector3.zero;
	public GameObject donkeyCannonObj;

	void Start () 
	{
		for(int i = 0; i < spawnPoints.Length; i++)
		{
			adjustedPos = spawnPoints[i].position;
			adjustedPos.x = Random.Range(-20f,20f);
			spawnPoints[i].position = adjustedPos;
			Instantiate(donkeyCannonObj, spawnPoints[i].position, spawnPoints[i].rotation);
		}
	}

}
