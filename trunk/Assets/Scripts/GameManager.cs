////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ANDROID: adb.exe logcat -s Unity
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	GameObject[] m_players;
	GameObject targetCameraManager;
	
	public int m_level;
	int num_levels;
	
	public int iPlayerActive=0;
	bool[] bGoalsReached;
	
	GameObject triangle;
	public float triangle_offset;
	public bool bConcept;
	
	GUIManager guiManager;
	ScoreManager scoreManager;
	GameObject goAudioManager;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		Application.targetFrameRate = 60;
		
		m_players = new GameObject[2];
		
		if(GameObject.Find("PLAYER0"))
			m_players[0] = GameObject.Find("PLAYER0");
		else
			m_players[0] = GameObject.Find("PLAYER0_concept");
		
		if(GameObject.Find("PLAYER1"))
			m_players[1] = GameObject.Find("PLAYER1");
		else
			m_players[1] = GameObject.Find("PLAYER1_concept");
		
		targetCameraManager = GameObject.Find("TargetCamera");
		num_levels = PlayerPrefs.GetInt("num_levels");
		
		if(bConcept)
			triangle = (GameObject)Instantiate(Resources.Load("triangle", typeof(GameObject)));
		
		guiManager = GetComponent<GUIManager>();
		scoreManager = GetComponent<ScoreManager>();
		
		goAudioManager = GameObject.Find("goAudioManager");
	}
		
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		if(goAudioManager && goAudioManager.audio.isPlaying)
			goAudioManager.audio.Stop();
			
		bGoalsReached = new bool[]{false, false};
		
		m_players[0].SendMessage("SetActive", true);
		m_players[1].SendMessage("SetActive", false);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//Keyboard reset level
		if(Input.GetKeyDown(KeyCode.R))
			ResetLevel();
		
		//Keyboard change player
		if(Input.GetKeyDown(KeyCode.Q))
			ChangePlayer();
		
		if(bConcept && m_players[iPlayerActive])
		{
			Vector3 pos = m_players[iPlayerActive].transform.position;
			triangle.transform.position = new Vector3(pos.x, pos.y+triangle_offset, pos.z);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ResetLevel()
	{
		PlayerPrefs.SetString("next_level", Application.loadedLevelName);
		Application.LoadLevel("LOADING");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ChangePlayer()
	{
		iPlayerActive++;
		iPlayerActive=iPlayerActive%2;
		
		targetCameraManager.SendMessage("SetPlayerActive", iPlayerActive);
		targetCameraManager.SendMessage("ChangePlayer");
		
		if(scoreManager!=null)
			scoreManager.SendMessage("PlayerChanged");
		
		m_players[0].SendMessage("SetActive", (iPlayerActive==0));
		m_players[1].SendMessage("SetActive", (iPlayerActive==1));
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void goalReached(int id)
	{
		bGoalsReached[id] = true;
		
		//Level completed
		if(bGoalsReached[0] && bGoalsReached[1])
		{
			DoLevelCompleted();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void DoLevelCompleted()
	{
		guiManager.SendMessage("LevelCompleted");
			
		if(scoreManager!=null)
			scoreManager.SendMessage("CheckBonus", m_level);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void goalLeft(int id)
	{
		bGoalsReached[id] = false;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SecretItemReached_M()
	{
		scoreManager.SendMessage("SecretItemReached");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void LoadNextLevel()
	{
		m_level+=1;
		if(m_level<=num_levels)
		{
			string s_level = m_level.ToString();
			if(m_level<10) s_level = "0"+s_level;
			
			PlayerPrefs.SetString("next_level", "LEVEL_"+s_level);
			Application.LoadLevel("LOADING");
		}
		else
		{
			Application.LoadLevel("02_MAIN_MENU");
		}
	}
}





