using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	void Awake()
	{
        DontDestroyOnLoad(transform.gameObject);
    }
}
