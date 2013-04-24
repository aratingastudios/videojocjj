
using UnityEngine;
using System.Collections;

public class ActivatorManager : MonoBehaviour
{
	public Transform[] targets;
	
	Vector3 targetPos;
	string state = "idle";
	float smoothTime = 0.3f;
	Vector3 velocity = Vector3.zero;
	
	GameObject gameManagerObj;
	GUIManager guiManager;
	
	GameObject targetCameraObj;
	TargetCameraManager targetCameraManager;
	
	bool bActivationDone=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
		guiManager = gameManagerObj.GetComponent<GUIManager>();
		
		targetCameraObj = GameObject.Find("TargetCamera");
		targetCameraManager = targetCameraObj.GetComponent<TargetCameraManager>();
		
		targetPos = new Vector3(transform.position.x, transform.position.y-0.5f, transform.position.z);
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
		if(state=="idle" && !bActivationDone)
		{
			state = "move";
			
			if(targets[0].childCount==0)
				targetCameraManager.SendMessage("LookAtActivator", targets[0].position);
			else
				targetCameraManager.SendMessage("LookAtActivator", targets[0].GetChild(0).position);
			
			bActivationDone=true;
		}
	}
}
