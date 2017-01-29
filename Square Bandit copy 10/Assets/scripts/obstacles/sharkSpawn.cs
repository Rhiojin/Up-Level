using UnityEngine;
using System.Collections;

public class sharkSpawn : MonoBehaviour {

	public Transform[] sharkPoints;
	public GameObject shark;

	void Start () 
	{
		for(int i = 0; i < sharkPoints.Length; i++)
		{
			Instantiate(shark, sharkPoints[i].position, sharkPoints[i].rotation);
		}
	}
}
