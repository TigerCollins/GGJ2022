using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    public UnityEvent onJump;
    public UnityEvent onGrounded;
    public UnityEvent onNoLongerGrounded;
    public UnityEvent<PlayerController.MovementState> onMoveInputStateChange;

    [Header("Debug")]
    [SerializeField] bool groundedState;
    [SerializeField] bool playerFacingRight;
    [SerializeField] PlayerController.MovementState movementState;

    public bool IsGrounded
    {
        set
        {
            if(value != groundedState)
            {
                groundedState = value;
                animator.SetBool("Grounded", value);
                if (value)
                {
                    onGrounded.Invoke();
                }
               

                else
                {
                    onNoLongerGrounded.Invoke();
                }
                
            }
        }
    }

    public void Init()
    {
        onJump.AddListener(delegate { animator.SetTrigger("Jump");});
       // onGrounded.AddListener(delegate { AttemptIdleAnimationState(); });
        onMoveInputStateChange.AddListener(MovementStateChange);
    }

    private void Update()
    {
        //Grounded
       // animator.SetBool("Grounded", groundedState);
        DirectionChange();
        if(!PlayerController.instance.IsGrounded)
        {
            animator.SetTrigger("Jump");
        }
    }

    public void MovementStateChange(PlayerController.MovementState state)
    {
        if(state != movementState &&  PlayerController.instance.IsGrounded)
        {
            movementState = state;
            if (PlayerController.instance.CanMove)
            {
                animator.SetInteger("AnimState", (int)state);
            }

            else
            {

            }
         
          
        }       
    }

    public void AttemptIdleAnimationState()
    {
        if(PlayerController.instance.IsGrounded)
        {
            onMoveInputStateChange.Invoke(PlayerController.MovementState.idle);
        }
    }

    void DirectionChange()
    {
        if(PlayerController.instance.IsFacingRight != playerFacingRight)
        {
            playerFacingRight = PlayerController.instance.IsFacingRight;
            spriteRenderer.flipX = playerFacingRight;
        }
    }
}
