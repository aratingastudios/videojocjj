using UnityEngine;
using System.Collections;

public class ElevatorManager : MonoBehaviour
{
	public Transform target;
	bool bElevate=false;
	
	public float smoothTime = 0.3F;
	float dist;
	Vector3 velocity = Vector3.zero;
	float distThreshold = 0.01f;
	
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
			//float newPosition = Mathf.SmoothDamp(transform.position.y, target.position.y, ref yVelocity, smoothTime);
        	//transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
			
			transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
			
			dist = Vector3.Distance(transform.position, target.position);

			if(dist < distThreshold)
			{
				bElevate=false;
			}
		}
	}
}




