using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public float levelTime;
	public int numChanges;
	
	public bool bTimeBonus = false;
	public bool bChangesBonus = false;
	public bool bSecretItemBonus = false;
	
	float totalTime;
	int nPlayerChanges = 0;
	
	int num_levels;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		num_levels = PlayerPrefs.GetInt("num_levels");
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
}
		
		