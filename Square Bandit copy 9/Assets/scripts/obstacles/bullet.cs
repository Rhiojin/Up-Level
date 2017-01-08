using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	public float movespeed = 15;
	levelManager levelScript;
	void Start () {
	
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();
		Destroy(gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!levelScript.paused)
		{
			transform.Rotate(0,0,-360*Time.deltaTime);
			transform.position += Vector3.right*movespeed*Time.deltaTime;
		}
	}
}
