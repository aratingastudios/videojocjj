using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public GUISkin m_skin_main_menu;
	public Texture2D m_tex;
	public Texture2D m_tex_info;
	public Texture2D m_tex_reset;
	
	int buttonSize;
	int buttonSize3;
	int buttonSize4;
		
	bool bShowOptions;
	bool bShowInfo;
	bool bShowReset;
	
	GameObject goAudioManager;
	bool bAudio = true;
	bool bAudioOld = true;

	//title
	float ratio;
	int w_title;
	int h_title;

	float height;
	float offset;
	float ratio_info;
	float ratio_4_3 = 4.0f/3.0f;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		buttonSize  = (int)(Screen.width * (80.0f/800.0f));
		buttonSize3 = (int)(buttonSize*1.5f);
		buttonSize4 = (int)(Screen.width * (400.0f/800.0f));
		
		bAudio = (PlayerPrefs.GetInt("music") == 0); //0=ON, 1=OFF
		bAudioOld = bAudio;
		
		goAudioManager = GameObject.Find("goAudioManager");
		if(goAudioManager && !goAudioManager.audio.isPlaying && bAudio)
			goAudioManager.audio.Play();

		ratio = m_tex.width / m_tex.height;
		w_title = (int)(Screen.width / 2f);
		h_title = (int)((float)w_title / ratio);

		ratio_info = ratio = (float)Screen.width / (float)Screen.height;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin_main_menu;

		if(!bShowReset)
		{
			if(!bShowInfo)
			{
				GUI.DrawTexture(new Rect(Screen.width/2-w_title/2, Screen.height/15, w_title, h_title), m_tex);
				
				if(GUI.Button(new Rect(Screen.width/2-buttonSize3/2, Screen.height-buttonSize3-buttonSize3/6, buttonSize3, buttonSize3), "", "continue")){
					Application.LoadLevel("03_LEVEL_SELECT");
				}
				if(GUI.Button(new Rect(20, Screen.height-20-buttonSize, buttonSize, buttonSize), "", "options")){
					bShowOptions=!bShowOptions;
				}
				if(GUI.Button(new Rect(Screen.width-20-buttonSize, Screen.height-20-buttonSize, buttonSize, buttonSize), "", "info")){
					bShowInfo = true;
				}
				if(bShowOptions){
					OnGUIOptions();
				}
			}
			else{
				OnGUIShowInfo();
			}
		}
		else{
			OnGUIShowReset();
		}
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
			PlayerPrefs.SetInt("music", bAudio ? 0 : 1); //0=ON, 1=OFF
		}
		if(GUI.Button(new Rect(20, Screen.height-60-buttonSize*3, buttonSize, buttonSize), "", "reset")){
			bShowReset = true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowInfo()
	{
		height = ratio_4_3/ratio_info;
		offset = (1.0f-height)/2.0f;
		
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_info, new Rect(0,offset,1,height));

		if(GUI.Button(new Rect(0,0,Screen.width,Screen.height), "", "dummy_style")){
			bShowInfo=false;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowReset()
	{
		height = ratio_4_3/ratio_info;
		offset = (1.0f-height)/2.0f;
		
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_reset, new Rect(0,offset,1,height));

		if(GUI.Button(new Rect(Screen.width/2-buttonSize4/2, Screen.height/2-buttonSize4/8, buttonSize4, buttonSize4/4), "", "yes")){
			PlayerPrefs.DeleteAll();
			bShowReset=false;
		}
		if(GUI.Button(new Rect(Screen.width/2-buttonSize4/2, Screen.height/2+buttonSize4/8, buttonSize4, buttonSize4/4), "", "no")){
			bShowReset=false;
		}
	}
}







