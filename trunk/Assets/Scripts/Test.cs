using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	float res;
	float _from = 1.0f;
	float _to = 0.0f;
	
	void Update()
	{
		//Debug.Log("time: " + Time.time);
		//Debug.Log("deltatime: " + Time.deltaTime);
		
		Debug.Log("res: " + res);
		
		res = Mathf.Lerp(_from, _to, Time.time);
	}
}
