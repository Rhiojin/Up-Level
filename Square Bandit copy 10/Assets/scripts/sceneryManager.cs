using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneryManager : MonoBehaviour {

	public Transform clouds1;
	public Transform clouds2;
	public Transform clouds3;

	Vector3 clouds1target = new Vector2(-60, 60);
	Vector3 clouds2target = new Vector2(-77, 45);
	Vector3 clouds3target = new Vector2(-86, 47);

	Vector2 clouds1MinMax = new Vector2(-60, 60);
	Vector2 clouds2MinMax = new Vector2(-77, 45);
	Vector2 clouds3MinMax = new Vector2(-86, 47);

	public SpriteRenderer sky;
	public  Color[] colors;
	private int pointer = 1;
	private Vector3 colorCheck1;
	private Vector3 colorCheck2;

	float colorLerpSpeed = 0.5f;
	float speed = 2f;
	float time = 0;
	void Start () 
	{
		clouds1target = clouds1.localPosition;
		clouds2target = clouds2.localPosition;
		clouds3target = clouds3.localPosition;

		int r = Random.Range(0,colors.Length);
		sky.color = colors[r];
	}
	
	// Update is called once per frame
	void Update () 
	{
		clouds1target.x -= Time.deltaTime * speed;
		clouds2target.x -= Time.deltaTime * speed/2;
		clouds3target.x -= Time.deltaTime * speed/5;

		if(clouds1target.x < clouds1MinMax.x) clouds1target.x = clouds1MinMax.y;
		if(clouds2target.x < clouds2MinMax.x) clouds2target.x = clouds2MinMax.y;
		if(clouds3target.x < clouds3MinMax.x) clouds3target.x = clouds3MinMax.y;

		clouds1.localPosition = clouds1target;
		clouds2.localPosition = clouds2target;
		clouds3.localPosition = clouds3target;

		sky.color = Color.Lerp(sky.color, colors[pointer], 0.05f*Time.deltaTime);
		CheckColors(sky.color, colors[pointer]);
	}

	void CheckColors(Color a, Color b)
	{
		colorCheck1.x = a.r;
		colorCheck1.y = a.g;
		colorCheck1.z = a.b;

		colorCheck2.x = b.r;
		colorCheck2.y = b.g;
		colorCheck2.z = b.b;
		if((colorCheck2-colorCheck1).sqrMagnitude <= 0.001f)
		{
			pointer++;
			if(pointer >= colors.Length) pointer = 0;
		}
	}
}
