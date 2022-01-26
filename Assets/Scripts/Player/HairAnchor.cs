using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairAnchor : MonoBehaviour
{
    
    Vector2 rootAnchorOffset;
    public Vector2 partOffset = Vector2.zero;
    public float lerpSpeed = 20f;

    private Transform[] hairParts;
    private Transform hairAnchor;

    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerController playerController;

    [Header("Hair Offsets (Assume facing right)")]
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
        }
        // jump
        else if (playerController.MoveAxis.y > 0)
        {
            currentOffset = jumpOffset;
        }
        // fall
        else if (playerController.PlayerCharacterController.velocity.y < -.03f)
        {
            currentOffset = fallOffset;
        }
        // run
        else if (playerController.MoveAxis.x != 0)
        {
            currentOffset = runOffset;
        }

        // flip x offset direction if we're facing left
        if (!playerController.IsFacingRight)
        {
            transform.localPosition = new Vector2(rootAnchorOffset.x, rootAnchorOffset.y);
            currentOffset.x = currentOffset.x * -1;
        }

        partOffset = currentOffset;
    }

    private void Update() 
    {
        Transform pieceToFollow = hairAnchor;

        foreach(Transform hairPart in hairParts)
        {
            // make sure we're not including the hair anchor, only the hair parts
            if (!hairPart.Equals(hairAnchor))
            {
                Vector2 targetPosition = (Vector2) pieceToFollow.position + partOffset;
                Vector2 newPositionLerped = Vector2.Lerp(hairPart.position, targetPosition, Time.deltaTime * lerpSpeed);
               
                hairPart.position = newPositionLerped;
                pieceToFollow = hairPart;
            }
        }
    }

}
