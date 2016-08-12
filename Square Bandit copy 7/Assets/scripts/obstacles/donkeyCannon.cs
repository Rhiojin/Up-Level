using UnityEngine;
using System.Collections;

public class donkeyCannon : MonoBehaviour {

	public Transform arrow;
	Vector3 arrowVector = Vector3.zero;
	float timeSeed = 0;
	float amplitude = 0.5f; // higher is bounce length
	float frequency = 0.5f; //lower is faster bounce
	float startPos;
	float rotationDirection = 120;
		

	void Start () 
	{
//		timeSeed = Random.v
		arrowVector = arrow.localPosition;
		startPos = arrowVector.x;
		if(Random.value > 0.5f)
		{
			rotationDirection *= -1;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0,0, rotationDirection*Time.deltaTime);
		Bobbing();
	}

	void Bobbing()
	{
		arrowVector.x = startPos +  Mathf.SmoothStep(0,amplitude, Mathf.PingPong((Time.time+timeSeed)/frequency,1));
//		arrowVector.x += moveSpeed*Time.deltaTime;
		arrow.transform.localPosition = arrowVector;
	}
}
