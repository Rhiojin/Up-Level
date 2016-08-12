using UnityEngine;
using System.Collections;

public class pcControl : MonoBehaviour {

	float moveSpeed = -20;
	float jumpForce = 3550;
	float boostForce = 5550;
	float stompForce = 4850;
	float maxGravity = 15;
	float actionThreshold = 40;

	Vector3 leftBounds;
	Vector3 rightBounds;
	float boundsEase = 1.5f;

//	float xRange = 29.5f;

	Vector3 scale = new Vector3(1,1,1);
	Vector3 forwardVector = new Vector3(1,0,0);
	public Animator anim;
	public Rigidbody2D thisRigidbody;
	public GameObject thisBody;
	public Collider2D thisCollider;
	bool canJump = true;
	Vector2 screenSpacePos = Vector2.zero;
	bool RocketBoosting = false;
	bool ButtStomping = false;
	bool toggling = false;
	bool donkeyCannoning = false;
	BoxCollider2D oneWayPlatform;

	Vector2 zeroVector = Vector2.zero;
	Vector2 upVector = Vector2.up;

	Vector2 pauseVelocity = Vector2.zero;
	bool paused = false;

	public levelManager levelScript;
	public camControl camScript;
	public bool dead = false;

	public Transform pcShadow;
	public SpriteRenderer pcShadowRenderer;
	Vector3 pcShadowScale;
	public Transform raycastPoint;

	RaycastHit2D hit;

	public TrailRenderer trail;
	public ParticleSystem rocketParticle;
	public ParticleSystem stompParticle;
	public ParticleSystem breakParticle;
	bool togglingTrail = false;

	Transform cannon;
	Collider2D cannonCollider;

	Touch touch;
	bool touchMoved = false;
	Vector2 touchStart = Vector2.zero;
	Vector2 touchCurrent = Vector2.zero;
	Vector2 swipeDistance = Vector2.zero;


	void Start () 
	{
		levelScript._didPause += Pause;
		levelScript._didUnpause += Unpause;

		leftBounds = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
		rightBounds = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));

		thisCollider = GetComponent<Collider2D>();
		
		scale = transform.localScale;

		//face right
		SwapTravelDirection();
		pcShadowScale = pcShadow.localScale;

		trail.sortingLayerName = "pc";
		trail.enabled = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!paused && !dead)
		{
//		if(!RocketBoosting && !ButtStomping)
//		{
			if(!donkeyCannoning) Move();
//		}

			GetInput();
		}
		 
		CastShadow();
	}


	public void Pause()
	{
		pauseVelocity = thisRigidbody.velocity;
		thisRigidbody.velocity = zeroVector;
		thisRigidbody.gravityScale = 0;
		paused = true;
	}

	public void Unpause()
	{
		thisRigidbody.velocity = pauseVelocity;
		thisRigidbody.gravityScale = maxGravity;
		paused = false;
	}

	void GetInput()
	{
		#if UNITY_EDITOR
		//change direction
		if(Input.GetMouseButtonDown(0))
		{
			if(!donkeyCannoning)SwapTravelDirection();
			else  StartCoroutine( DonkeyCannonShot() );
		}

		//jump
		if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
		{
			if(!donkeyCannoning)ActionButton();
			else  StartCoroutine( DonkeyCannonShot() );

//			if(!RocketBoosting)
//			{
//				if(thisRigidbody.velocity.y > 0)
//					StartCoroutine( RocketBoost() );
//			}
//			if(!ButtStomping)
//			{
//				if(thisRigidbody.velocity.y <= 0)
//					StartCoroutine( ButtStomp() );
//			}
		}
		#endif

		#if UNITY_ANDROID || UNITY_IOS
//		if(Input.touchCount > 0)
//		{
//			touch = Input.GetTouch(0);
//
////			if(touch.tapCount == 1)
////			{
////				if(!donkeyCannoning)SwapTravelDirection();
////				else  StartCoroutine( DonkeyCannonShot() );
////			}
//
//
//			if(touch.phase == TouchPhase.Began)
//			{
//				touchMoved = false;
//				touchStart = touch.position;
//
//			}
//
//			if(touch.phase == TouchPhase.Moved)
//			{
//				if(!donkeyCannoning)
//				{
//					touchMoved = true;
//					touchCurrent = touch.position;
//
////					if(Vector2.Distance(touchCurrent, touchStart) > 10)
////					{
//						
//
////					}
//				}
//			}
//
//			if(touch.phase == TouchPhase.Ended)
//			{
//				if(touchMoved == false)
//				{
//
//
//					if(!donkeyCannoning)SwapTravelDirection();
//					else  StartCoroutine( DonkeyCannonShot() );
//
//
//				}
//
//				else
//				{
//					if(Mathf.Abs(touchCurrent.y) > Mathf.Abs(touchStart.y))
//					{
//						if(canJump)
//						{
//							Jump();
//						}
//						else 
//						{
//							if(!RocketBoosting && Mathf.Abs(touchCurrent.y) > Mathf.Abs(touchStart.y))
//							{
//								StartCoroutine( RocketBoost() );
//							}
//							else
//							{
//								if(!ButtStomping)
//									StartCoroutine( ButtStomp() );
//
//							}
//						}
//
//					}
//
//				}
//			}
//		}
//

		#endif
	}


	void Move()
	{
		transform.position += forwardVector*moveSpeed*Time.deltaTime;
//		if(transform.position.x > xRange) 
//		{
//			transform.position = new Vector3(-xRange+1, transform.position.y, transform.position.z);
//		}
//
//		if(transform.position.x < -xRange) 
//		{
//			transform.position = new Vector3(xRange-1, transform.position.y, transform.position.z);
//		}

		if(transform.position.x > rightBounds.x+boundsEase) 
		{
			if(!togglingTrail && trail.enabled) StartCoroutine( ToggleTrail() );
			transform.position = new Vector3(leftBounds.x-boundsEase, transform.position.y, transform.position.z);

		}

		if(transform.position.x < leftBounds.x-boundsEase) 
		{
			if(!togglingTrail && trail.enabled) StartCoroutine( ToggleTrail() );
			transform.position = new Vector3(rightBounds.x+boundsEase, transform.position.y, transform.position.z);
		}
	}

	IEnumerator ToggleTrail()
	{
		togglingTrail = true;

		trail.enabled = false;
		yield return new WaitForSeconds(0.4f);
		trail.enabled = true;

		togglingTrail = false;
	}

	public void SwapTravelDirection()
	{
		moveSpeed *= -1;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void ActionButton()
	{

		if(donkeyCannoning)
		{
			StartCoroutine( DonkeyCannonShot() );
		}

		else 
		{
			if(canJump)
			{
				Jump();
			}
			else 
			{
	//			print(thisRigidbody.velocity.y);
				if(thisRigidbody.velocity.y > actionThreshold)
				{
					if(!RocketBoosting)
						StartCoroutine( RocketBoost() );
				}
				else 
				{
					if(!ButtStomping)
						StartCoroutine( ButtStomp() );
				}
			}
		}
	}
		

	void Jump()
	{
		canJump = false;
		anim.Play("pc_Jump");
		thisRigidbody.AddForce(upVector*jumpForce);
	}

	IEnumerator RocketBoost()
	{
		trail.enabled = true;
		anim.Play("pc_RocketBoost");
		RocketBoosting = true;
		 
		rocketParticle.Emit(5);

		thisRigidbody.velocity = zeroVector;
		thisRigidbody.AddForce(upVector*boostForce);

		yield return new WaitForSeconds(1);
//		anim.Play("pc_Jump");
		trail.enabled = false;
		//thisCollider.isTrigger = false;
		//RocketBoosting = false;
	}

	IEnumerator ButtStomp()
	{
		anim.Play("pc_buttStomp");
		ButtStomping = true;
		trail.enabled = true;
		thisRigidbody.velocity = zeroVector;
		thisRigidbody.gravityScale = maxGravity/2.5f;

		yield return new WaitForSeconds(0.25f);
		thisRigidbody.gravityScale = maxGravity;
		thisRigidbody.AddForce(-upVector*stompForce);

		yield return new WaitForSeconds(0.35f);
		trail.enabled = false;

		//thisCollider.isTrigger = false;
		//ButtStomping = false;
	}

	void EnterDonkeyCannon()
	{
		thisRigidbody.gravityScale = 0;
		thisRigidbody.velocity = zeroVector;
		donkeyCannoning = true;
		breakParticle.Emit(10);
//		thisBody.SetActive(false);
		transform.position = cannon.position;
//		anim.Play("pc_DonkeyCannon");
//		transform.parent = cannon;

	}
	IEnumerator DonkeyCannonShot()
	{
		donkeyCannoning = false;
		rocketParticle.Play();
		thisBody.SetActive(true);
		RocketBoosting = true;

		cannonCollider.enabled = false;
		thisRigidbody.AddForce(cannon.rotation*Vector2.right*boostForce);
		thisRigidbody.gravityScale = maxGravity;
		yield return new WaitForSeconds(0.3f);
		cannonCollider.enabled = true;

		yield return new WaitForSeconds(1.7f);
		rocketParticle.Stop();
		RocketBoosting = false;
//		anim.Play("pc_Jump");
	}

	IEnumerator TogglePlatform(BoxCollider2D _col)
	{
		_col.enabled = false;

		yield return new WaitForSeconds(0.51f);

		if(_col != null) _col.enabled = true;
		toggling = false;

	}





	void CastShadow()
	{
		hit = Physics2D.Raycast(raycastPoint.position, -Vector2.up);
		if(hit.collider != null)
		{
			float dist = Mathf.Abs(hit.point.y - raycastPoint.position.y);
			if(dist > 2)
			{
				if(pcShadowRenderer.enabled == true) pcShadowRenderer.enabled = false;
			}
			else if(dist <=2)
			{
				if(pcShadowRenderer.enabled == false) pcShadowRenderer.enabled = true;
				pcShadowScale.x = 0.75f * (2-dist);
				pcShadow.localScale = pcShadowScale;
				pcShadow.position = hit.point;
			}
		}
	}

	void Death()
	{
		dead = true;
		levelScript.GameOver();
	}


	void OnCollisionEnter2D(Collision2D colEnt)
	{
		if(colEnt.collider.CompareTag("ground"))
		{
			if(ButtStomping) camScript.StartCamRock(transform.position.x);
//			else if(RocketBoosting) camScript.StartScreenShake();
			anim.Play("pc_Run");
			canJump = true;
			RocketBoosting = false;
			//rocketParticle.Stop();
			if(ButtStomping) stompParticle.Emit(5);
			ButtStomping = false;

		}

		if(colEnt.collider.CompareTag("enemy"))
		{
			//	print("eter");
//			anim.Play("pc_Run");
//			canJump = true;
//			RocketBoosting = false;
			if(ButtStomping || RocketBoosting)
			{
				if(ButtStomping)
				{
					camScript.StartCamRock(transform.position.x);
					stompParticle.Emit(5);

				}
				else if(RocketBoosting)
				{
					camScript.StartScreenShake();
					breakParticle.Emit(5);
				}
				ButtStomping = false;
				//RocketBoosting = false;
				//rocketParticle.Stop();
				Destroy(colEnt.collider.gameObject);
				thisRigidbody.velocity = zeroVector;
				thisRigidbody.AddForce(upVector*jumpForce);
				levelScript.Scored(10);
			}
			else 
			{
				breakParticle.Emit(15);
				Death();
			}
		}

		if(colEnt.collider.CompareTag("hazard"))
		{
			breakParticle.Emit(15);
			Death();
		}

		if(colEnt.collider.CompareTag("breakable"))
		{
			if(ButtStomping)
			{
				ButtStomping = false;
				RocketBoosting = false;
			//	rocketParticle.Stop();
				Destroy(colEnt.collider.gameObject);
				thisRigidbody.velocity = zeroVector;
				thisRigidbody.AddForce(upVector*jumpForce);
				camScript.StartCamRock(transform.position.x);
				stompParticle.Emit(5);
			}

			if(RocketBoosting)
			{
				//ButtStomping = false;
				//RocketBoosting = false;
				camScript.StartScreenShake();
				Destroy(colEnt.collider.gameObject);
				thisRigidbody.velocity = zeroVector;
				anim.Play("pc_Jump");
				trail.enabled = false;
//				thisRigidbody.AddForce(-upVector*jumpForce);
				breakParticle.Emit(5);
			}

			if(!canJump)
			{
				//ButtStomping = false;
				//RocketBoosting = false;
				Destroy(colEnt.collider.gameObject);
				thisRigidbody.velocity = zeroVector;
				//				thisRigidbody.AddForce(-upVector*jumpForce);
			}
			levelScript.Scored(2);
		
		}



	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("oneWayTrigger"))
		{
			//print("@Q#");
			if(!toggling)
			{
				toggling = true;
				StartCoroutine( TogglePlatform(col.transform.parent.GetComponent<BoxCollider2D>()) );
			}
		}

		if(col.CompareTag("deathTrigger"))
		{
			thisRigidbody.velocity = zeroVector;
			thisRigidbody.gravityScale = 0;
			Death();
		}

		if(col.CompareTag("floorPoints5"))
		{
			levelScript.Scored(5);
			StartCoroutine( levelScript.Flicker(col.GetComponent<SpriteRenderer>()) );
		}

		if(col.CompareTag("floorPoints20"))
		{
			levelScript.Scored(20);
			StartCoroutine( levelScript.Flicker(col.GetComponent<SpriteRenderer>()) );

		}

		if(col.CompareTag("donkeyCannon"))
		{
			cannon = col.gameObject.transform;
			cannonCollider = col;
			EnterDonkeyCannon();
		}
	}
	void OnTriggerStay2D(Collider2D col)
	{
		if(col.CompareTag("oneWayTrigger"))
		{
			oneWayPlatform = col.transform.parent.GetComponent<BoxCollider2D>();
			if(oneWayPlatform.enabled == true) oneWayPlatform.enabled = false;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.CompareTag("oneWayTrigger"))
		{
			if(oneWayPlatform != null)
			{
				oneWayPlatform.enabled = true;
				oneWayPlatform = null;
			}
		}
	}

}
