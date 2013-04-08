using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	public GUISkin m_skin;
	public string gui_state;
	
	//Mobile->Tamaño de pantalla de referencia: 800x480
	int buttonSize;
	int buttonSize2;
	int buttonSize3;
	
	int labelSize;
	
	float originalRatioLabel=85.0f/800.0f;
	float originalRatio=80.0f/800.0f;
	float originalRatio2=100.0f/800.0f;
	
	int margin = 20;
	int marginLabel = 20;
	int marginStars = 10;
	
	string[] player_buttons;
	
	//FPS
	float updateInterval = 1.0f;
	float accum = 0; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval
	string sFPS;
	bool bFPS = true;
	
	GameManager gameManager;
	ScoreManager scoreManager;
	
	bool bAudio = true;
	bool bAudioOld = true;
	
	bool bAudioFx = true;
	bool bAudioFxOld = true;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gui_state="in_game";
		
		buttonSize = (int)(Screen.width * originalRatio);
		buttonSize2 = (int)(Screen.width * originalRatio2);
		buttonSize3 = (int)(buttonSize*1.5f);
		
		labelSize = (int)(Screen.width * originalRatioLabel);
		
		timeleft = updateInterval;
		
		player_buttons = new string[]{"player0_button", "player1_button"};
		
		gameManager = GetComponent<GameManager>();
		scoreManager = GetComponent<ScoreManager>();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		bAudio = (PlayerPrefs.GetInt("music") == 1);
		if(bAudio)
			audio.Play();
		bAudioOld = bAudio;
		
		bAudioFx = (PlayerPrefs.GetInt("musicFx") == 1);
		bAudioFxOld = bAudioFx;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(bFPS)
			showFPS();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		if(gui_state=="in_game")
		{
			//Menu button
			if(GUI.Button(new Rect(Screen.width-5-buttonSize,5,buttonSize,buttonSize), "", "pause"))
				gui_state="show_menu";
			
			//Reset level button
			if(GUI.Button(new Rect(Screen.width-10-buttonSize*2,5,buttonSize,buttonSize), "", "reset"))
				gameManager.SendMessage("ResetLevel");
			
			//Change player button
			if(GUI.Button(new Rect(Screen.width-buttonSize2*2-10,Screen.height-buttonSize2-5,buttonSize2,buttonSize2), "", player_buttons[gameManager.iPlayerActive]))
				gameManager.SendMessage("ChangePlayer");
		}
		
		else if(gui_state=="show_menu")
			OnGUIShowMenu();
		
		else if(gui_state=="show_options")
			OnGUIShowOptions();
		
		else if(gui_state=="show_help")
			OnGUIShowHelp();
		
		else if(gui_state=="show_level_completed")
			OnGUILevelCompleted();
			
		if(bFPS)
			GUI.Label(new Rect(20,20,200,50), sFPS, "fps");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUILevelCompleted()
	{
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "background");
		GUI.Box(new Rect(Screen.width/2-200,0,400,Screen.height), "", "level_completed");
		
		//labels
		GUI.BeginGroup(new Rect(Screen.width/2-labelSize-labelSize/2-marginLabel,Screen.height/2,labelSize*3+marginLabel*2,labelSize));
		
		GUI.Label(new Rect(0,0,labelSize,labelSize), "TIME BONUS", scoreManager.bTimeBonus ? "bonus_text_on" : "bonus_text_off");
		GUI.Label(new Rect(labelSize+marginLabel,0,labelSize,labelSize), "PLAYER SWAPS", scoreManager.bChangesBonus ? "bonus_text_on" : "bonus_text_off");
		GUI.Label(new Rect(labelSize*2+marginLabel*2,0,labelSize,labelSize), "SECRET ITEM", scoreManager.bSecretItemBonus ? "bonus_text_on" : "bonus_text_off");
			
		GUI.EndGroup();
		
		//stars
		GUI.BeginGroup(new Rect(Screen.width/2-labelSize-labelSize/2-marginStars,Screen.height/2-100,labelSize*3+marginStars*2,labelSize));
		
		GUI.Box(new Rect(0,0,labelSize,labelSize), "", scoreManager.bTimeBonus ? "star_on" : "star_off");
		GUI.Box(new Rect(labelSize+marginStars,0,labelSize,labelSize), "", scoreManager.bChangesBonus ? "star_on" : "star_off");
		GUI.Box(new Rect(labelSize*2+marginStars*2,0,labelSize,labelSize), "", scoreManager.bSecretItemBonus ? "star_on" : "star_off");
				
		GUI.EndGroup();
		
		//buttons
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize-buttonSize/2-margin,Screen.height-buttonSize-50,buttonSize*3+margin*2,buttonSize));
		
		if(GUI.Button(new Rect(0, 0, buttonSize, buttonSize), "", "reset"))
			gameManager.SendMessage("ResetLevel");
		
		if(GUI.Button(new Rect(buttonSize+margin, 0, buttonSize, buttonSize), "", "levels"))
			Application.LoadLevel("03_LEVEL_SELECT");
				
		if(GUI.Button(new Rect(buttonSize*2+margin*2, 0, buttonSize, buttonSize), "", "next"))
			gameManager.SendMessage("LoadNextLevel");
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowMenu()
	{
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-buttonSize3/2-margin,Screen.height/2-buttonSize3/2,buttonSize3*3+margin*2,buttonSize3));
		
		if(GUI.Button(new Rect(0,0,buttonSize3,buttonSize3), "", "continue"))
			gui_state = "in_game";
			
		if(GUI.Button(new Rect(buttonSize3+margin,0,buttonSize3,buttonSize3), "", "levels"))
			Application.LoadLevel("03_LEVEL_SELECT");
			
		if(GUI.Button(new Rect(buttonSize3*2+margin*2,0,buttonSize3,buttonSize3), "", "options"))
			gui_state = "show_options";
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowOptions()
	{	
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-margin,Screen.height/2-buttonSize3-margin,buttonSize3*2+margin,buttonSize3*2+margin));
		bAudioFx = GUI.Toggle(new Rect(0,0,buttonSize3,buttonSize3), bAudioFx, "", "fx");
		bAudio   = GUI.Toggle(new Rect(buttonSize3+margin,0,buttonSize3,buttonSize3), bAudio, "", "music");
		
		if(GUI.Button(new Rect(0,buttonSize3+margin,buttonSize3, buttonSize3), "", "help"))
			gui_state = "show_help";
		
		GUI.Button(new Rect(buttonSize3+margin,buttonSize3+margin,buttonSize3, buttonSize3), "", "info");
		GUI.EndGroup();
		
		if(GUI.Button(new Rect(20,Screen.height-buttonSize-20,buttonSize, buttonSize), "", "back"))
			gui_state = "show_menu";
		
		if(bAudio != bAudioOld)
		{
			audio.enabled = bAudio;
			if(audio.enabled)
				audio.Play();
			bAudioOld = bAudio;
			PlayerPrefs.SetInt("music", bAudio ? 1 : 0);
		}
		
		if(bAudioFx != bAudioFxOld)
		{
			bAudioFxOld = bAudioFx;
			PlayerPrefs.SetInt("musicFx", bAudioFx ? 1 : 0);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowHelp()
	{
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "help_screen");
		
		if(GUI.Button(new Rect(20,20,buttonSize, buttonSize), "", "back"))
			gui_state = "show_options";
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void LevelCompleted()
	{
		gui_state="show_level_completed";
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void showFPS()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0.0)
		{
			float fps = accum / frames;
			sFPS = System.String.Format("{0:F2} FPS", fps);

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
