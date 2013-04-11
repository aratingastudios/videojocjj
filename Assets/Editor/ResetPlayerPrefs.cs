using UnityEngine;
using UnityEditor;
using System.Collections;

public class ResetPlayerPrefs : MonoBehaviour
{
	[MenuItem ("Custom/ResetPlayerSettings")]
    static void ResetPlayerSettings()
	{
		PlayerPrefs.DeleteAll(); //No funciona en el editor
	}
}
