using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SecretItemManager : MonoBehaviour
{
	GameObject gameManagerObj;
	GUIManager guiManager;
	ScoreManager scoreManager;
	
	string state="idle";
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
		guiManager = gameManagerObj.GetComponent<GUIManager>();
		scoreManager = gameManagerObj.GetComponent<ScoreManager>();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnTriggerEnter(Collider collider)
	{
		if(state=="idle" && collider.name.Contains("PLAYER"))
			state = "found";
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(state=="found")
		{
			scoreManager.SendMessage("SecretItemReached");
			
			if(audio && guiManager.bAudioFx && !audio.isPlaying)
				audio.Play();
			
			state="playing_audio";
		}
		
		if(state=="playing_audio")
		{
			if(!audio.isPlaying)
				Destroy(gameObject);
		}
	}
}
