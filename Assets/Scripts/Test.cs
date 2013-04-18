using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public GUIStyle m_style;
	
	void OnGUI()
	{
		GUI.Label(new Rect(50,50,500,100), "A B C D E", m_style);
	}
}
