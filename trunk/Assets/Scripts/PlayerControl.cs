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
	
    Vector3 moveDirection = Vector3.zero;
	
	float horiz=0.0f;
	float vert=0.0f;
	
	float goalDist;
	
	GameObject gameManagerObj;
	GameManager gameManager;
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
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{	
		controller = GetComponent<CharacterController>();
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
		gameManager = gameManagerObj.GetComponent<GameManager>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		buttonSize = (int)(Screen.width * originalRatio);
		
		touchLeft  = new Rect(5,Screen.height-buttonSize-5,buttonSize,buttonSize);
		touchRight = new Rect(buttonSize+10,Screen.height-buttonSize-5,buttonSize,buttonSize);
		touchJump  = new Rect(Screen.width-buttonSize-5,Screen.height-buttonSize-5,buttonSize,buttonSize);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.skin=m_skin;
		
		if(gameManager.gui_state == "in_game")
		{
			if(isActive)
			{
				GUI.Box(touchLeft, "LEFT");
				GUI.Box(touchRight, "RIGHT");
				GUI.Box(touchJump, "JUMP");
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//Moving platform support
		if(activePlatform != null)
		{
			Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
			Vector3 moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
			transform.position = transform.position + moveDistance;
		}
		
		activePlatform = null;
		
		if(isActive)
		{
			//PC controls
			horiz = Input.GetAxis("Horizontal");
			if(Input.GetButton("Jump"))
				vert=1.0f;
			
			//Mobile controls
			foreach(Touch touch in Input.touches)
			{
	            Vector2 pos = new Vector2(touch.position.x, Screen.height-touch.position.y);
				
				if(touchLeft.Contains(pos))
					horiz=-1.0f;
				else if(touchRight.Contains(pos))
					horiz=1.0f;
				else if(touchJump.Contains(pos))
					vert=1.0f;
			}
		}
		
		//rotate character to face direction when move
		if(Mathf.Abs(horiz)>0.1f)
			transform.forward = Vector3.Normalize(new Vector3(horiz, 0.0f, 0.0f));
		//face to camera when idle
		else
			transform.forward = Vector3.Normalize(new Vector3(0.0f, 0.0f, -1.0f));

        if(controller.isGrounded)
		{
			moveDirection = new Vector3(horiz, vert*jumpSpeed, 0.0f);
            moveDirection *= speed;
        }
		else
		{
			moveDirection.x = horiz*speed;
		}
		
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
		
		horiz=0.0f;
		vert=0.0f;
		
		//Moving platforms support
		if(activePlatform != null)
		{
			activeGlobalPlatformPoint = transform.position;
			activeLocalPlatformPoint = activePlatform.InverseTransformPoint (transform.position);
		}
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetActive(bool b)
	{
		isActive=b;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnTriggerEnter(Collider collider)
	{	
		if(collider.name=="Goal")
		{
			gameManagerObj.SendMessage("goalReached", id);
			Destroy(gameObject);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	//Esta función se ejecuta cuando se llama al Move del CharacterController
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Moving platforms support
		if(hit.gameObject.name.Contains("Elevator") && hit.moveDirection.y < -0.9 && hit.normal.y > 0.5)
		{
        	activePlatform = hit.collider.transform;    
    	}
		
		//Switch to start the moving platform
		if(hit.gameObject.name.Contains("Activator"))
		{
			activatorManager = hit.gameObject.GetComponent<ActivatorManager>();
			activatorManager.bActivate=true;
		}
	}
}
	

	
	
	
	
	
	
	
	
	
	
	
	
	