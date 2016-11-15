using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FCTCanvas : MonoBehaviour {

	public Text fctText;
	public RectTransform fctTextTransform;
	public CanvasGroup fctTextGroup;
	Vector2 zeroVector = new Vector2(0,1);

	
	public void SetFCT(Vector3 pos,int p)
	{
		transform.position = pos;
		StartCoroutine( StartFCT(p) );
	}


	IEnumerator StartFCT (int points) 
	{
		fctText.text = points.ToString();
		fctTextGroup.alpha = 1;
		fctTextTransform.anchoredPosition = zeroVector;
		while(fctTextGroup.alpha > 0)
		{
			fctTextGroup.alpha -= 0.5f*Time.deltaTime;
			fctTextTransform.anchoredPosition += 5*Vector2.up*Time.deltaTime;
			yield return null;
		}
	}
}
