using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairAnchor : MonoBehaviour
{

    Vector2 rootAnchorOffset;
   
    public Vector2 partOffset = Vector2.zero;
    public float lerpSpeed = 20f;
    [SerializeField] float fallVelocityBuffer = -.5f;

    private Transform[] hairParts;
    private Transform hairAnchor;

    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerController playerController;

    [Space(10)]

    public Vector2 rootOffset = Vector2.zero;
    public Transform followPoint;

    [Header("Hair 'Juice'")]
    [SerializeField] bool canWiggle;
    [SerializeField] bool wiggleWhenIdle;
    [SerializeField] WiggleVariables idleWiggleVariables;
    [SerializeField] WiggleVariables movingWiggleVariables;
 

    [Header("Hair Offsets (Assume facing right)")]
    [SerializeField] bool isIdleOffset;
    [SerializeField] private Vector2 idleOffset = new Vector2(-0.01f, -0.1f);
    [SerializeField] private Vector2 runOffset = new Vector2(-0.1f, -0.01f);
    [SerializeField] private Vector2 jumpOffset = new Vector2(-0.01f, -0.1f);
    [SerializeField] private Vector2 fallOffset = new Vector2(-0.01f, 0.1f);

    private void Awake() 
    {
        playerController.UpdateEvent.AddListener(delegate { UpdateHairOffset(); });
        rootAnchorOffset = transform.localPosition;

        hairAnchor = GetComponent<Transform>();
        hairParts = GetComponentsInChildren<Transform>();
    }

    private void UpdateHairOffset()
    {
        Vector2 currentOffset = Vector2.zero;
        transform.localPosition = new Vector2(-rootAnchorOffset.x,rootAnchorOffset.y);

        // idle
        if ( playerController.MoveAxis.x == 0 && playerController.PlayerCharacterController.velocity.y == 0 || playerController.IsHittingWall && playerController.IsGrounded)
        {
            currentOffset = idleOffset;
            isIdleOffset = true;
        }
        // jump
        else if (playerController.MoveAxis.y > 0)
        {
            currentOffset = jumpOffset;
            isIdleOffset = false;
        }
        // fall
        else if (playerController.IsFalling)
        {
            currentOffset = fallOffset;
            isIdleOffset = false;
        }
        // run
        else if (playerController.MoveAxis.x != 0)
        {
            currentOffset = runOffset;
            isIdleOffset = false;
        }

        else
        {
            currentOffset = idleOffset;
            isIdleOffset = true;
        }

        // flip x offset direction if we're facing left
        if (!playerController.IsFacingRight)
        {
            transform.localPosition = new Vector2(rootAnchorOffset.x + rootOffset.x, rootAnchorOffset.y + rootOffset.y);
            currentOffset.x = currentOffset.x * -1;
        }

        else
        {
            transform.localPosition = new Vector2(-rootAnchorOffset.x + rootOffset.x, rootAnchorOffset.y + rootOffset.y);
        }

        partOffset = currentOffset;
    }

    private void Update() 
    {
        //The point the hair moves to (for animations)
        if(followPoint!=null)
        {
            rootOffset = followPoint.transform.localPosition - new Vector3( rootAnchorOffset.x, rootAnchorOffset.y,0);
        }

            Transform pieceToFollow = hairAnchor;

        foreach(Transform hairPart in hairParts)
        {
            // make sure we're not including the hair anchor, only the hair parts
            if (!hairPart.Equals(hairAnchor))
            {
                

                Vector2 targetPosition = (Vector2) pieceToFollow.position + (partOffset + WiggleOffset);
                Vector2 newPositionLerped = Vector2.Lerp(hairPart.position, targetPosition, Time.deltaTime * lerpSpeed);
               
                hairPart.position = newPositionLerped;
                pieceToFollow = hairPart;
            }
        }
    }


    Vector2 WiggleOffset
    {
        get
        {
            Vector2 wiggleOffset = Vector2.zero;
            if(canWiggle)
            {
                if (wiggleWhenIdle && isIdleOffset)
                {
                    wiggleOffset = new Vector2(idleWiggleVariables.wiggleBounds.x * Mathf.Sin(Time.time * idleWiggleVariables.wiggleSpeed) * idleWiggleVariables.wiggleAmount, idleWiggleVariables.wiggleBounds.y * Mathf.Sin(Time.time * idleWiggleVariables.wiggleSpeed) * idleWiggleVariables.wiggleAmount);
                }

                else if (!isIdleOffset)
                {
                    wiggleOffset = new Vector2(movingWiggleVariables.wiggleBounds.x * Mathf.Sin(Time.time * movingWiggleVariables.wiggleSpeed) * movingWiggleVariables.wiggleAmount, movingWiggleVariables.wiggleBounds.y * Mathf.Sin(Time.time * movingWiggleVariables.wiggleSpeed) * movingWiggleVariables.wiggleAmount);
                }
            }
            
            return wiggleOffset;
        }
    }
}

[System.Serializable]
public class WiggleVariables
{
    public Vector2 wiggleBounds;
    public float wiggleSpeed;
    public float wiggleAmount;
}
