using UnityEngine;
using System.Collections;

public class ElevatorManager : MonoBehaviour
{
	public Vector3 targetPos;
	public float smoothTime = 1.0f;
	
	float dist;
	Vector3 velocity = Vector3.zero;
	float distThreshold = 0.01f;
	bool bElevate=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Activate()
	{
		bElevate=true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(bElevate)
		{	
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
			
			dist = Vector3.Distance(transform.position, targetPos);

			if(dist < distThreshold)
			{
				bElevate=false;
			}
		}
	}
}




