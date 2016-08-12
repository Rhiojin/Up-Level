using UnityEngine;
using System.Collections;

public class platformDecorator : MonoBehaviour {

	public Transform[] decorObjects;
	public SpriteRenderer bg;
	Vector3 pos;
	void Start () 
	{
		for(int i = 0; i < decorObjects.Length;i++)
		{
			pos = decorObjects[i].localPosition;
			pos.x = Random.Range(0,14);
			decorObjects[i].localPosition = pos;
			decorObjects[i].GetComponent<SpriteRenderer>().color = bg.color;
		}
	}
	

}
