using UnityEngine;
using System.Collections;

public class breakablePlatform : MonoBehaviour {

	bool activated = false;
	Vector3 originalPos;
	Vector3 shakePos;
	float timer = 2;
	float shakeRange = 0.008f;

	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(activated)
		{
			shakePos.x = Random.Range(-shakeRange, shakeRange);
			shakePos.y = Random.Range(-shakeRange, shakeRange);

			transform.localPosition = originalPos+shakePos;
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.CompareTag("Player"))
		{
			shakePos = transform.localPosition;
			originalPos = shakePos;
			activated = true;
		}
	}
}
