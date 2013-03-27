//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//http://forum.unity3d.com/threads/165701-How-would-you-map-parallax-from-perspective-to-orthographic
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour 
{
	Transform targetCamera;
	public float scaleFactor = 0.25F; //this would move the layer for 0.25m for every 1m of camera movement
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		targetCamera = GameObject.Find("TargetCamera").transform;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update ()
	{
		foreach(Transform child in transform)
		{
			//transform.position = new Vector3(targetCamera.position.x*scaleFactor, transform.position.y, transform.position.z);
			child.position = new Vector3(targetCamera.position.x*scaleFactor, targetCamera.position.y*scaleFactor, child.position.z);
		}
	}
}
