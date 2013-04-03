using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
	public GUISkin m_skin;
	
	string screen_state = "START_SCREEN";
	
	public float wait_time;
	float start_time;
	
	int buttonSize;
	int buttonSize3;
	float originalRatio=80.0f/800.0f;
	
	public int num_levels;
	int selGridInt = -1;
    string[] selStrings;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		//Start Screen
		start_time = Time.time;
		
		//Main Menu
		buttonSize  = (int)(Screen.width * originalRatio);
		buttonSize3 = (int)(buttonSize*1.5f);
		
		//Level Select
		selStrings = new string[num_levels];
		
		for(int i=0;i<num_levels;i++)
			selStrings[i] = (i+1).ToString();
		
		PlayerPrefs.SetInt("num_levels", num_levels);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(screen_state=="START_SCREEN")
		{
			float elapsed_time = Time.time - start_time;
			
			if(elapsed_time > wait_time)
			{
				screen_state="MAIN_MENU";
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		if(screen_state=="START_SCREEN")
			OnGUIStartScreen();
		
		else if(screen_state=="MAIN_MENU")
			OnGUIMainMenu();
		
		else if(screen_state=="LEVEL_SELECT")
			OnGUILevelSelect();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIStartScreen()
	{	
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "start_screen");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIMainMenu()
	{
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "main_menu");
		
		if(GUI.Button(new Rect(Screen.width/2-buttonSize3/2, Screen.height/2-buttonSize3/2, buttonSize3, buttonSize3), "", "continue"))
			screen_state="LEVEL_SELECT";
				
		GUI.Button(new Rect(20, Screen.height-20-buttonSize, buttonSize, buttonSize), "", "options");
		GUI.Button(new Rect(Screen.width-20-buttonSize, Screen.height-20-buttonSize, buttonSize, buttonSize), "", "social");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUILevelSelect()
	{
		GUI.Box(new Rect(0,0,Screen.width, Screen.height), "", "level_select");
		
		selGridInt = GUI.SelectionGrid(new Rect(50, 50, Screen.width-100, 240), selGridInt, selStrings, 6, "sel_grid");
		
		if(selGridInt>-1)
		{
			string nom = "LEVEL_";
			
			if(selGridInt<10) 
				nom = nom+"0";
			nom = nom + (selGridInt+1);
			
			PlayerPrefs.SetString("next_level", nom);
			Application.LoadLevel("LOADING");
		}
		
		if(GUI.Button(new Rect(20,Screen.height-buttonSize-20,buttonSize, buttonSize), "", "back"))
		{
			screen_state="MAIN_MENU";
		}
	}
}
