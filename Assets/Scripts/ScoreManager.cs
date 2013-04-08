using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public float levelTime;
	public int numChanges;
	
	bool bTimeBonus = false;
	bool bChangesBonus = false;
	
	int nPlayerChanges = 0;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{

	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CheckBonus()
	{
		//Check Time Bonus
		if(Time.timeSinceLevelLoad  <= levelTime)
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
}
