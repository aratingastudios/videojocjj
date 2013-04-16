using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public GUISkin m_skin_main_menu;
	int buttonSize;
	int buttonSize3;
	float originalRatio=80.0f/800.0f;
	
	bool bShowOptions;
	
	GameObject goAudioManager;
	bool bAudio = true;
	bool bAudioOld = true;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		buttonSize  = (int)(Screen.width * originalRatio);
		buttonSize3 = (int)(buttonSize*1.5f);
		
		bAudio = (PlayerPrefs.GetInt("music") == 1);
		bAudioOld = bAudio;
		
		goAudioManager = GameObject.Find("goAudioManager");
		if(goAudioManager && !goAudioManager.audio.isPlaying && bAudio)
			goAudioManager.audio.Play();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin_main_menu;
		
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "main_menu");
		
		if(GUI.Button(new Rect(Screen.width/2-buttonSize3/2, Screen.height/2-buttonSize3/2, buttonSize3, buttonSize3), "", "continue"))
			Application.LoadLevel("03_LEVEL_SELECT");
				
		if(GUI.Button(new Rect(20, Screen.height-20-buttonSize, buttonSize, buttonSize), "", "options"))
		{
			//PlayerPrefs.DeleteAll();
			bShowOptions=!bShowOptions;
		}
	
		GUI.Button(new Rect(Screen.width-20-buttonSize, Screen.height-20-buttonSize, buttonSize, buttonSize), "", "social");
		
		if(bShowOptions)
			OnGUIOptions();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIOptions()
	{
		bAudio = GUI.Toggle(new Rect(20, Screen.height-40-buttonSize*2, buttonSize, buttonSize), bAudio, "", "music");
		
		if(bAudio != bAudioOld)
		{
			if(bAudio)
				goAudioManager.audio.Play();
			else
				goAudioManager.audio.Stop();
			
			bAudioOld = bAudio;
			PlayerPrefs.SetInt("music", bAudio ? 1 : 0);
		}
	}
}







