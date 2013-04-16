using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	float ratio;
	float size;
	
	float ratio_16_9 = 16.0f/9.0f;
	float ratio_4_3 = 4.0f/3.0f;
	
	float size_16_9 = 7.5f;
	float size_4_3 = 10.0f;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////
	//Miramos cual es el ratio actual del dispositivo y calculamos
	//el size de la cámara orto adecuado para ese ratio.
	
	void Awake()
	{
		ratio = (float)Screen.width/(float)Screen.height;
		float f = Mathf.InverseLerp(ratio_16_9, ratio_4_3, ratio);
		camera.orthographicSize = Mathf.Lerp(size_16_9, size_4_3, f);
	}	
}
