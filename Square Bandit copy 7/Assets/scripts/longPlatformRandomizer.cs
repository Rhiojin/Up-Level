using UnityEngine;
using System.Collections;

public class longPlatformRandomizer : MonoBehaviour {

	public Transform[] platforms;
	Vector3 pos;
	void Start () 
	{
		for(int i = 0; i < platforms.Length;i++)
		{
			pos = platforms[i].localPosition;
			pos.x = Random.Range(-0.5f,0.5f);
			platforms[i].localPosition = pos;

		}
	}

}
