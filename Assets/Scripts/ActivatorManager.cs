
using UnityEngine;
using System.Collections;

public class ActivatorManager : MonoBehaviour
{
	public Transform[] targets;
	public Vector2 targetPos;
	
	string state = "idle";
	Vector3 targetPosition;
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
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(state == "move")
		{
			Vector3 newPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
			
			if(Vector3.Distance(transform.position, newPos) < 0.01f)
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
