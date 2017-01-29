using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Optimizer. simply destroys all unneeded objects on game start
/// </summary>

public class optimizer : MonoBehaviour {

	public GameObject[] objectsToOptimize;

	void Start () {
		
	}

	public void StartOptimize()
	{
		StartCoroutine( Optimize() );
	}

	IEnumerator Optimize()
	{
		for(int i = 0; i < objectsToOptimize.Length; i++)
		{
			Destroy(objectsToOptimize[i]);
			yield return new WaitForSeconds(0.2f);
		}	
	}
}
