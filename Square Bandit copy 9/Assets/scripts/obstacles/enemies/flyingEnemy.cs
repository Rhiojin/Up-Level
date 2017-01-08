using UnityEngine;
using System.Collections;

public class flyingEnemy : MonoBehaviour {

	public Transform nose;
	public Transform body1;
	public Transform body2;
	public Transform tail;
	public Transform wing1;
	float amplitude = 0.6f;
	float frequency = 0.8f;
	float lerpSpeed = 7;
	float moveSpeed = 16;
	float rotateSpeed = 300;
	float maxAngle = -80;
	Vector3 magnitude;

	Vector3 body1TargetPos;
	Vector3 body2TargetPos;
	Vector3 tailTargetPos;

	Vector3 leftBounds;
	Vector3 rightBounds;
	float boundsEase = 10.5f;

	public bool isShark = false;

	void Start () 
	{
		body1TargetPos = body1.localPosition;
		body2TargetPos = body2.localPosition;
		tailTargetPos = tail.localPosition;

		if(Random.value > 0.5f)
		{
			moveSpeed *= -1;
			Vector3 s = transform.localScale;
			s.x *= -1;
			transform.localScale = s;
		}
			
		leftBounds = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
		rightBounds = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));
	}

	void Update () 
	{
		Animate();
		Move();
	}

	void Animate()
	{
		magnitude = amplitude*(Mathf.Sin(2*Mathf.PI*frequency*Time.time) - Mathf.Sin(2*Mathf.PI*frequency*(Time.time - Time.deltaTime)))*nose.right;
		nose.localPosition += magnitude;

		body1TargetPos.y = Mathf.Lerp(body1.localPosition.y, nose.localPosition.y, lerpSpeed*Time.deltaTime);
		body1.localPosition = body1TargetPos;

		body2TargetPos.y = Mathf.Lerp(body2.localPosition.y, body1.localPosition.y, lerpSpeed*Time.deltaTime);
		body2.localPosition = body2TargetPos;

		tailTargetPos.y = Mathf.Lerp(tail.localPosition.y, body2.localPosition.y, lerpSpeed*Time.deltaTime);
		tail.localPosition = tailTargetPos;

		if(!isShark)wing1.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.PingPong(Time.time * rotateSpeed, 120.0f)+270.0f);
	}

	void Move()
	{
		transform.position += transform.right * moveSpeed * Time.deltaTime;

		if(transform.position.x > rightBounds.x+boundsEase) 
		{
			transform.position = new Vector3(leftBounds.x, transform.position.y, transform.position.z);
		}

		if(transform.position.x < leftBounds.x-boundsEase) 
		{
			transform.position = new Vector3(rightBounds.x+boundsEase, transform.position.y, transform.position.z);
		}
	}
}
