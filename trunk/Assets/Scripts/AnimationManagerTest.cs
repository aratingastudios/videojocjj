using UnityEngine;
using System.Collections;

public class AnimationManagerTest : MonoBehaviour
{
	void Activate()
	{
		 
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		int i=0;
		
		foreach(AnimationState state in animation)
		{
			int x = 20+100*i+20*i;
			
			if(GUI.Button(new Rect(x, Screen.height-60, 100, 50), state.name))
				animation.Play(state.name);
			
			i+=1;
		}
	}
}
