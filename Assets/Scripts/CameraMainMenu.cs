using UnityEngine;
using System.Collections;

public class CameraMainMenu : MonoBehaviour
{
	public float smoothTime = 1.0f;
	Vector3 velocity = Vector3.zero;
	
	Vector3[] targets;
	Vector3 target;
	
	int index=0;
	
	float dist;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		targets = new Vector3[2];
		targets[0] = new Vector3(15.0f, 0.0f, 0.0f);
		targets[1] = new Vector3(-15.0f, 0.0f, 0.0f);
		
		target = targets[index];
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
		
		dist = Vector3.Distance(transform.position, target);
		
		if(dist < 0.5f)
		{
			index = (index+1)%2;
			target = targets[index];
		}
	}
}
