using UnityEngine;
using System.Collections;

public class ElevatorManager : MonoBehaviour
{
	public Vector2 targetPos;
	public float smoothTime = 1.0f;
	
	float dist;
	Vector3 velocity = Vector3.zero;
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
			Vector3 newPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);

			if(Vector3.Distance(transform.position, newPos) < 0.01f)
				bElevate=false;
		}
	}
}




