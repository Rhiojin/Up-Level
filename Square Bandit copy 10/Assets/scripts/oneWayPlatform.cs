using UnityEngine;
using System.Collections;

public class oneWayPlatform : MonoBehaviour {

	public Collider2D pcCollider;
	public Collider2D parentCollider;
	void Start () {

		pcCollider = GameObject.Find("pc").GetComponent<Collider2D>();
	//	parentCollider = transform.parent.GetComponent<Collider2D>();
//		print("22k");
//		Physics2D.IgnoreCollision(pcCollider, parentCollider, true);
//		parentCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("pc"))
		{
			//print("22k");
			Physics2D.IgnoreCollision(pcCollider, parentCollider, true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.CompareTag("pc"))
		{
			//print("99k");
			Physics2D.IgnoreCollision(pcCollider, parentCollider, false);
			parentCollider.enabled = true;
		}
	}
}
