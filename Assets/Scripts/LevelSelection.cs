using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
	public GUISkin m_skin_level_select;
	public int num_levels;
	
	int selGridInt = -1;
    string[] selStrings;
	
	int buttonSize;
	float originalRatio=80.0f/800.0f;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		selStrings = new string[num_levels];
		
		for(int i=0;i<num_levels;i++)
		{
			selStrings[i] = (i+1).ToString();
		}
		
		PlayerPrefs.SetInt("num_levels", num_levels);
		
		buttonSize = (int)(Screen.width * originalRatio);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin_level_select;
		
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
			Application.LoadLevel("02_MAIN_MENU");
		}
	}
}




