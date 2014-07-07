using UnityEngine;
using System.Collections;

public class TestJoel : MonoBehaviour
{
	public Texture mTex;

	void OnGUI()
	{
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), mTex, new Rect(0.5f,0.5f,0.5f,0.5f));
	}
}
