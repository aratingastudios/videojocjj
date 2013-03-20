using UnityEngine;
using System.Collections;

public class ActivatorManager : MonoBehaviour
{
	public bool bActivate=false;
	public Transform[] targets;
	bool bActivated=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(bActivate && !bActivated)
		{
			foreach(Transform t in targets)
				t.SendMessage("Activate");
			
			bActivate=false;
			bActivated=true;
		}
	}
}
