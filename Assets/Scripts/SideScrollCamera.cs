using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;
    [SerializeField] Transform cameraTarget;

    [Header("Camera Movement Settings")]
    [Tooltip("The lower the number, the shorter the delay. 0 for instant follow.")]
    [SerializeField]float cameraDelay = .2f;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,2.5f,-9f);
    Vector3 velocity = Vector3.zero;

    [Space(10)]
    [SerializeField] bool moveDirectionChangesXOffset;
    [SerializeField] float moveDirectionXOffset;

    void FixedUpdate()
    {
        CameraLogic();
    }

    void CameraLogic()
    {
        float effectiveXOffset = cameraOffset.x;
        if (moveDirectionChangesXOffset)
        {
            if(PlayerController.instance.IsFacingRight)
            {
                effectiveXOffset = moveDirectionXOffset;
            }

            else
            {
                effectiveXOffset = -moveDirectionXOffset;
            }
        }

        cameraObject.transform.position = Vector3.SmoothDamp(cameraObject.transform.position, new Vector3(cameraTarget.position.x + effectiveXOffset, cameraTarget.position.y + cameraOffset.y, cameraTarget.position.z + cameraOffset.z), ref velocity, cameraDelay);

    }
}
