using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	float ratio;
	float size;
	
	float ratio1 = 4.0f/3.0f;
	float ratio2 = 16.0f/9.0f;
	
	float size1 = 7.5f;
	float size2 = 9.0f;
	
	void Awake()
	{
		ratio = (float)Screen.width/(float)Screen.height;
		//Debug.Log("ratio: " + ratio);
		
		float f = Mathf.InverseLerp(ratio1, ratio2, ratio);
		//Debug.Log("f: " + f);
		
		size = Mathf.Lerp(size1, size2, f);
		//Debug.Log("size: " + size);
		
		camera.orthographicSize = size;
	}	
}
