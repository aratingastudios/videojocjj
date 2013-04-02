using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
	public GUISkin m_skin;
	public float wait_time;
	float start_time;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		start_time = Time.time;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		float elapsed_time = Time.time - start_time;
		
		if(elapsed_time > wait_time)
		{
			Application.LoadLevel("01 MAIN_MENU");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "start_screen");
	}
}
