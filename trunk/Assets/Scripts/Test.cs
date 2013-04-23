using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public GUISkin m_skin;
	
	float screenWidth  = 800.0f;
	float screenHeight = 480.0f;
	
	float scaleFactor;
	float newX;
	float newY;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		//Reescalamos el GUI para que se adapte a las diferentes resoluciones de pantalla
		//Para que no deforme tenemos que mirar si ha crecido más en W o en H
		//El factor de escalado será el que haya crecido menos
		scaleFactor = Mathf.Min(Screen.width/screenWidth,Screen.height/screenHeight);
		Vector3 scale = new Vector3(scaleFactor, scaleFactor, 1.0f);
		Matrix4x4 svMat = GUI.matrix; //save current matrix
		
		newX = (Screen.width - screenWidth*scaleFactor)/2;
		newY = (Screen.height - screenHeight*scaleFactor)/2;
		
		GUI.matrix = Matrix4x4.TRS(new Vector3(newX>0?newX:1,newY>0?newY:1,0), Quaternion.identity, scale);
		
		////
		
		int area_w = 500;
		int area_h = 250;
		
		//GUI.Box(new Rect(screenWidth/2-area_w/2,screenHeight/2-area_h/2,area_w,area_h), "BOX");
		
		GUILayout.BeginArea(new Rect(screenWidth/2-area_w/2,screenHeight/2-area_h/2,area_w,area_h));
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		GUILayoutElement("bonus_time");
		GUILayout.Label("time", "bonus_text");
		GUILayoutElement("star_on");
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		GUILayoutElement("bonus_swaps");
		GUILayout.Label("swaps", "bonus_text");
		GUILayoutElement("star_on");
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		GUILayoutElement("bonus_item");
		GUILayout.Label("item", "bonus_text");
		GUILayoutElement("star_off");
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		GUI.matrix = svMat; //restore matrix
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GUILayoutElement(string item)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box("", item);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
}
