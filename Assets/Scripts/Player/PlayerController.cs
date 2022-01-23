using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	//public GameController gameController;

	bool receivingMovementInput;

	[Header("Player Settings")]
	public static PlayerController instance;

	[SerializeField] CharacterController rb;

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

	// Initialization
	private void Awake()
	{
		instance = this;
		originalPos = playerTransform.position;
	}

	void Start()
	{
		StartCoroutine(PositionTracking());
	}

	public IEnumerator PositionTracking()
	{
		if (isSafe && rb.isGrounded)
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
		ApplyMovement();
		IsGrounded = rb.isGrounded;

	}

	public bool IsGrounded
    {
        set
        {
			if(value != isGrounded)
            {
				value = rb.isGrounded;
				characterEvents.onGrounded.Invoke();

				if(value == true)
                {
					movement.isMidair = false;
				}
			}
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
		}

		Vector2 desiredVelocity = new Vector2(input, rb.velocity.y);
		desiredVelocity *= movement.maxSpeed;

		//Midair
		if (movement.isMidair)
		{
			if(movement.canControlMidAir)
            {
				desiredVelocity = Vector2.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * movement.airControlDamping);
				movement.MoveVector = new Vector3(desiredVelocity.x, desiredVelocity.y, 0);
			}

			else
            {
				//desiredVelocity = new Vector2(); Mathf.Lerp(movement.CurrentSpeed, moveAxis.x, Time.deltaTime * movement.airControlDamping);
				//movement.MoveVector = new Vector3(effectiveMovement, 0, 0);
			}
			movement.MoveVector = transform.TransformDirection(movement.MoveVector);
		}

		//On Ground
		else if (rb.isGrounded)
		{
			desiredVelocity = new Vector2(Mathf.LerpUnclamped(rb.velocity.x, desiredVelocity.x, movement.controlDamping * Time.deltaTime), rb.velocity.y);
			movement.MoveVector = new Vector3(desiredVelocity.x, desiredVelocity.y, 0);
			movement.MoveVector = transform.TransformDirection(movement.MoveVector);
		}
		moveDirection += movement.gravity * Time.deltaTime;
		moveDirection = new Vector3(movement.MoveVector.x, moveDirection.y, movement.MoveVector.z);

		rb.Move(moveDirection * Time.deltaTime);
	}

	

	public void Jump(InputAction.CallbackContext callbackContext)
	{
		if(callbackContext.performed)
        {
			
			if (rb.isGrounded)
            {
				movement.isMidair = true;
				moveDirection.y = Mathf.Sqrt(movement.jumpHeight * -3.0f * movement.gravity.y);
				characterEvents.onJumped.Invoke();
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
			RaycastHit2D hit = Physics2D.Raycast(new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z), Vector2.right * (isFacingRight ? 1 : -1), _raycast.raycastDistance);
			Debug.DrawRay(new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z), Vector2.right * (isFacingRight ? 1 : -1), _raycast.raycastColour, _raycast.raycastDistance);
			if (Physics2D.Raycast(new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z), Vector2.right * (isFacingRight ? 1 : -1), _raycast.raycastDistance))
			{

				//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
				if (hit.collider.TryGetComponent(out InteractableDimensionObject interactable))
				{
					interactable.DebugRaycastHit();
				}

			}
		}

	}
}

[System.Serializable]
public class RaycastDetails
{
	public bool useRaycast;
	[Tooltip("This decides where the raycast comes from Leave this variable blank for it to default to this gameobject.")]
	public Transform raycastPoint;
	public float raycastDistance;
	public Color raycastColour;
}

[System.Serializable]
public class MovementDetails
{
	public Vector3 gravity = new Vector3(0, 20.0f, 0);

	[Space(10)]

	[Range(1.25f,15f)] public float controlDamping = 7f;
	public float maxSpeed = 6;
	public float jumpHeight = 8;

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

