using UnityEngine;
using System.Collections;
using System.Linq;

public class pcControl : MonoBehaviour {

	float moveSpeed = -20;
	float jumpForce = 3550;
	float weightLessForce = 42050;
	float boostForce = 5550;
	float stompForce = 4850;
	float maxGravity = 15;
	float actionThreshold = 40;
	float thisGravityScale = 15;

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
	bool ShoulderBashing = false;
	bool toggling = false;
	bool inDonkeyCannon = false;
	bool donkeyCannoning = false;
	bool weightless = false;
	bool isFacingRight = false;
	BoxCollider2D oneWayPlatform;

	Vector2 zeroVector = Vector2.zero;
	Vector2 upVector = Vector2.up;
	Vector2 forwardVector2 = Vector2.right;
	Vector2 bashForce = new Vector2(1550, 1200);

	Vector2 pauseVelocity = Vector2.zero;
	bool paused = false;

	public levelManager levelScript;
	public camControl camScript;
	public bool dead = false;
	public bool gameStarted = false;


	public Transform pcShadow;
	public SpriteRenderer pcShadowRenderer;
	Vector3 pcShadowScale;
	public Transform raycastPoint;

	RaycastHit2D hit;

	public TrailRenderer trail;
	public ParticleSystem rocketParticle;
	public ParticleSystem stompParticle;
	public ParticleSystem breakParticle;
	public ParticleSystem poofParticle;
	bool togglingTrail = false;

	Transform cannon;
	Collider2D cannonCollider;

	Touch touch;
	bool touchMoved = false;
	bool inputRegistered = false;
	Vector2 touchStart = Vector2.zero;
	Vector2 touchCurrent = Vector2.zero;
	Vector2 swipeDistance = Vector2.zero;
	float yStart;
	float yCurrent;

	public SpriteRenderer hat;
	public SpriteRenderer body;
	public SpriteRenderer face;
	public SpriteRenderer armsL;
	public SpriteRenderer armsR;
	public SpriteRenderer handsL;
	public SpriteRenderer handsR;
	public SpriteRenderer legsL;
	public SpriteRenderer legsR;
	public ParticleSystem particleRocket;
	public ParticleSystem particleStomp;
	public ParticleSystem particleBreak;
	public BoxCollider2D fistCollider;
	public Transform particlePosTop;
	public Transform particePosFront;

	Transform respawnTransform = null;


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

		LoadCharacterSkin();
		thisGravityScale = thisRigidbody.gravityScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!paused && !dead)
		{
//		if(!RocketBoosting && !ButtStomping)
//		{
			if(!donkeyCannoning) Move();
			else FreezePosition();
//		}

			if(gameStarted)GetInput();
		}
		 
		BoundsReposition();
		CastShadow();
	}

	void LoadCharacterSkin()
	{
		//pull character art from sprite sheet. Assigned in order of appearance
		string pathPrefix = "Skins/upLevel-character-";
		string skinName = PlayerPrefs.GetString("selectedSkin","viking");
		string fullPath = pathPrefix+skinName;
//		fullPath = "upLevel-character-viking";
		Sprite[] skinData = Resources.LoadAll<Sprite>(fullPath);

		body.sprite = skinData[0];
		hat.sprite = skinData[1];
		armsL.sprite = skinData[2];
		armsR.sprite = skinData[2];

		particleRocket.GetComponent<Renderer>().material.mainTexture = TextureFromSprite(skinData[3]);
		particleBreak.GetComponent<Renderer>().material.mainTexture = TextureFromSprite(skinData[3]);

		legsL.sprite = skinData[4];
		legsR.sprite = skinData[4];

		face.sprite = skinData[5];
		handsL.sprite = skinData[6];
		handsR.sprite = skinData[6];

		particleStomp.GetComponent<Renderer>().material.mainTexture = TextureFromSprite(skinData[7]);
		transistionCanvas.instance.ChangeImage(skinData[7]);

	}

	public void UpdateSkin(string name)
	{
		//pull character art from sprite sheet. Assigned in order of appearance
		string pathPrefix = "Skins/upLevel-character-";
		string fullPath = pathPrefix+name;
		PlayerPrefs.SetString("selectedSkin",name);
		Sprite[] skinData = Resources.LoadAll<Sprite>(fullPath);

		body.sprite = skinData[0];
		hat.sprite = skinData[1];
		armsL.sprite = skinData[2];
		armsR.sprite = skinData[2];

		particleRocket.GetComponent<Renderer>().material.mainTexture = TextureFromSprite(skinData[3]);
		particleBreak.GetComponent<Renderer>().material.mainTexture = TextureFromSprite(skinData[3]);

		legsL.sprite = skinData[4];
		legsR.sprite = skinData[4];

		face.sprite = skinData[5];
		handsL.sprite = skinData[6];
		handsR.sprite = skinData[6];

		particleStomp.GetComponent<Renderer>().material.mainTexture = TextureFromSprite(skinData[7]);
		transistionCanvas.instance.ChangeImage(skinData[7]);

	}

	Texture2D TextureFromSprite(Sprite sprite)
	{
//		Texture2D newTex = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
		Texture2D newTex = new Texture2D((int)sprite.textureRect.width,(int)sprite.textureRect.height);

		Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,(int)sprite.textureRect.y,(int)sprite.textureRect.width,(int)sprite.textureRect.height);
		newTex.SetPixels(newColors);
		newTex.Apply();
		return newTex;
	}

	public void PlayFootstep()
	{
		soundManager.instance.PlayStep();
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

		if(Input.GetKeyDown(KeyCode.A))
		{
			if(!ShoulderBashing) StartCoroutine(ShoulderBash());
		}

		#endif

		#if UNITY_ANDROID || UNITY_IOS || UNITY_METRO
		if(Input.touchCount > 0)
		{
			touch = Input.GetTouch(0);

//			if(touch.tapCount == 1)
//			{
//				if(!donkeyCannoning)SwapTravelDirection();
//				else  StartCoroutine( DonkeyCannonShot() );
//			}


			if(touch.phase == TouchPhase.Began)
			{
				touchMoved = false;
				inputRegistered = false;
				touchStart = touch.position;

			}

			if(touch.phase == TouchPhase.Moved)
			{
				if(!donkeyCannoning)
				{
//					touchMoved = true;
					touchCurrent = touch.position;
					if(!touchMoved && Vector2.Distance(touchStart, touchCurrent) > 10) 
					{
						touchMoved = true;
						inputRegistered = true;

						Vector2 v = touchCurrent - touchStart;
						if(Mathf.Abs(v.x) > Mathf.Abs(v.y))
						{
							if(touchCurrent.x > touchStart.x) 
							{
								if(isFacingRight && !weightless)
								{
									if(!ShoulderBashing)StartCoroutine( ShoulderBash() );
								}
								else
								{
									SwapTravelDirection();
								}
							}
							else
							{
								if(!isFacingRight)
								{
									if(!ShoulderBashing) StartCoroutine( ShoulderBash() );
								}
								else
								{
									SwapTravelDirection();
								}
							}
						}
						else
						{
							if(!ButtStomping && !canJump) StartCoroutine( ButtStomp() );
						}
					}

				}
			}

			if(touch.phase == TouchPhase.Ended)
			{
				if(touchMoved == false && !inputRegistered)
				{

					if(!donkeyCannoning)
					{
						//check water or space biome
						if(weightless)
						{
							WeightlessJump();
						}
						else if(canJump)
						{
							Jump();
						}
						else
						{
							if(!RocketBoosting)
							{
								StartCoroutine( RocketBoost() );
							}
						}						
					}
					else 
					{
						StartCoroutine( DonkeyCannonShot() );
					}

				}

//				else
//				{
//					Vector2 v = touchCurrent - touchStart;
//					if(Mathf.Abs(v.x) > Mathf.Abs(v.y))
//					{
//						if(touchCurrent.x > touchStart.x) 
//						{
//							if(isFacingRight && Mathf.Abs(touchCurrent.x - touchStart.x) > 5)
//							{
//								if(!ShoulderBashing)StartCoroutine( ShoulderBash() );
//							}
//							else
//							{
//								SwapTravelDirection();
//							}
//						}
//						else
//						{
//							if(!isFacingRight && Mathf.Abs(touchCurrent.x - touchStart.x) > 5)
//							{
//								if(!ShoulderBashing) StartCoroutine( ShoulderBash() );
//							}
//							else
//							{
//								SwapTravelDirection();
//							}
//						}
//					}
//					else
//					{
//					if(!ButtStomping && !canJump && Mathf.Abs(touchCurrent.y - touchStart.y) > 10) StartCoroutine( ButtStomp() );
//					}
//				}

			}
		}


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


	}

	void BoundsReposition()
	{
		if(transform.position.x > rightBounds.x+boundsEase) 
		{
			if(!togglingTrail && trail.enabled) StartCoroutine( ToggleTrail() );
			transform.position = new Vector3(leftBounds.x, transform.position.y, transform.position.z);

		}

		if(transform.position.x < leftBounds.x-boundsEase) 
		{
			if(!togglingTrail && trail.enabled) StartCoroutine( ToggleTrail() );
			transform.position = new Vector3(rightBounds.x+boundsEase, transform.position.y, transform.position.z);
		}
	}

	void FreezePosition()
	{
		transform.position = cannon.position;
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
		isFacingRight = !isFacingRight;
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
			if(weightless)
			{
				WeightlessJump();
			}
			else if(canJump)
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
//		anim.Play("pc_jump2");

		thisRigidbody.AddForce(upVector*jumpForce);
	}

	void WeightlessJump()
	{
		thisRigidbody.AddForce(upVector*weightLessForce);
	}

	IEnumerator RocketBoost()
	{
		trail.enabled = true;
		anim.Play("pc_RocketBoost");
		RocketBoosting = true;
		 
		rocketParticle.Play();

		thisRigidbody.velocity = zeroVector;
		thisRigidbody.AddForce(upVector*boostForce);

		yield return new WaitForSeconds(1);
//		anim.Play("pc_Jump");
		trail.enabled = false;
		rocketParticle.Stop();
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

	IEnumerator ShoulderBash()
	{
		ShoulderBashing = true;
		fistCollider.enabled = true;
		trail.enabled = true;
		anim.Play("pc_ShoulderBash");

		poofParticle.Emit(3);
		poofParticle.Stop();

		thisRigidbody.velocity = zeroVector;
		if(isFacingRight)
			thisRigidbody.AddForce(new Vector2(bashForce.x,bashForce.y));
		else thisRigidbody.AddForce(new Vector2(-bashForce.x,bashForce.y));

		yield return new WaitForSeconds(1);
		fistCollider.enabled = false;
		trail.enabled = false;
		ShoulderBashing = false;
	}

	void EnterDonkeyCannon()
	{
		inDonkeyCannon = true;
		donkeyCannoning = true;
		thisRigidbody.gravityScale = 0;
		thisRigidbody.velocity = zeroVector;

		transform.position = cannon.position;
		anim.Play("pc_Jump");


	}

	IEnumerator DonkeyCannonShot()
	{
		inDonkeyCannon = false;
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

		stompParticle.Emit(10);
		stompParticle.Stop();	

		//CHANGE DEATH TO BE LIKE HHN
		//play death animation
		if((PlayerPrefs.GetInt("coins",0) >= 100 || AdManager.AdColony_AdAvailable())&& respawnTransform != null)
		{
			levelScript.ShowRespawnMenu();
		}

		else 
		{
//			stompParticle.transform.parent = null;
			levelScript.GameOver();
		}
	}



	void SaveForRespawn(Transform t)
	{
		respawnTransform = t;
	}

	public void Respawn()
	{
		StartCoroutine( ActualRespawn() );
	}

	IEnumerator ActualRespawn()
	{
		yield return new WaitForSeconds(1);
		transform.position = respawnTransform.position+Vector3.up*3;
		anim.Play("pc_Run");
		dead = false;
	}

	public void DontRespawn()
	{
		levelScript.GameOver();
	}

	void OnCollisionEnter2D(Collision2D colEnt)
	{
		if(colEnt.collider.CompareTag("ground"))
		{
			if(ButtStomping) camScript.StartCamRock(transform.position.x);
			if(!canJump && !weightless || ShoulderBashing)
			{
				anim.Play("pc_Run");
				SaveForRespawn(colEnt.transform);
			}
			canJump = true;
			RocketBoosting = false;
			if(ButtStomping) 
			{
				stompParticle.Emit(5);
				stompParticle.Stop();	
			}
			ButtStomping = false;

		}

		if(colEnt.collider.CompareTag("enemy"))
		{
			if(ButtStomping || RocketBoosting || ShoulderBashing)
			{
				if(ButtStomping)
				{
					camScript.StartCamRock(transform.position.x);
					stompParticle.Emit(5);
					stompParticle.Stop();	

				}
				else if(RocketBoosting)
				{
					camScript.StartScreenShake();

					breakParticle.transform.position = colEnt.transform.position;
					breakParticle.Emit(10);
					breakParticle.Stop();

				}
				else if(ShoulderBashing)
				{
					camScript.StartCamRock(transform.position.x);
					breakParticle.transform.position = colEnt.transform.position;
					breakParticle.Emit(10);
					breakParticle.Stop();
				}
				ButtStomping = false;
				//RocketBoosting = false;
				//rocketParticle.Stop();
				Destroy(colEnt.collider.gameObject);
				if(!ShoulderBashing)
				{
					thisRigidbody.velocity = zeroVector;
					thisRigidbody.AddForce(upVector*jumpForce);
				}
				levelScript.Scored(10);
			}
			else 
			{
				breakParticle.transform.position = colEnt.transform.position;
				breakParticle.Emit(10);
				breakParticle.Stop();

				Death();
			}
		}

		if(colEnt.collider.CompareTag("hazard"))
		{
			breakParticle.transform.position = colEnt.transform.position;
			breakParticle.Emit(10);
			breakParticle.Stop();

			Death();
		}

		if(colEnt.collider.CompareTag("breakable"))
		{
			if(ButtStomping)
			{
				RocketBoosting = false;
			//	rocketParticle.Stop();
				Destroy(colEnt.collider.gameObject);
				thisRigidbody.velocity = zeroVector;
				thisRigidbody.AddForce(upVector*jumpForce/1.5f);
				camScript.StartCamRock(transform.position.x);
				stompParticle.Emit(5);
				stompParticle.Stop();
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

				breakParticle.transform.position = colEnt.transform.position;
				breakParticle.Emit(5);
				breakParticle.Stop();

			}

			if(ShoulderBashing)
			{
				Destroy(colEnt.collider.gameObject);
				
				camScript.StartCamRock(transform.position.x);
				breakParticle.transform.position = colEnt.transform.position;
				breakParticle.Emit(10);
				breakParticle.Stop();
			}

			if(!ButtStomping && !canJump)
			{
				anim.Play("pc_Run");
				canJump = true;
				RocketBoosting = false;
			}
			if(ButtStomping || RocketBoosting || ShoulderBashing)
			{
				if(Random.value > 0.6f)
				{
					levelScript.MakeACoin(colEnt.collider.transform.position);
				}
				levelScript.Scored(2);
			}
		
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

			breakParticle.transform.position = col.transform.position;
			breakParticle.Emit(10);
			breakParticle.Stop();

			EnterDonkeyCannon();
		}

		if(col.CompareTag("coin"))
		{
			soundManager.instance.PlayClip("coinSound", 0.5f);
			Destroy(col.gameObject);
			levelScript.GainCoinsIngame(1);

		}

		if(col.CompareTag("weightless"))
		{
			thisRigidbody.velocity = Vector3.zero;
			thisRigidbody.AddForce(upVector*weightLessForce);
			thisRigidbody.AddForce(upVector*weightLessForce);
			if(weightless == false)
			{
				weightless = true;
				thisRigidbody.gravityScale = thisRigidbody.gravityScale*0.2f;
				thisRigidbody.mass = 55;
				anim.Play("pc_Swim");
				anim.speed = 0.4f;
			}
		}

//		if(col.CompareTag("weightlessEnd"))
//		{
//			if(weightless == true)
//			{
//				weightless = false;
//				thisRigidbody.velocity = Vector3.zero;
//				thisRigidbody.gravityScale = thisGravityScale;
//				thisRigidbody.mass = 1;
//				anim.speed = 1;
//				thisRigidbody.velocity *= 0.5f;
//			}
//		}
	}
	void OnTriggerStay2D(Collider2D col)
	{
		if(col.CompareTag("oneWayTrigger"))
		{
			oneWayPlatform = col.transform.parent.GetComponent<BoxCollider2D>();
			if(oneWayPlatform.enabled == true) oneWayPlatform.enabled = false;
		}

		if(col.CompareTag("weightless"))
		{
			if(weightless == false)
			{
				print("aerae0");
				weightless = true;
				thisRigidbody.gravityScale = thisRigidbody.gravityScale*0.2f;
				thisRigidbody.mass = 55;
				anim.Play("pc_Swim");
				anim.speed = 0.3f;
			}
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

		if(col.CompareTag("weightless"))
		{
			
			if(weightless == true)
			{
				weightless = false;
				//thisRigidbody.velocity = Vector3.zero;
				thisRigidbody.gravityScale = thisGravityScale;
				thisRigidbody.mass = 1;
				anim.speed = 1;
				thisRigidbody.velocity *= 0.5f;
			}
		}
	}

}
