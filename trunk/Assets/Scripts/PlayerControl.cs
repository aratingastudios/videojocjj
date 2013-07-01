
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
	public int id;
    public float speed = 6.0F;
    public float jumpSpeed = 1.5F;
	public float gravity = 20.0F;
	
	public bool isActive;
	public GUISkin m_skin;
	public AudioClip[] sounds;
	public bool bConcept = false;
	public bool bCheckEdges = true;
	public LayerMask layermask = -1;
	
	bool bGoalReached=false;
	
    Vector3 moveDirection = Vector3.zero;
	
	float horiz=0.0f;
	float vert=0.0f;
	
	float goalDist;
	
	GameObject gameManagerObj;
	GUIManager guiManager;
	
	CharacterController controller;
	ActivatorManager activatorManager;
	
	//Mobile->Tamaño de pantalla de referencia: 800x480
	int buttonSize;
	float originalRatio=100.0f/800.0f;
	
	Rect touchLeft;
	Rect touchRight;
	Rect touchJump;
	
	//Moving platform support
	Transform activePlatform;
	Vector3 activeLocalPlatformPoint;
	Vector3 activeGlobalPlatformPoint;
	
	Material mat_child;
	
	Ray ray;
	RaycastHit hit;
	
	Animation animation_child;
	bool bSendGoalReached;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{	
		controller = GetComponent<CharacterController>();
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
		guiManager = gameManagerObj.GetComponent<GUIManager>();
		
		//if(transform.GetChildCount() > 0)
		//	mat_child = transform.GetChild(0).renderer.material;
		
		if(transform.GetChildCount() > 0)
			animation_child = transform.GetChild(0).animation;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin=m_skin;
		
		if(guiManager.gui_state == "in_game")
		{
			if(isActive && !bGoalReached)
			{
				GUI.Box(touchLeft, "", "arrow_left");
				GUI.Box(touchRight, "", "arrow_right");
				GUI.Box(touchJump, "", "arrow_up");
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//////////////////////////
		//ESTO LO PONDREMOS EN EL START DESPUES DE LA FASE DE PRUEBAS
		//////////////////////////
		buttonSize = (int)(Screen.width * originalRatio);
		touchLeft  = new Rect(5,Screen.height-buttonSize-5,buttonSize,buttonSize);
		touchRight = new Rect(buttonSize+10,Screen.height-buttonSize-5,buttonSize,buttonSize);
		touchJump  = new Rect(Screen.width-buttonSize-5,Screen.height-buttonSize-5,buttonSize,buttonSize);
		//////////////////////////
		
		if(guiManager.gui_state == "in_game")
		{
			//Moving platform support
			if(activePlatform != null)
			{
				Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
				Vector3 moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
				transform.position = transform.position + moveDistance;
			}
			
			activePlatform = null;	
			CheckMovingPlatforms();
			
			if(isActive && !bGoalReached)
			{	
				//Mobile controls
				if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
				{
					foreach(Touch touch in Input.touches)
					{
						Vector2 pos = new Vector2(touch.position.x, Screen.height-touch.position.y);
						
						if(touchLeft.Contains(pos))
						{
							horiz=-1.0f;
							PlaySounds("run");
						}
						else if(touchRight.Contains(pos))
						{
							horiz=1.0f;
							PlaySounds("run");
						}
						else if(touchJump.Contains(pos))
						{
							vert=1.0f;
							PlaySounds("jump");
						}
					}
				}
				//PC controls
				else
				{
					horiz = Input.GetAxis("Horizontal");
					if(Input.GetButton("Jump"))
						vert=1.0f;
					
					//Play sounds
					PlaySounds("");
				}
			
				//rotate character to face direction when move
				if(Mathf.Abs(horiz)>0.1f)
				{
					transform.forward = Vector3.Normalize(new Vector3(horiz, 0.0f, 0.0f));
					if(animation_child && animation_child.GetClipCount() > 0 && controller.isGrounded && !animation_child.IsPlaying("jump"))
						animation_child.Play("walk");
				}
				else
				{
					if(animation_child && animation_child.GetClipCount() > 0 && controller.isGrounded && !animation_child.IsPlaying("jump"))
						animation_child.Play("idle");
				}
				
				//face to camera when idle
				//else
				//	transform.forward = Vector3.Normalize(new Vector3(0.0f, 0.0f, -1.0f));
			}
			
	        if(controller.isGrounded)
			{
				moveDirection = new Vector3(horiz*speed, vert*jumpSpeed*speed, 0.0f);
				
				if(CheckEdge())
					moveDirection = transform.forward * speed;
				
				if(vert>0.95f && animation_child && !animation_child.IsPlaying("jump"))
					animation_child.Play("jump");
			}
			else
				moveDirection.x = horiz*speed;

				
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
						
			//..end of movement..//
			
			horiz=0.0f;
			vert=0.0f;
			
			//Moving platforms support
			if(activePlatform != null)
			{
				activeGlobalPlatformPoint = transform.position;
				activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);
			}
			
			//Colocamos los 2 personajes en distintos carriles para que no choquen
			if(!bGoalReached)
			{
				if(isActive)
					transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
				else
					transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
			}
			
			if(bSendGoalReached && !animation_child.IsPlaying("portal") && !audio.isPlaying)
			{
				bSendGoalReached=false;
				gameManagerObj.SendMessage("GoalReached", id);
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Check if player is in the edge of a moving or static platform
	bool CheckEdge()
	{
		if(bCheckEdges && activePlatform == null) //avoid problems with moving platforms
		{
			bool b = Physics.Raycast(transform.position, Vector3.down, 1.0f, layermask);
			return !b;
		}
		else
			return false;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void PlaySounds(string sInputMobile)
	{
		if(!bConcept)
		{
			if(controller.isGrounded)
			{
				if(Input.GetButton("Jump") || sInputMobile=="jump")
					PlaySound(1); //jump
				
				else if(Input.GetButton("Horizontal") || sInputMobile=="run")
					PlaySound(0); //run
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
	void PlaySound(int id)
	{
		if(guiManager.bAudioFx)
		{
			if(id==1)
				audio.Stop();
			
			if(sounds.Length>0 && sounds[id] && !audio.isPlaying)
			{
				audio.clip = sounds[id];
				audio.Play();
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Message from GameManager
	void SetActivePlayer(SetActiveOptions options)
	{
		if(!bGoalReached)
		{
			isActive = options.bSetActive;
			
			if(!bConcept)
			{
				if(options.bPlayAudio && isActive)
					PlaySound(2);
				
				if(!isActive)
					animation_child.Play("idle");
				
				/*
				if(isActive)
					mat_child.color = Color.white;
				else
					mat_child.color = Color.grey;
				*/
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnTriggerEnter(Collider collider)
	{	
		if(collider.name=="Goal")
		{
			bGoalReached=true;
			transform.position = new Vector3(transform.position.x, transform.position.y, 1.0f);
			collider.gameObject.SendMessage("PlayPortalSound");
			animation_child.Play("portal");
			bSendGoalReached=true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Nos aseguramos de que el player está encima de una plataforma
	//No lo hacemos en el "OnControllerColliderHit" pq no funciona correctamente
	void CheckMovingPlatforms()
	{
		ray = new Ray(transform.position, Vector3.down);
    
        if(Physics.Raycast(ray, out hit, 1.0f))
			if(hit.collider.name.Contains("Elevator") && hit.normal.y > 0.5)
				activePlatform = hit.collider.transform;    
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	//Esta función se ejecuta cuando se llama al Move del CharacterController
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Mejor lo hacemos con un Raycast, aquí no funciona correctamente
		/*
		//Moving platforms support
		if(hit.gameObject.name.Contains("Elevator") && hit.moveDirection.y < -0.9 && hit.normal.y > 0.5)
			activePlatform = hit.collider.transform;    
		*/
			
		//Comprobar choques por arriba
		if(hit.moveDirection.y > 0.9 && hit.normal.y < 0.5)
			if(moveDirection.y > 0.0f)
				moveDirection.y = 0.0f;
		
		//Switch to start the moving platform
		if(hit.gameObject.name.Contains("Activator"))
		{
			activatorManager = hit.gameObject.GetComponent<ActivatorManager>();
			activatorManager.SendMessage("Activate");
		}
	}
}
	

