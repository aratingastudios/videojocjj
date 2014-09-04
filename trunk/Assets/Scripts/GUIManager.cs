using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	public GUISkin m_skin;
	public string gui_state;
	
	//Mobile->Tamaño de pantalla de referencia: 800x480
	float screenWidth = 800.0f;
	float screenHeight = 480.0f;
	
	float screen_ratio;
	
	int buttonSize;
	int buttonSize2;
	int buttonSize3;
	int marginButton;
	int black_width;
	
	string[] player_buttons;
	
	//FPS
	float updateInterval = 1.0f;
	float accum = 0; // FPS accumulated over the interval
	int frames = 0;  // Frames drawn over the interval
	float timeleft;  // Left time for current interval
	string sFPS;
	bool bFPS = false;
	
	GameManager gameManager;
	ScoreManager scoreManager;
	
	public bool bAudioFx = true;
	bool bAudioFxOld = true;
	
	public Texture2D m_tex_help;
	public Texture2D m_tex_info;
	public Texture2D m_tex_tut_01;
	public Texture2D m_tex_tut_02;
	public Texture2D m_tex_tut_03;
	public Texture2D m_tex_tut_04;
	float ratio;
	float ratio_4_3 = 4.0f/3.0f;
	float height;
	float offset;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gui_state="in_game";
		
		timeleft = updateInterval;
		
		player_buttons = new string[]{"player0_button", "player1_button"};
		
		gameManager = GetComponent<GameManager>();
		scoreManager = GetComponent<ScoreManager>();
		
		bAudioFx = (PlayerPrefs.GetInt("musicFx") == 0); //0=ON, 1=OFF
		bAudioFxOld = bAudioFx;
		
		ratio = (float)Screen.width/(float)Screen.height;
		
		if(gameManager.m_level == 1)
			gui_state = "show_tut_01";
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		////////////////////////////
		//TODO:UNA VEZ TERMINADA LA FASE DE PRUEBAS HABRA QUE PONER ESTO EN EL AWAKE!!!
		////////////////////////////
		screen_ratio = Screen.width / screenWidth;
		
		buttonSize  = (int)(80.0f * screen_ratio);
		buttonSize2 = (int)(100.0f * screen_ratio);
		buttonSize3 = (int)(120.0f * screen_ratio);
		marginButton = (int)(50.0f * screen_ratio);
		black_width  = (int)(400.0f * screen_ratio);
		
		////////////////////////////
		
		if(bFPS)
			ShowFPS();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No usamos el escalado de GUI con GUI.matrix porque estira los controles entre 16/9 y 4/3.
	//Tendriamos que poner el mismo escalado en X-Y y para eso mejor lo hacemos manualmente.
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		if(gui_state=="in_game")
		{
			//Menu button
			if(GUI.Button(new Rect(Screen.width-5-buttonSize,5,buttonSize,buttonSize), "", "pause"))
			{
				gui_state="show_menu";
				scoreManager.SendMessage("SetState", "in_pause");
			}
			
			//Reset level button
			if(GUI.Button(new Rect(Screen.width-10-buttonSize*2,5,buttonSize,buttonSize), "", "reset"))
				gameManager.SendMessage("ResetLevel");
			
			//Change player button
			if(GUI.Button(new Rect(Screen.width-buttonSize2*2-10,Screen.height-buttonSize2-5,buttonSize2,buttonSize2), "", player_buttons[gameManager.iPlayerActive]))
				gameManager.SendMessage("ChangePlayer");
		}
		
		else if(gui_state=="show_menu")
			OnGUIShowMenu();
		
		else if(gui_state=="show_info")
			OnGUIShowInfo();

		else if(gui_state=="show_options")
			OnGUIShowOptions();
		
		else if(gui_state.Contains("show_tut"))
			OnGUIShowHelp();
		
		else if(gui_state=="show_level_completed")
			OnGUILevelCompleted();
			
		if(bFPS)
			GUI.Label(new Rect(20,50,200,50), sFPS, "fps");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUILevelCompleted()
	{
		GUI.Box(new Rect(Screen.width/2-black_width/2,0,black_width,Screen.height), "", "black");
		
		string minutes = Mathf.Floor(scoreManager.totalTime/60.0f).ToString();
		string seconds = (scoreManager.totalTime % 60).ToString("00");
		
		string minutesL = Mathf.Floor(scoreManager.levelTime/60.0f).ToString();
		string secondsL = (scoreManager.levelTime % 60).ToString("00");
		
		//Reescalamos el GUI para que se adapte a las diferentes resoluciones de pantalla
		//Para que no deforme tenemos que mirar si ha crecido más en W o en H
		//El factor de escalado será el que haya crecido menos
		float scaleFactor = Mathf.Min(Screen.width/screenWidth,Screen.height/screenHeight);
		Vector3 scale = new Vector3(scaleFactor, scaleFactor, 1.0f);
		Matrix4x4 svMat = GUI.matrix; //save current matrix
		
		float newX = (Screen.width - screenWidth*scaleFactor)/2;
		float newY = (Screen.height - screenHeight*scaleFactor)/2;
		
		GUI.matrix = Matrix4x4.TRS(new Vector3(newX>0?newX:1,newY>0?newY:1,0), Quaternion.identity, scale);
		
		////
		
		GUI.Label(new Rect(0,30,screenWidth,50), "LEVEL COMPLETE!", "level_complete");
		
		int area_w = 350;
		int area_h = 250;
		
		GUILayout.BeginArea(new Rect(screenWidth/2-area_w/2,screenHeight/2-area_h/2,area_w,area_h));
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		GUILayoutBox("bonus_time", 64);
		GUILayout.Label(minutes + ":" + seconds, "bonus_text");
		GUILayout.Label(minutesL + ":" + secondsL, "bonus_text_blue");
		GUILayoutBox(scoreManager.bTimeBonus ? "star_on" : "star_off", 64);
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		GUILayoutBox("bonus_swaps", 64);
		GUILayout.Label(scoreManager.nPlayerChanges + "/" + scoreManager.numChanges, "bonus_text");
		GUILayoutBox(scoreManager.bChangesBonus ? "star_on" : "star_off", 64);
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		GUILayoutBox("bonus_item", 64);
		GUILayout.Label((scoreManager.bSecretItemBonus?1:0)+ "/1", "bonus_text");
		GUILayoutBox(scoreManager.bSecretItemBonus ? "star_on" : "star_off", 64);
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		GUI.matrix = svMat; //restore matrix		
		
		//buttons
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize-buttonSize/2-marginButton,Screen.height-buttonSize-20,buttonSize*3+marginButton*2,buttonSize));
		
		if(GUI.Button(new Rect(0, 0, buttonSize, buttonSize), "", "reset"))
			gameManager.SendMessage("ResetLevel");
		
		if(GUI.Button(new Rect(buttonSize+marginButton, 0, buttonSize, buttonSize), "", "levels"))
			Application.LoadLevel("03_LEVEL_SELECT");
				
		if(GUI.Button(new Rect(buttonSize*2+marginButton*2, 0, buttonSize, buttonSize), "", "next"))
			gameManager.SendMessage("LoadNextLevel");
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GUILayoutBox(string item, int size)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box("", item, GUILayout.Width(size), GUILayout.Height(size));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowMenu()
	{
		GUI.Box(new Rect(-5,-5,Screen.width+10,Screen.height+10), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-buttonSize3/2-marginButton,Screen.height/2-buttonSize3/2,buttonSize3*3+marginButton*2,buttonSize3));
		
		if(GUI.Button(new Rect(0,0,buttonSize3,buttonSize3), "", "continue"))
		{
			gui_state = "in_game";
			scoreManager.SendMessage("SetState", "waiting");
		}
			
		if(GUI.Button(new Rect(buttonSize3+marginButton,0,buttonSize3,buttonSize3), "", "levels"))
			Application.LoadLevel("03_LEVEL_SELECT");
			
		if(GUI.Button(new Rect(buttonSize3*2+marginButton*2,0,buttonSize3,buttonSize3), "", "options"))
		{
			gui_state = "show_options";
			scoreManager.SendMessage("SetState", "show_options");
		}
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowOptions()
	{	
		GUI.Box(new Rect(-5,-5,Screen.width+10,Screen.height+10), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-buttonSize3/2-marginButton,Screen.height/2-buttonSize3/2,buttonSize3*3+marginButton*2,buttonSize3));
		
		bAudioFx = GUI.Toggle(new Rect(0,0,buttonSize3,buttonSize3), bAudioFx, "", "fx");
		
		if(GUI.Button(new Rect(buttonSize3+marginButton,0,buttonSize3, buttonSize3), "", "help"))
			gui_state = "show_tut_01";
		
		if(GUI.Button(new Rect(buttonSize3*2+marginButton*2,0,buttonSize3, buttonSize3), "", "info"))
			gui_state = "show_info";

		GUI.EndGroup();
		
		if(GUI.Button(new Rect(20,Screen.height-buttonSize-20,buttonSize, buttonSize), "", "back"))
			gui_state = "show_menu";
		
		if(bAudioFx != bAudioFxOld)
		{
			bAudioFxOld = bAudioFx;
			PlayerPrefs.SetInt("musicFx", bAudioFx ? 0 : 1); //0=ON, 1=OFF
			gameManager.SendMessage("SetAudioFx", bAudioFx);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowHelp()
	{
		height = ratio_4_3/ratio;
		offset = (1.0f-height)/2.0f;
		
		GUI.Box(new Rect(-5,-5,Screen.width+10,Screen.height+10), "");
		
		if(gui_state == "show_tut_01")
		{
			GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_tut_01, new Rect(0,offset,1,height));
			if(GUI.Button(new Rect(0,0,Screen.width,Screen.height), "", "dummy_style"))
				gui_state = "show_tut_02";
		}
		
		else if(gui_state == "show_tut_02")
		{
			GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_tut_02, new Rect(0,offset,1,height));
			if(GUI.Button(new Rect(0,0,Screen.width,Screen.height), "", "dummy_style"))
				gui_state = "show_tut_03";
		}
		
		else if(gui_state == "show_tut_03")
		{
			GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_tut_03, new Rect(0,offset,1,height));
			if(GUI.Button(new Rect(0,0,Screen.width,Screen.height), "", "dummy_style"))
				gui_state = "show_tut_04";
		}
		
		else if(gui_state == "show_tut_04")
		{
			GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_tut_04, new Rect(0,offset,1,height));
			if(GUI.Button(new Rect(0,0,Screen.width,Screen.height), "", "dummy_style"))
			{
				if(gameManager.m_level == 1)
					gui_state = "in_game";
				else
					gui_state = "show_options";
			}
		}
		
		//if(GUI.Button(new Rect(20,20,buttonSize, buttonSize), "", "back"))
		//	gui_state = "show_options";
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnGUIShowInfo()
	{
		height = ratio_4_3/ratio;
		offset = (1.0f-height)/2.0f;

		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_info, new Rect(0,offset,1,height));
		if(GUI.Button(new Rect(0,0,Screen.width,Screen.height), "", "dummy_style"))
			gui_state = "show_options";
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Message from GameManager
	void LevelCompleted()
	{
		gui_state="show_level_completed";
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ShowFPS()
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
