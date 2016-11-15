using UnityEngine;
using System.Collections;

public class platformScript : MonoBehaviour {


//	public Transform light;
	public SpriteRenderer bg;

	public bool longPlatform = false;

	levelManager levelScript;


	void Awake () 
	{
		levelScript = GameObject.Find("level manager").GetComponent<levelManager>();


//		light.localPosition = new Vector3(Random.Range(-0.9f,0.9f),light.localPosition.y, light.localPosition.z);

		bg.color = levelScript.BGcolors[Random.Range(0,levelScript.BGcolors.Length)];

		if(!longPlatform)
		{
			if(Random.value > 0.08f)
			{
				int r = Random.Range(0,levelScript.bgWalls.Length);


				if(r == 1)
				{
					if(Random.value >0.5f) 
						bg.transform.localScale = new Vector3(0.6f,bg.transform.localScale.y,bg.transform.localScale.z);
					else
						bg.transform.localScale = new Vector3(-0.6f,bg.transform.localScale.y,bg.transform.localScale.z);

				}
				//bgwalls set
				bg.sprite = levelScript.bgWalls[r];

			}
		}



	}
	

}
