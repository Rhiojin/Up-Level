using UnityEngine;
using System.Collections;

public class eraser : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//		if(col.collider.CompareTag("trashTrigger"))
		//		{
		if(col.gameObject != gameObject)
			Destroy(col.gameObject);

		//		}
	}

	void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.gameObject != gameObject)
			Destroy(trig.transform.gameObject);

	}

}
