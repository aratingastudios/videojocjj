using UnityEngine;
using System.Collections;

public class PhysicsTest : MonoBehaviour
{
	//int cont=0;
	public float speed;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		//Debug.Log(cont++);
		rigidbody.AddForce(Vector3.up*speed);
	}
}
