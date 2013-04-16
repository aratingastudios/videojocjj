using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
	public Texture2D m_tex;
	
	//public float wait_time;
	//float start_time;
	
	float ratio;
	float ratio_4_3 = 4.0f/3.0f;
	float height;
	float offset;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		//start_time = Time.time;
		
		ratio = (float)Screen.width/(float)Screen.height;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//float elapsed_time = Time.time - start_time;
		//if(elapsed_time > wait_time)
		//	Application.LoadLevel("02_MAIN_MENU");
		
		ratio = (float)Screen.width/(float)Screen.height;
		
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(Input.touchCount > 0)
            	Application.LoadLevel("02_MAIN_MENU");
		}
		else
		{
			if(Input.GetMouseButton(0))
            	Application.LoadLevel("02_MAIN_MENU");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		height = ratio_4_3/ratio;
		offset = (1.0f-height)/2.0f;
		
		//GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), m_tex);
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex, new Rect(0,offset,1,height));
	}
}









