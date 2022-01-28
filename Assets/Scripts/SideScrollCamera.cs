using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;
    [SerializeField] Transform cameraTarget;
    [SerializeField] PlayerController playerController;

    [Header("Camera Movement Settings")]
    [Tooltip("This clamps the movable range from the original player")]
    [SerializeField] bool useCameraBounds;
    [SerializeField] Vector2 cameraXBounds;
    Vector3 originalPosition;

    [Space(10)]
    [Tooltip("The lower the number, the shorter the delay. 0 for instant follow.")]
    [SerializeField]float cameraDelay = .2f;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,2.5f,-9f);
    Vector3 velocity = Vector3.zero;

    [Space(10)]
    [SerializeField] bool moveDirectionChangesXOffset;
    [SerializeField] float moveDirectionXOffset;

    [Header("Juice")]
    [SerializeField] MMFeedbacks screenShakeFeedback;

    [Space(10)]

    [SerializeField] float shakeTime;
    [SerializeField] Vector2 shakeFrequency;
    [SerializeField] Vector3 minAmplitude;
    [SerializeField] Vector3 maxAmplitude;


    private void Start()
    {
        originalPosition = cameraTarget.transform.position;
    }

    void FixedUpdate()
    {
        CameraLogic();
    }

    void CameraLogic()
    {
        float effectiveXOffset = cameraOffset.x;
        if (moveDirectionChangesXOffset)
        {
            if(playerController.IsFacingRight)
            {
                effectiveXOffset = moveDirectionXOffset;
            }

            else
            {
                effectiveXOffset = -moveDirectionXOffset;
            }
        }

        Vector3 desiredPosition = new Vector3(cameraTarget.position.x + effectiveXOffset, cameraTarget.position.y + cameraOffset.y, cameraTarget.position.z + cameraOffset.z);
        if(useCameraBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, cameraXBounds.x, cameraXBounds.y);
        }

        cameraObject.transform.position = Vector3.SmoothDamp(cameraObject.transform.position, desiredPosition, ref velocity, cameraDelay);



    }

    public void ShakeCamera(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ShakeCamera();
        }
    }

    void ShakeCamera()
    {
        if(screenShakeFeedback.TryGetComponent(out MMFeedbackWiggle wiggle))
        {
            wiggle.WigglePositionDuration = shakeTime;
            wiggle.TargetWiggle.PositionWiggleProperties.AmplitudeMin = minAmplitude;
            wiggle.TargetWiggle.PositionWiggleProperties.AmplitudeMax = maxAmplitude;
            wiggle.TargetWiggle.PositionWiggleProperties.FrequencyMin = shakeFrequency.x;
            wiggle.TargetWiggle.PositionWiggleProperties.FrequencyMax = shakeFrequency.y;
            screenShakeFeedback.Initialization();
            screenShakeFeedback.PlayFeedbacks();
        }
    }

    void ShakeCameraOneTime(float time,Vector3 minPos, Vector3 maxPos)
    {
        if (screenShakeFeedback.TryGetComponent(out MMFeedbackWiggle wiggle))
        {
            wiggle.WigglePositionDuration = time;
            wiggle.TargetWiggle.PositionWiggleProperties.AmplitudeMin = minPos;
            wiggle.TargetWiggle.PositionWiggleProperties.AmplitudeMax = maxPos;
            wiggle.TargetWiggle.PositionWiggleProperties.FrequencyMin = shakeFrequency.x;
            wiggle.TargetWiggle.PositionWiggleProperties.FrequencyMax = shakeFrequency.y;
            screenShakeFeedback.Initialization();
            screenShakeFeedback.PlayFeedbacks();
        }
    }
}
