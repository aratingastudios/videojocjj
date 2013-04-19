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
	
	float totalTime;
	int nPlayerChanges = 0;
	
	int num_levels;
	
	bool bInPause=false;
	
	float screen_width = 800.0f;
	float screen_ratio;
	int boxSize;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		num_levels = PlayerPrefs.GetInt("num_levels");
		screen_ratio = Screen.width / screen_width;
		boxSize  = (int)(40.0f * screen_ratio);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		if(bInPause)
		{
			//Time
			string minutes = Mathf.Floor(Time.timeSinceLevelLoad/60.0f).ToString();
			string seconds = (Time.timeSinceLevelLoad % 60).ToString("00");
			
			GUI.Box(new Rect(40,10,boxSize,boxSize),"","bonus_time");
			GUI.Label(new Rect(100,10,110,50), minutes + ":" + seconds, "score_text");
			
			GUI.Box(new Rect(200,10,boxSize,boxSize),"","bonus_swaps");
			GUI.Label(new Rect(265,10,110,50), nPlayerChanges + "/" + numChanges, "score_text");
			
			GUI.Box(new Rect(360,10,boxSize,boxSize),"","bonus_item");
			GUI.Label(new Rect(425,10,100,50), (bSecretItemBonus?1:0)+ "/1", "score_text");
		}
	}
	
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
	
	void SetInPause(bool b)
	{
		bInPause = b;
	}
}
		
		