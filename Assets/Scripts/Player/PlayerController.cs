using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
	//public GameController gameController;

	bool receivingMovementInput;
	[SerializeField] bool canMove = true;

	

	[Header("Player Settings")]
	public static PlayerController instance;
	[SerializeField] PlayerAnimation playerAnimator;
	[SerializeField] CharacterController characterController;
	[SerializeField] float physicsPushPower;

	[SerializeField] MovementDetails movement;
	[SerializeField] CharacterEvents characterEvents;
	 Vector3 moveDirection = Vector3.zero;

	

	[SerializeField] bool isFacingRight;

	[Header("Raycast")]
	[SerializeField] RaycastDetails _raycast;

	[Header("Position Tracking")]
	[SerializeField] bool isSafe;
	[SerializeField] Vector3 lastSafePosition;
	[SerializeField] float tickRate;

	[Header("Debug")]
	[SerializeField] Transform playerTransform;
	[SerializeField] Vector2 moveAxis;
	Vector3 originalPos;
	[SerializeField] bool isGrounded;

	public enum MovementState
    {
		idle,
		combatIdle,
		Sprint,
		Jumping,
		Freefall
    }

	// Initialization
	private void Awake()
	{
		instance = this;
		originalPos = playerTransform.position;
		playerAnimator.Init();
	}

	void Start()
	{
		StartCoroutine(PositionTracking());
	}

	public IEnumerator PositionTracking()
	{
		if (isSafe && IsGrounded)
		{
			lastSafePosition = transform.position;
		}

		yield return new WaitForSeconds(tickRate);
		StartCoroutine(PositionTracking());
	}

	// Main tick
	void FixedUpdate()
	{
		//Movement();
		if(canMove)
        {
			ApplyMovement();
		}
		
		IsGrounded = characterController.isGrounded;

	}

    private void Update()
    {
		ForcePlayerHeightToDrop();

	}

    public bool IsGrounded
    {
        set
        {
			if(value != isGrounded)
            {
				value = characterController.isGrounded;
				playerAnimator.IsGrounded = value;
				characterEvents.onGrounded.Invoke();
				isGrounded = value;
				

				if (value == true)
                {
					movement.isMidair = false;
					//playerAnimator.AttemptIdleAnimationState();
				}
			}
        }

		get
        {
			 return characterController.isGrounded;
        }
    }

#if UNITY_EDITOR
	public void EditorResetPosition(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerTransform.position = originalPos;
		}
	}

	public void EditorSlowMo(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Time.timeScale = 0.2f;
		}
	}

	public void EditorNormalTime(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Time.timeScale = 1f;
		}
	}
#endif


	

	void ApplyMovement()
	{

		float input = moveAxis.x;
		if (Mathf.Abs(input) < 0.3f)
		{
			input = 0f;
			playerAnimator.AttemptIdleAnimationState();
			//Animate Run

		}

		else
        {
			if(IsGrounded)
            {
				playerAnimator.onMoveInputStateChange.Invoke(MovementState.Sprint);
			}
			
		}

		Vector2 desiredVelocity = new Vector2(input, characterController.velocity.y);
		desiredVelocity *= movement.maxSpeed;

		//Midair
		if (!IsGrounded)
		{
			if(movement.canControlMidAir)
            {
				desiredVelocity = Vector2.Lerp(characterController.velocity, desiredVelocity, Time.deltaTime * movement.airControlDamping);
				movement.MoveVector = new Vector3(desiredVelocity.x, desiredVelocity.y, 0);
			}

			movement.MoveVector = transform.TransformDirection(movement.MoveVector);
		}

		//On Ground
		else if (IsGrounded)
		{
			desiredVelocity = new Vector2(Mathf.LerpUnclamped(characterController.velocity.x, desiredVelocity.x, movement.controlDamping * Time.deltaTime), characterController.velocity.y);
			movement.MoveVector = new Vector3(desiredVelocity.x, desiredVelocity.y, 0);
			movement.MoveVector = transform.TransformDirection(movement.MoveVector);
		}

		//JUMP (when midair)
		if(movement.isMidair)
        {
			moveDirection += movement.gravity * Time.deltaTime * movement.jumpGravityMultiplier;
		}
		//JUMP (when grounded)
		else
		{
			moveDirection += movement.gravity * Time.deltaTime ;
		}
		
		moveDirection = new Vector3(movement.MoveVector.x, moveDirection.y, movement.MoveVector.z);

		characterController.Move(moveDirection * Time.deltaTime);
	}

	

	public void Jump(InputAction.CallbackContext callbackContext)
	{
		if(callbackContext.performed)
        {
			
			if (IsGrounded)
            {
				movement.isMidair = true;
				movement.HeightWhenJumped = transform.position.y;
				movement.JumpTarget = Mathf.Sqrt(movement.jumpHeight * -3.0f * movement.gravity.y);
				moveDirection.y = movement.JumpTarget;
				characterEvents.onJumped.Invoke();
				playerAnimator.onJump.Invoke();
			}
			
		}
	}

	public void MovementVector(InputAction.CallbackContext callbackContext)
	{
		moveAxis = callbackContext.ReadValue<Vector2>();
		

		//Facing Direction
		if (moveAxis.x < 0)
		{
			isFacingRight = false;
			receivingMovementInput = true;
		}

		else if(moveAxis.x >0)
        {
			isFacingRight = true;
			receivingMovementInput = true;

		}

		else
        {
			receivingMovementInput = false;

		}
	}

	public void Raycast(InputAction.CallbackContext context)
	{

		if (context.performed && _raycast.useRaycast)
		{
			bool isHitting = false;
			RaycastHit hit;
			Vector3 origin = new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z);
			Debug.DrawRay(origin, Vector2.right * (isFacingRight ? 1 : -1) * _raycast.raycastDistance, _raycast.aboveCheckRaycastColour);
			if (Physics.Raycast(origin, Vector2.right * (isFacingRight ? 1 : -1), out hit, _raycast.raycastDistance, _raycast.raycastMask))
			{
				//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
				if (hit.collider.TryGetComponent(out InteractableDimensionObject interactable))
				{
					interactable.DebugRaycastHit();
				}
			}
		}
	}

	public bool CheckInputState()
    {
		return receivingMovementInput;
    }

	public bool IsFacingRight
    {
        get
        {
			return isFacingRight;
        }
    }


	void ForcePlayerHeightToDrop()
    {
		if(HitObjectAbove() == true)
        {
			moveDirection.y = 0;
        }
	}
	public bool HitObjectAbove()
    {
		bool isHitting = false;
		RaycastHit hit;
		Vector3 origin = new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z);
		Debug.DrawRay(origin, transform.TransformDirection(Vector2.up) * _raycast.aboveCheckDistance, _raycast.aboveCheckRaycastColour);
		if (Physics.Raycast(origin,transform.TransformDirection(Vector2.up),out hit, _raycast.aboveCheckDistance,_raycast.raycastMask))  
		{
			//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
			if (hit.transform != null)
			{
				isHitting = true;
			}

		}
		return isHitting;
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{


		/*if (hit.collider.TryGetComponent(out Projectile projectile))
		{
			if (!projectile.isInAnimation)
			{
				RigidBodyPhysics(hit);
			}
		}

		else
		{*/
			RigidBodyPhysics(hit);
		//}




	}

	public void RigidBodyPhysics(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;


		// no rigidbody
		if (body == null || body.isKinematic)
			return;

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3f)
			return;

		// Calculate push direction from move direction,
		// we only push objects to the sides never up and down
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// Apply the push
		body.velocity = pushDir * physicsPushPower;

	}
}

[System.Serializable]
public class RaycastDetails
{
	public bool useRaycast;
	public LayerMask raycastMask;
	[Tooltip("This decides where the raycast comes from Leave this variable blank for it to default to this gameobject.")]
	public Transform raycastPoint;
	public float raycastDistance;
	public Color raycastColour;

	[Space(10)]

	public float aboveCheckDistance = .5f;
	public Color aboveCheckRaycastColour;
}

[System.Serializable]
public class MovementDetails
{
	public Vector3 gravity = new Vector3(0, 20.0f, 0);
	public float jumpGravityMultiplier = 10;

	[Space(10)]

	[Range(1.25f,15f)] public float controlDamping = 7f;
	public float maxSpeed = 6;
	public float jumpHeight = 8;
	float heightWhenJumped;
	float jumpTarget;

	Vector3 moveVector;

	[Space(10)]

	public bool isMidair;
	public bool canControlMidAir = true;
	[Range(1.25f, 15f)] public float airControlDamping = 2.5f;

	public Vector3 MoveVector
	{
		set
		{
			moveVector = value;
		}

		get
		{
			return moveVector;
		}
	}

	public float HeightWhenJumped
    {
		get
        {
			return heightWhenJumped;

		}

        set
        {
			heightWhenJumped = value;
        }
    }

	public float JumpTarget
    {
		get
        {
			return jumpTarget;
        }

		set
        {
			jumpTarget = value;
        }
    }
}

[System.Serializable]
public class CharacterEvents
{
	[Header("Jump")]
	public UnityEvent onGrounded;
	public UnityEvent onJumped;

	[Header("Life")]
	public UnityEvent onDeath;
	public UnityEvent onKill;
	public UnityEvent onRespawnOrSpawn;
}

