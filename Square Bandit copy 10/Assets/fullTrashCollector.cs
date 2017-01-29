using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullTrashCollector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		Destroy(col.gameObject);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy(col.gameObject);
	}
}
