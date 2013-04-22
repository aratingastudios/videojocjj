using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public GUIStyle m_style;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		m_style.fontSize = 40;
		GUI.Label(new Rect(50,50,400,100), "TEST FONT 40", m_style);
		m_style.fontSize = 80;
		GUI.Label(new Rect(50,100,400,100), "TEST FONT 80", m_style);
	}
}
