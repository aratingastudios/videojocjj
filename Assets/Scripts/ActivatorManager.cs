
using UnityEngine;
using System.Collections;

public class ActivatorManager : MonoBehaviour
{
	public Transform[] targets;
	string state = "idle";
	Vector3 targetPosition;
	float smoothTime = 0.3f;
	Vector3 velocity = Vector3.zero;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		targetPosition = new Vector3(transform.position.x, transform.position.y-0.3f, transform.position.z);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(state == "move")
		{
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
			if(Vector3.Distance(transform.position, targetPosition) < 0.1f)
				state="activate";
		}
		
		if(state=="activate")
		{
			foreach(Transform t in targets)
				t.SendMessage("Activate");
			
			state="end";
			
			if(audio && !audio.isPlaying)
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
