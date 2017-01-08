using UnityEngine;
using System.Collections;

public class waveMasterController : MonoBehaviour {

	Vector3 targetPos;

	void Start () 
	{
		targetPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		targetPos.x = Mathf.Repeat(Time.time, 2.5f);
		transform.localPosition = targetPos;
	}
}
