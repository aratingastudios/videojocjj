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
	
	public float totalTime;
	public int nPlayerChanges = 0;
	
	int num_levels;
	
	float wait_time = 2.0f;
	float fading_time = 1.0f;
	Color old_gui_color;
	float alpha = 1.0f;
	string state = "idle";
	float timer;
	
	int iconSize = 40;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		num_levels = PlayerPrefs.GetInt("num_levels");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
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
			
			GUI.Box(new Rect(40,10,iconSize,iconSize),"","bonus_time");
			GUI.Label(new Rect(100,17,110,50), minutes + ":" + seconds, m_text_style);
			
			GUI.Box(new Rect(200,10,iconSize,iconSize),"","bonus_swaps");
			GUI.Label(new Rect(255,17,110,50), nPlayerChanges + "/" + numChanges, m_text_style);
			
			GUI.Box(new Rect(360,10,iconSize,iconSize),"","bonus_item");
			GUI.Label(new Rect(415,17,100,50), (bSecretItemBonus?1:0)+ "/1", m_text_style);
			
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
	//Message from GameManager
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
	//Message from GameManager
	void PlayerChanged()
	{
		nPlayerChanges++;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Message from SecretItemManager
	void SecretItemReached()
	{
		bSecretItemBonus = true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Message from GUIManager
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
		
		