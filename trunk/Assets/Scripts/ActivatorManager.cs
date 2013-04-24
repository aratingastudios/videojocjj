
using UnityEngine;
using System.Collections;

public class ActivatorManager : MonoBehaviour
{
	public Transform[] targets;
	public Vector3 targetPos;
	
	string state = "idle";
	Vector3 targetPosition;
	float smoothTime = 0.3f;
	Vector3 velocity = Vector3.zero;
	
	GameObject gameManagerObj;
	GUIManager guiManager;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
		guiManager = gameManagerObj.GetComponent<GUIManager>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(state == "move")
		{
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
			if(Vector3.Distance(transform.position, targetPos) < 0.01f)
				state="activate";
		}
		
		if(state=="activate")
		{
			foreach(Transform t in targets)
				t.SendMessage("Activate");
			
			state="end";
			
			if(audio && !audio.isPlaying && guiManager.bAudioFx)
				audio.Play();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Activate()
	{
		if(state=="idle")
			state = "move";
	}
}
