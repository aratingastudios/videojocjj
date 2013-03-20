using UnityEngine;
using System.Collections;

public class WallManager : MonoBehaviour
{
	public float fadeSpeed = 1.0f;
	bool bActivate = false;
	Color color;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		//color = renderer.material.color;
		//color.a = 0.0f;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Activate()
	{
		bActivate=true;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		/*
		if(bActivate)
		{
			renderer.material.color = Color.Lerp(renderer.material.color, color, fadeSpeed * Time.deltaTime);
			
			if(renderer.material.color.a < 0.05f)
			{
				bActivate=false;
				gameObject.active=false;
			}
		}
		*/
		if(bActivate)
		{
			bActivate=false;
			gameObject.SetActive(false);
		}
	}
}
