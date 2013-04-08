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
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CheckBonus()
	{
		//Check Time Bonus
		totalTime = Time.timeSinceLevelLoad;
		
		if(totalTime  <= levelTime)
			bTimeBonus = true;
		
		//Check Changes Bonus
		if(nPlayerChanges <= numChanges)
			bChangesBonus = true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void PlayerChanged()
	{
		nPlayerChanges++;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void secretItemReached()
	{
		bSecretItemBonus = true;
	}
}
