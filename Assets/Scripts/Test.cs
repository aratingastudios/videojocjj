using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(20,20,150,150), "load"))
			Application.LoadLevel("LEVEL_01_TEST");
	}
}
