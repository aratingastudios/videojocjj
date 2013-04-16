using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public GUISkin m_skin_main_menu;
	public Texture2D m_tex;
	
	int buttonSize;
	int buttonSize3;
	float originalRatio=80.0f/800.0f;
	
	bool bShowOptions;
	
	GameObject goAudioManager;
	bool bAudio = true;
	bool bAudioOld = true;
	
	float ratio;
	float ratio_4_3 = 4.0f/3.0f;
	float height;
	float offset;

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
		
		ratio = (float)Screen.width/(float)Screen.height;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin_main_menu;
		
		height = ratio_4_3/ratio;
		offset = (1.0f-height)/2.0f;
		
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex, new Rect(0,offset,1,height));
		
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







