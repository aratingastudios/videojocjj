using UnityEngine;
using System.Collections;

public class TargetCameraManager : MonoBehaviour
{
	Transform[] m_players;
	Transform m_target;
	Transform m_current;
	
	Vector3 m_activatorPos;
	Vector3 m_initialPos;

	public bool m_check_bounds;
	public bool m_change_player;
	public int m_iPlayer;
	
	public float minX;
	public float maxX;
	public float minY_16_9;
	public float minY_4_3;
	float minY;
	public float maxY_16_9;
	public float maxY_4_3;
	float maxY;
	
	bool bChangePos=false;
	Vector3 newPos;
	
	float smoothTime = 0.3F;
	float smoothTimeView = 1.0f;
	Vector3 velocity = Vector3.zero;
	
	float ratio_16_9 = 16.0f/9.0f;
	float ratio_4_3  = 4.0f/3.0f;
	float ratio;
	float inv_ratio;
	
	string state="idle";
	float dist;
	float startTime;
	float currentTime;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		m_players = new Transform[2];

		if(GameObject.Find("PLAYER0"))
			m_players[0] = GameObject.Find("PLAYER0").transform;
		else
			m_players[0] = GameObject.Find("PLAYER0_concept").transform;
		
		if(GameObject.Find("PLAYER1"))
			m_players[1] = GameObject.Find("PLAYER1").transform;
		else
			m_players[1] = GameObject.Find("PLAYER1_concept").transform;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		m_current = m_players[0].transform;
		transform.position = m_current.position;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(state=="idle")
		{
			//change active player
			if(m_change_player && m_players[m_iPlayer]!=null)
			{
				m_target = m_players[m_iPlayer].transform;
				bChangePos=true;
			}
			//follow the player
			else if(m_current)
				transform.position = Vector3.SmoothDamp(transform.position, m_current.position, ref velocity, smoothTime);
				
			//Player change
			if(bChangePos)
				ChangeActivePlayer();
		}
		//Movemos la cámara hacia la plataforma que se está moviendo
		else if(state=="view_activator")
		{
			Vector3 newPos = new Vector3(m_activatorPos.x, m_activatorPos.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTimeView);
			currentTime = Time.time-startTime;
			dist = Vector3.Distance(transform.position, newPos);
			
			//Cuando este suficientemente cerca ya no hace falta acercarse mas
			if(dist < 0.5f)
			{
				state="viewing";
				startTime=Time.time;
			}
			//Si tarda demasiado en llegar (por culpa de max,min) hacemos que vuelva
			else if(currentTime > 4.0f)
				state="idle";
				
		}
		//Esperamos un tiempo antes de hacer volver la cámara a su sitio
		else if(state=="viewing")
		{
			currentTime = Time.time-startTime;
			if(currentTime > 1.5f)
				state="idle";
		}
		
		////////////////////////////
		//UNA VEZ TERMINADA LA FASE DE PRUEBAS HABRA QUE PONER ESTO EN EL AWAKE!!!
		////////////////////////////
		//Calulamos el minY y el maxY adecuado en función del ratio actual
		ratio = (float)Screen.width/(float)Screen.height;
		inv_ratio = Mathf.InverseLerp(ratio_16_9, ratio_4_3, ratio);
		
		minY = Mathf.Lerp(minY_16_9, minY_4_3, inv_ratio);
		maxY = Mathf.Lerp(maxY_16_9, maxY_4_3, inv_ratio);
		//////////////
		
		//check camera limits
		if(m_check_bounds)
			CheckBounds();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Move camera to next player
	void ChangeActivePlayer()
	{	
		if(m_target!=null)
		{
			transform.position = Vector3.SmoothDamp(transform.position, m_target.position, ref velocity, smoothTime);
				
			if(Vector3.Distance(transform.position, m_target.position) < 0.01f)
			{
				m_current = m_target;
				bChangePos = false;
				m_change_player = false;
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Check level bounds
	void CheckBounds()
	{
		float newX = Mathf.Clamp(transform.position.x, minX, maxX);
		float newY = Mathf.Clamp(transform.position.y, minY, maxY);
		
		transform.position = new Vector3(newX, newY, transform.position.z);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Message from GameManager
	void ChangePlayer()
	{
		m_change_player=true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Message from GameManager
	void SetPlayerActive(int id)
	{
		m_iPlayer = id;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Cuando se pulsa un activador la cámara se mueve hasta el control que se ha activado
	//para que el jugador pueda ver lo que está ocurriendo
	
	//Message from ActivatorManager
	void LookAtActivator(Vector2 targetPos)
	{
		state = "view_activator";
		m_activatorPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
		startTime = Time.time;
	}
}












