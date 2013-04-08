using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public GUISkin m_skin;
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		GUI.Label(new Rect(Screen.width/2-100,Screen.height/2-50,200,200), "LOADING...", "loading");
		
	}
}
