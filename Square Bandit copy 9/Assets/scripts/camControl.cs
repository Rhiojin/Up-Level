using UnityEngine;
using System.Collections;

public class camControl : MonoBehaviour {

	public Transform target;
//	public Rigidbody2D targetRigidbody;
	public Vector3 targetPos;
//	float minHeight = 3f;
//	float maxHeight = 20;
	float lerpSpeed = 2;
	float yOffset = 15;
	float xClamp = 8; //was 20

//	float baseOrthoSize = 7.5f;
//	float maxOrthoSize = 15f;
//	float targetOrthoSize;
//	float orthoSizePercent = 0.5f;
//	float zoomLerpSpeed = 5;

	Vector3 zeroVector = Vector3.zero;

	bool shaking = false;
	bool camRock = false;
	float shakeTimerMax = 0.4f;
	float shakeTimer = 0.4f;
	float shakeRotationLimits = 3;
	float shakeYlimits = 1.5f;
	float shakeXlimits = 1.5f;
	float shakeDelay = 0.08f;
	//	float shakeForce = 1;
	Vector3 originalPos = Vector3.zero;
	Vector3 targetShakePos = Vector3.zero;
	Vector3 targetShakeRotation = Vector3.zero;
	Vector3 originalRotation;
	Vector3 rockRotation;

	Camera thisCam;
	Transform soundManagerObj;

	float camRockTimeOut = 0;

	void Start () 
	{
		targetPos = transform.position;
//		targetRigidbody = target.GetComponent<Rigidbody2D>();
//		GameObject.Find("levelManager").GetComponent<levelManager>().playerDidDie += PlayerDied;
		thisCam = GetComponent<Camera>();
		rockRotation = zeroVector;

		soundManagerObj = GameObject.Find("SoundManager").transform;
	}


	// Update is called once per frame
	void LateUpdate () 
	{
		if(target)
		{
			//positionCamer
//			targetPos.x = Mathf.Clamp(target.position.x,-xClamp, xClamp);
			targetPos.y = target.position.y+yOffset;
//			targetPos.y = Mathf.Clamp(target.position.y,minHeight, maxHeight);
			if(transform.position.y - yOffset < target.transform.position.y)
			{
				transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed*0.35f*Time.deltaTime);
			}

			//updagte zoom to contain pc and objects as much as possible
			//			targetOrthoSize = baseOrthoSize + (Mathf.Abs(0+target.position.x) * orthoSizePercent);
//			targetOrthoSize = baseOrthoSize + targetRigidbody.velocity.magnitude * orthoSizePercent;

//			if(targetOrthoSize > 15) targetOrthoSize = 15;
//			thisCam.orthographicSize = Mathf.Lerp(thisCam.orthographicSize, targetOrthoSize, zoomLerpSpeed*Time.deltaTime);
		}

		if(shaking)
		{
			Shake();
		}

		if(camRock)
		{
			rockRotation.z = Mathf.MoveTowards(rockRotation.z, 0, 50*Time.deltaTime);
			transform.eulerAngles = rockRotation;
			transform.position = Vector3.Lerp(transform.position, originalPos, lerpSpeed*2*Time.deltaTime);
			camRockTimeOut += Time.deltaTime;

			if( Mathf.Abs( transform.position.y - originalPos.y) < 1.2f || camRockTimeOut >= 2)
			{
				transform.eulerAngles = zeroVector;
				transform.position = originalPos;
				camRock = false;
				camRockTimeOut = 0;
			}
		}

		soundManagerObj.transform.position = transform.position;
	}

	void PlayerDied()
	{
//		target = null;
	}

	public void StartScreenShake()
	{
		shakeTimer = shakeTimerMax;
		shaking = true;
		targetShakePos.x = Random.Range(-shakeXlimits*shakeTimer, shakeXlimits*shakeTimer);
		targetShakePos.y = Random.Range(-shakeYlimits*shakeTimer, shakeYlimits*shakeTimer);
		transform.position += targetShakePos;

//		targetShakeRotation.z = Random.Range(-shakeRotationLimits*shakeTimer, shakeRotationLimits*shakeTimer);
//		transform.eulerAngles = targetShakeRotation;

	}

	public void StartCamRock(float dist)
	{
		dist = Mathf.Clamp(dist,-8,8);

		if(dist <= 0)
		{
			if(dist > -2) dist = -2;
		}

		if(dist > 0)
		{
			if(dist < 2) dist = 2;
		}

		camRock = true;
		if(dist <0 )
			rockRotation.z = dist;
		else
			rockRotation.z = dist;

		originalPos = transform.position;
		transform.eulerAngles = rockRotation;
		transform.position -= -Vector3.up*2;

	}
	public void Shake()
	{
		targetShakePos.x = Random.Range(-shakeXlimits*shakeTimer, shakeXlimits*shakeTimer);
		targetShakePos.y = Random.Range(-shakeYlimits*shakeTimer, shakeYlimits*shakeTimer);
		transform.position += targetShakePos;

//		targetShakeRotation.z = Random.Range(-shakeRotationLimits*shakeTimer, shakeRotationLimits*shakeTimer);
//		transform.eulerAngles = targetShakeRotation;

		shakeTimer -= Time.deltaTime;
		if(shakeTimer <= 0)
		{
			shaking = false;
			targetShakeRotation.z = 0;
			transform.eulerAngles = targetShakeRotation;
		}
	}
}
