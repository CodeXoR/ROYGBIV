using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class MyCharacterController : MonoBehaviour 
{
	// States - Player
	public enum PlayerStates { Normal = 0, HasObject = 1 }
	public PlayerStates state = PlayerStates.Normal;
	// Gameobject References
	public LaserGun laserGun;
	public GameObject carriedObject;
	public GameObject footHold;
	public GameObject playerIndicator;
	public GameObject arrowIndicator;
	// Transform && Vector3 References
	public Transform pickupPos;
	public Vector3 moveDirection = Vector3.zero;
	// Component References
	public Animator anim;
	CharacterController controller;
	// Gameplay Variables
	public float speed = 6.0f;
	public float jumpSpeed = 1.0f;
	public float gravity = 20.0f;
	public float turnSpeed = 20.0f;
	public bool isGrounded;
	// Collision LayerMask
	public LayerMask hitMask;
	// Current Area Name && last Area Name
	public string lastArea;
	public string currentArea;
	
	void Start()
	{
		lastArea = "Area1";
		currentArea = "";
		controller = GetComponent<CharacterController>();
		StartCoroutine (PlayerUpdate ());
	}
	
	public IEnumerator PlayerUpdate () 
	{
		while(true)
		{
			//Debug.Log(currentArea);
			if(GameManager.Get().gameState == GameManager.GameStates.Playing)
			{
				if(state==PlayerStates.Normal)
				{
					ApplyMoveInputs();
					CheckCollision ();
					yield return 0;
				}
				else if(state==PlayerStates.HasObject)
				{
					ApplyMoveInputs();
					if(Input.GetMouseButtonDown(0))
					{
						DropObject();
						yield return 0;
					}
					else
						yield return 0;
				}
				else
					yield return 0;
			}
			else
			{
				StopAnimations();
				yield return 0;
			}
		}
	}

	void StopAnimations()
	{
		anim.SetBool("Jump", false);
		anim.SetBool("Jump", false);
		anim.SetFloat ("Direction", 0);
		anim.SetFloat ("Speed", 0);
	}

	void ApplyMoveInputs()
	{
		float horiz = Input.GetAxis("Horizontal");
		float verti = Input.GetAxis("Vertical");

		isGrounded = CheckGround ();

		if (isGrounded) 
		{
			if(anim.GetBool("Jump")==true)
				anim.SetBool("Jump",false);

			bool hittingCrate = false;

			if(state==PlayerStates.HasObject)
				hittingCrate = CheckSorrounding();

			horiz = hittingCrate == true ? 0 : horiz;

			transform.Rotate(0, horiz * turnSpeed * Time.deltaTime, 0);

			// if moving forward move at player movespeed
			// if moving backward move at approximately 1/3 of player movespeed
			moveDirection = verti > 0 ? transform.forward * verti * speed :
				transform.forward * verti * speed * .35f;
			
			moveDirection *= speed;

			if(state==PlayerStates.HasObject)
			{
				if(Input.GetMouseButton(1))
				{
					DropObject();
					anim.SetBool("Aim", true);
				}
			}

			if(state==PlayerStates.Normal)
			{
				if(Input.GetButton("Jump"))
				{
					moveDirection.y = jumpSpeed;
				}
			}
		}

		if(Input.GetMouseButton(1))
			anim.SetBool("Aim", true);
		
		if(Input.GetMouseButtonUp(1))
			anim.SetBool("Aim", false);

		if(anim.GetBool("Aim")==true)
		{
			laserGun.ActivateLaserGuide();
			if(Input.GetMouseButtonDown(0))
				laserGun.ShootProjectile();
		}

		if(anim.GetBool("Aim")==false)
			laserGun.DeactivateLaserGuide();
		
		if(!isGrounded)
		{
			if(anim.GetBool("Jump")==false)
				anim.SetBool("Jump",true);
		}

		// pseudo gravity
		moveDirection.y -= gravity * Time.deltaTime;

		Vector3 moveMag = new Vector3 (moveDirection.x, 0, moveDirection.z);
		float magnitude = moveMag.magnitude * verti;
		anim.SetFloat ("Direction", horiz);
		anim.SetFloat ("Speed", magnitude);

		if(controller.enabled)
			controller.Move(moveDirection * Time.deltaTime);
	}
	
	void PickUpObject(GameObject obj)
	{
		if(carriedObject == null)
		{
			carriedObject = obj;
			Vector3 dir = (obj.transform.position - transform.position).normalized;
			if(Vector3.Dot(dir,transform.forward) >= 0.00001f)
				transform.rotation = Quaternion.LookRotation(-transform.forward);
			carriedObject.transform.parent = gameObject.transform;
			carriedObject.transform.position = new Vector3( pickupPos.position.x,
			                                                carriedObject.transform.position.y,
			                                                pickupPos.position.z);
			carriedObject.transform.rotation = pickupPos.transform.rotation;
		}
	}
	
	void DropObject()
	{
		if(carriedObject != null)
		{
			carriedObject.GetComponent<PickUp>().state = PickUp.PickUpStates.CanPickUp;
			carriedObject.transform.parent = null;
			carriedObject = null;
			state = PlayerStates.Normal;
		}
	}

	bool CheckGround()
	{
		RaycastHit hitN;
		// Debug Ray
		//Debug.DrawRay (transform.position+transform.up*.1f, -transform.up * .2f, Color.green);
		if(Physics.Raycast(transform.position+transform.up*.1f, -transform.up, out hitN, .2f))
		{
			footHold = hitN.transform.gameObject;
			return true;
		}
		else if(!Physics.Raycast(transform.position+transform.up*.1f, -transform.up, .2f))
		{
			footHold = null;
			return false;
		}
		else
			return false;
	}

	void CheckCollision()
	{
		Collider[] hits = Physics.OverlapSphere (transform.position, .5f, hitMask);

		if(hits.Length==0)
			arrowIndicator.GetComponent<Canvas>().enabled = false;
			
		if( hits.Length > 0 )
		{
			// first hit is an object that can be picked up
			if(hits[0].gameObject.layer == LayerMask.NameToLayer("ReflectiveObject"))
			{
				// reference to pcikup script 
				PickUp obj = hits[0].gameObject.GetComponent<PickUp>();

				// gameobject doesn't have a pickup component
				// cutoff following statements
				if( obj == null )
					return;

				// if collided with a pickup object while aiming
				if(anim.GetBool("Aim")==true)
				{
					// pushback character if arm is shooting inside reflective block
					Vector3 pushBackDir = transform.position - hits[0].ClosestPointOnBounds(transform.position);
					transform.position = Vector3.Lerp(transform.position, 
					                                  transform.position+pushBackDir, Time.deltaTime*3.0f);
				}

				// if colliding with a carried gameobject
				// do not show arrow indicator 
				if(obj.state == PickUp.PickUpStates.Carried )
					arrowIndicator.GetComponent<Canvas>().enabled = false;

				// if colliding with a non-carried gameobject
				// show arrow indicator
				if(obj.state == PickUp.PickUpStates.CanPickUp &&
				   footHold != null && footHold.layer == LayerMask.NameToLayer("Ground"))
				{
					arrowIndicator.GetComponent<Canvas>().enabled = true;
					arrowIndicator.transform.position = obj.indicatorPos.position;
					arrowIndicator.transform.rotation = obj.indicatorPos.rotation;
				}

				if(obj.state == PickUp.PickUpStates.CanPickUp &&
				   footHold != null && footHold.layer != LayerMask.NameToLayer("Ground"))
					arrowIndicator.GetComponent<Canvas>().enabled = false;

				// if player is on ground &&
				// if player is not aiming
				// if player presses an input, pickup object colliding with player
				if( footHold != null && footHold.layer == LayerMask.NameToLayer("Ground") &&
					anim.GetBool("Aim")==false )
				{
					if( Input.GetMouseButtonDown(0) )
					{
						PickUpObject(obj.gameObject);
						obj.state = PickUp.PickUpStates.Carried;
						state = PlayerStates.HasObject;
						arrowIndicator.GetComponent<Canvas>().enabled = false;
					}
				}
			}
		}
	}

	bool CheckSorrounding()
	{
		/* // drawRays for debugging
		// right side raycast
		Debug.DrawRay (carriedObject.transform.position+carriedObject.transform.forward*.7f-carriedObject.transform.right*.5f, 
		               -carriedObject.transform.right * .35f, Color.red);
		Debug.DrawRay (carriedObject.transform.position-carriedObject.transform.right*.5f, 
		               -carriedObject.transform.right * .35f, Color.red);
		Debug.DrawRay (carriedObject.transform.position+carriedObject.transform.forward*1.4f-carriedObject.transform.right*.5f, 
		               -carriedObject.transform.right * .35f, Color.red);
		// left side raycast
		Debug.DrawRay (carriedObject.transform.position+carriedObject.transform.forward*.7f+carriedObject.transform.right*.5f, 
		               carriedObject.transform.right * .35f, Color.red);
		Debug.DrawRay (carriedObject.transform.position+carriedObject.transform.right*.5f, 
		               carriedObject.transform.right * .35f, Color.red);
		Debug.DrawRay (carriedObject.transform.position+carriedObject.transform.forward*1.4f+carriedObject.transform.right*.5f, 
		               carriedObject.transform.right * .35f, Color.red);
		*/

		Ray rayE1 = new Ray(carriedObject.transform.position-carriedObject.transform.right*.5f, -carriedObject.transform.right);
		Ray rayE2 = new Ray(carriedObject.transform.position+carriedObject.transform.forward*1.4f-carriedObject.transform.right*.5f, -carriedObject.transform.right);
		Ray rayE3 = new Ray(carriedObject.transform.position+carriedObject.transform.forward*.7f-carriedObject.transform.right*.5f, -carriedObject.transform.right);
		Ray rayW1 = new Ray(carriedObject.transform.position+carriedObject.transform.right*.5f, carriedObject.transform.right);
		Ray rayW2 = new Ray(carriedObject.transform.position+carriedObject.transform.forward*1.4f+carriedObject.transform.right*.5f, carriedObject.transform.right);
		Ray rayW3 = new Ray(carriedObject.transform.position+carriedObject.transform.forward*.7f+carriedObject.transform.right*.5f, carriedObject.transform.right);

		float x = Input.GetAxis ("Horizontal");
		// if turning right activate right face raycasts
		if( x > 0 )
		{
			if(Physics.Raycast(rayE1, .35f))
				return true;
			if(Physics.Raycast(rayE2, .35f))
				return true;
			if(Physics.Raycast(rayE3, .35f))
				return true;
			else
				return false;
		}
		// if turning left activate left face raycasts
		if( x < 0 )
		{
			if(Physics.Raycast(rayW1, .35f))
				return true;
			if(Physics.Raycast(rayW2, .35f))
				return true;
			if(Physics.Raycast(rayW3, .35f))
				return true;
			else
				return false;
		}

		else
			return false;
	}

	public GameObject CarriedObject { get{return carriedObject;} 
									  set{carriedObject=value;}}
}
