////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ANDROID: adb.exe logcat -s Unity
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GUISkin m_skin;
	GameObject[] m_players;
	GameObject targetCameraManager;
	
	public int m_level;
	int num_levels;
	
	//Mobile->Tamaño de pantalla de referencia: 800x480
	int buttonSize;
	int buttonSize2;
	int buttonSize3;
	
	float originalRatio=80.0f/800.0f;
	float originalRatio2=100.0f/800.0f;
	
	int iPlayerActive=0;
	bool[] isAlive;
	
	public string gui_state;
	int iGoalsReached=0;
	string[] player_buttons;
	
	GameObject triangle;
	public float triangle_offset;
	
	bool bAudio = true;
	bool bAudioOld = true;
	
	float margin = 20;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		m_players = new GameObject[2];
		
		if(GameObject.Find("PLAYER0"))
			m_players[0] = GameObject.Find("PLAYER0");
		else
			m_players[0] = GameObject.Find("PLAYER0_concept");
		
		if(GameObject.Find("PLAYER1"))
			m_players[1] = GameObject.Find("PLAYER1");
		else
			m_players[1] = GameObject.Find("PLAYER1_concept");
		
		targetCameraManager = GameObject.Find("TargetCamera");
		num_levels = PlayerPrefs.GetInt("num_levels");
		player_buttons = new string[]{"player0_button", "player1_button"};
		gui_state="in_game";
		//triangle = (GameObject)Instantiate(Resources.Load("triangle", typeof(GameObject)));
	}
		
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		isAlive = new bool[]{true, true};
		
		buttonSize = (int)(Screen.width * originalRatio);
		buttonSize2 = (int)(Screen.width * originalRatio2);
		buttonSize3 = (int)(buttonSize*1.5f);
		
		m_players[0].SendMessage("SetActive", true);
		m_players[1].SendMessage("SetActive", false);
		
		bAudio = (PlayerPrefs.GetInt("music") == 1);
		if(bAudio) audio.Play();
		bAudioOld = bAudio;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//Keyboard reset level
		if(Input.GetKeyDown(KeyCode.R))
			ResetLevel();
		
		//Keyboard change player
		if(Input.GetKeyDown(KeyCode.Q))
			ChangePlayer();
		/*
		if(m_players[iPlayerActive])
		{
			Vector3 pos = m_players[iPlayerActive].transform.position;
			triangle.transform.position = new Vector3(pos.x, pos.y+triangle_offset, pos.z);
		}
		*/
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ResetLevel()
	{
		Application.LoadLevel(Application.loadedLevelName);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ChangePlayer()
	{
		if(isAlive[(iPlayerActive+1)%2])
		{
			iPlayerActive++;
			iPlayerActive=iPlayerActive%2;
			
			targetCameraManager.SendMessage("SetPlayerActive", iPlayerActive);
			targetCameraManager.SendMessage("ChangePlayer");
			
			m_players[0].SendMessage("SetActive", (iPlayerActive==0));
			m_players[1].SendMessage("SetActive", (iPlayerActive==1));
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void goalReached(int id)
	{
		iGoalsReached++;
		isAlive[id]=false;
		
		//Change player
		if(iGoalsReached<2)
		{
			ChangePlayer();
		}
		//Level completed
		else
		{
			gui_state="level_completed";
			targetCameraManager.SendMessage("SetLevelCompleted");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
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
				ResetLevel();
			
			//Change player button
			if(GUI.Button(new Rect(Screen.width-buttonSize2*2-10,Screen.height-buttonSize2-5,buttonSize2,buttonSize2), "", player_buttons[iPlayerActive]))
			{
				ChangePlayer();
			}
		}
		
		else if(gui_state=="show_menu")
		{
			OnGUIShowMenu();
		}
		
		else if(gui_state=="show_options")
		{
			OnGUIShowOptions();
		}
		
		else if(gui_state=="level_completed")
		{
			if(GUI.Button(new Rect(Screen.width/2-buttonSize3/2, Screen.height/2-buttonSize3/2, buttonSize3, buttonSize3), "NEXT"))
			{
				m_level+=1;
				if(m_level<=num_levels)
				{
					string s_level = m_level.ToString();
					if(m_level<10)
						s_level = "0"+s_level;
					Application.LoadLevel("LEVEL_"+s_level);
					
				}
				else
					Application.LoadLevel("LEVEL_SELECT");
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowMenu()
	{
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-buttonSize3/2-margin,Screen.height/2-buttonSize3/2,buttonSize3*3+margin*2,buttonSize3));
		
		if(GUI.Button(new Rect(0,0,buttonSize3,buttonSize3), "", "continue"))
			gui_state = "in_game";
			
		if(GUI.Button(new Rect(buttonSize3+margin,0,buttonSize3,buttonSize3), "", "levels"))
			Application.LoadLevel("LEVEL_SELECT");
			
		if(GUI.Button(new Rect(buttonSize3*2+margin*2,0,buttonSize3,buttonSize3), "", "options"))
			gui_state = "show_options";
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowOptions()
	{	
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-margin,Screen.height/2-buttonSize3-margin,buttonSize3*2+margin,buttonSize3*2+margin));
        GUI.Button(new Rect(0,0,buttonSize3, buttonSize3), "", "fx");
		GUI.Button(new Rect(buttonSize3+margin,0,buttonSize3, buttonSize3), "", "music");
		GUI.Button(new Rect(0,buttonSize3+margin,buttonSize3, buttonSize3), "", "help");
		GUI.Button(new Rect(buttonSize3+margin,buttonSize3+margin,buttonSize3, buttonSize3), "", "info");
		GUI.EndGroup();
		
		if(GUI.Button(new Rect(20,Screen.height-buttonSize-20,buttonSize, buttonSize), "", "arrow_left"))
			gui_state = "show_menu";
		
		/*
		bAudio = GUI.Toggle(new Rect(Screen.width/2-buttonSize3/2-buttonSize3-20,Screen.height/2-buttonSize3/2,buttonSize3,buttonSize3), bAudio, "audio");
		
		if(bAudio != bAudioOld)
		{
			audio.enabled = bAudio;
			if(audio.enabled) audio.Play();
			bAudioOld = bAudio;
			PlayerPrefs.SetInt("music", bAudio ? 1 : 0);
		}
		*/
	}
}





