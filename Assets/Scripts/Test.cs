using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public float speed = 1f;
	
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);	
		Debug.Log(transform.position.z);
		Debug.Log(Time.time);
		Debug.Log("///////////////////////////////");
			
    }
}
