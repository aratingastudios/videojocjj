using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public float levelTime;
	public int numChanges;
	
	public bool bTimeBonus = false;
	public bool bChangesBonus = false;
	public bool bSecretItemBonus = false;
	
	public GUISkin m_skin;
	public GUIStyle m_text_style;
	
	float totalTime;
	int nPlayerChanges = 0;
	
	int num_levels;
	
	float screen_width = 800.0f;
	float screen_ratio;
	int boxSize;
	int fontSize;
	
	float wait_time = 2.0f;
	float fading_time = 1.0f;
	Color old_gui_color;
	float alpha = 1.0f;
	string state = "idle";
	float timer;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		num_levels = PlayerPrefs.GetInt("num_levels");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		////////////////////////////
		//UNA VEZ TERMINADA LA FASE DE PRUEBAS HABRA QUE PONER ESTO EN EL AWAKE!!!
		////////////////////////////
		screen_ratio = Screen.width / screen_width;
		boxSize = (int)(40.0f * screen_ratio);
		fontSize = (int)(32.0f * screen_ratio);
		////////////////////////////
		
		if(state=="waiting" && Time.time - timer > wait_time)
		{
			state="fading";
			timer = Time.time;
		}
			
		else if(state=="fading")
		{
			float t = (Time.time - timer) / fading_time;
			alpha = Mathf.SmoothStep(1.0f, 0.0f, t);
			
			if(alpha<0.1f)
				state="idle";
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		if(state=="in_pause" || state=="waiting" || state=="fading")
		{
			old_gui_color = GUI.color;
			
			if(state=="fading")
				GUI.color = new Color(1.0f, 1.0f, 1.0f, alpha);
			
			string minutes = Mathf.Floor(Time.timeSinceLevelLoad/60.0f).ToString();
			string seconds = (Time.timeSinceLevelLoad % 60).ToString("00");
			
			m_text_style.fontSize = fontSize;
			GUI.Box(new Rect(40,10,boxSize,boxSize),"","bonus_time");
			GUI.Label(new Rect(95,13,110,50), minutes + ":" + seconds, m_text_style);
			
			GUI.Box(new Rect(200,10,boxSize,boxSize),"","bonus_swaps");
			GUI.Label(new Rect(260,13,110,50), nPlayerChanges + "/" + numChanges, m_text_style);
			
			GUI.Box(new Rect(360,10,boxSize,boxSize),"","bonus_item");
			GUI.Label(new Rect(420,13,100,50), (bSecretItemBonus?1:0)+ "/1", m_text_style);
			
			GUI.color = old_gui_color;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Para hacer outline, pintamos el texto por debajo 9 veces (un poco guarro, no?)
	/*
	void DrawLabelOutline(Rect r, string text, string outline)
	{	
		for(int i=-5;i<6;i+=5)
		{
			for(int j=-5;j<6;j+=5)
			{
				Rect tmp_r = new Rect(r.x+i, r.y+j, r.width, r.height);
				GUI.Label(tmp_r, outline, "text_white");	
			}
		}
		
		GUI.Label(r, text, "text_white");
	}
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CheckBonus(int nLevel)
	{
		//Check Time Bonus
		totalTime = Time.timeSinceLevelLoad;
		
		if(totalTime  <= levelTime)
			bTimeBonus = true;
		
		//Check Changes Bonus
		if(nPlayerChanges <= numChanges)
			bChangesBonus = true;
		
		SaveLevelBonus(nLevel);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SaveLevelBonus(int nLevel)
	{
		int nStars = CheckStars();
		int nStarsOld;
		
		//Set stars for current level
		nStarsOld = PlayerPrefs.GetInt("LEVEL_"+ nLevel + "_stars");
		if(nStarsOld<nStars)
			PlayerPrefs.SetInt("LEVEL_"+ nLevel + "_stars", nStars);
		
		//Unlock next level
		if(nLevel < num_levels)
			PlayerPrefs.SetString("LEVEL_"+ (nLevel+1) + "_locked", "false");
		
		PlayerPrefs.Save();
	}
			
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
	int CheckStars()
	{
		int nStars = 0;
		
		if(bTimeBonus) nStars++;
		if(bChangesBonus) nStars++;
		if(bSecretItemBonus) nStars++;
		
		return nStars;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void PlayerChanged()
	{
		nPlayerChanges++;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SecretItemReached()
	{
		bSecretItemBonus = true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetState(string _state)
	{
		state=_state;
		
		if(state=="waiting")
		{
			alpha=1.0f;
			timer = Time.time;
		}
	}
}
		
		