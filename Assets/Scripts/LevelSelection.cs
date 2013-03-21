using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
	public int num_levels;
	int selGridInt = -1;
    string[] selStrings;
	public GUISkin m_skin;

	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		selStrings = new string[num_levels];
		
		for(int i=0;i<num_levels;i++)
		{
			selStrings[i] = (i+1).ToString();
		}
		
		PlayerPrefs.SetInt("num_levels", num_levels);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		selGridInt = GUI.SelectionGrid(new Rect(50, 50, Screen.width-100, 300), selGridInt, selStrings, 6, "sel_grid");
		
		if(selGridInt>-1)
		{
			string nom = "LEVEL_";
			
			if(selGridInt<10) 
				nom = nom+"0";
			nom = nom + (selGridInt+1);
			
			Application.LoadLevel(nom);
		}
	}
}
