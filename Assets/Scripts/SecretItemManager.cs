using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SecretItemManager : MonoBehaviour
{
	bool bFound = false;
	GameObject gameManagerObj;
	GUIManager guiManager;
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
		guiManager = gameManagerObj.GetComponent<GUIManager>();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.name.Contains("PLAYER"))
		{
			if(audio && guiManager.bAudioFx)
				audio.Play();
			
			bFound = true;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(bFound && !audio.isPlaying)
		{
			gameManagerObj.SendMessage("SecretItemReached_M");
			Destroy(gameObject);
		}
	}
}
