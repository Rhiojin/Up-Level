using UnityEngine;
using System.Collections;

public class spikeHazardSpawn : MonoBehaviour {

	public Transform[] spawnPoints;
	public GameObject spikeHazardObj;

	void Start () 
	{
		for(int i = 0; i < spawnPoints.Length; i++)
		{
			Instantiate(spikeHazardObj, spawnPoints[i].position, spawnPoints[i].rotation);
		}
	}
}
