//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//http://forum.unity3d.com/threads/165701-How-would-you-map-parallax-from-perspective-to-orthographic
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour 
{
	Transform targetCamera;
	public float scaleFactor = 0.25F; //this would move the layer for 0.25m for every 1m of camera movement
	Vector3 ini_cam_pos;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		targetCamera = GameObject.Find("TargetCamera").transform;
		ini_cam_pos = targetCamera.position;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update ()
	{
		Vector3 offset = targetCamera.position - ini_cam_pos;
		Vector3 pos = transform.position;
		
		//transform.position = new Vector3(targetCamera.position.x*scaleFactor - offset.x, pos.y, pos.z);
		
		transform.position = new Vector3(targetCamera.position.x*scaleFactor, transform.position.y, transform.position.z);
	}
}
