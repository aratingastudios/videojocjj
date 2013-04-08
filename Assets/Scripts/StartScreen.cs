using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
	public Texture2D m_tex;
	//public float wait_time;
	//float start_time;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		//start_time = Time.time;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//float elapsed_time = Time.time - start_time;
		//if(elapsed_time > wait_time)
		//	Application.LoadLevel("02_MAIN_MENU");
		
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
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), m_tex);
	}
}
